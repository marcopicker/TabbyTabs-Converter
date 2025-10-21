using System;
using System.Collections.Generic;
using System.Linq;
using GW2MidiConverter.General;
using GW2MidiConverter.GuildWars;
using GW2MidiConverter.GuildWars.Instruments;
using GW2MidiConverter.Midi;
using GW2MidiConverter.Tab.Octaves;
using GW2MidiConverter.Tab.Tokens;

namespace GW2MidiConverter.Tab;

internal class TrackConverter
{
    private readonly MidiTrack _track;
    private readonly MidiTimingMap _timingMap;

    private IReadOnlyList<GuildWarsNote[]> _potentialNotes = [];
    private IReadOnlyList<Octave> _octaves = [];

    private int _currentNoteIndex;

    public TrackConverter(MidiTrack track, MidiTimingMap timingMap)
    {
        ArgumentNullException.ThrowIfNull(track);
        ArgumentNullException.ThrowIfNull(timingMap);

        _track = track;
        _timingMap = timingMap;
    }

    public bool CanConvert(Instrument instrument)
    {
        ArgumentNullException.ThrowIfNull(instrument);

        List<MidiNoteNumber> allNoteNumbers = GetNoteNumbersInTrack();
        return allNoteNumbers.All(n => instrument.NoteMap.IsNoteMapped(n));
    }

    public TabTrack Convert(Instrument instrument, IOctaveCalculator octaveCalculator)
    {
        ArgumentNullException.ThrowIfNull(instrument);
        ArgumentNullException.ThrowIfNull(octaveCalculator);

        GetAllPotentialNotes(instrument);
        List<Octave[]> potentialOctaves = _potentialNotes.Select(n => n.Select(note => note.Octave).Distinct().ToArray())
                                                         .ToList();
        _octaves = octaveCalculator.CalculateOctaves(potentialOctaves);

        _currentNoteIndex = 0;
        List<TabMeasure> measures = _track.Measures.Select(m => ConvertMeasure(m, instrument.NoteMap)).ToList();
        TrackInfo trackInfo = _track.Info;
        return new TabTrack(trackInfo, measures);
    }

    private List<MidiNoteNumber> GetNoteNumbersInTrack()
    {
        List<MidiNoteNumber> allNoteNumbers = _track.Measures
                                                    .SelectMany(m => m.NoteGroups)
                                                    .SelectMany(n => n.Notes)
                                                    .ToList();
        return allNoteNumbers;
    }

    private void GetAllPotentialNotes(Instrument instrument)
    {
        List<MidiNoteNumber> allNoteNumbers = GetNoteNumbersInTrack();
        List<GuildWarsNote[]> potentialNotes = allNoteNumbers.Where(n => instrument.NoteMap.IsNoteMapped(n))
                                                             .Select(n => instrument.NoteMap.MapMidiToNotes(n))
                                                             .ToList();
        _potentialNotes = potentialNotes;
    }

    private TabMeasure ConvertMeasure(MidiMeasure midiMeasure, InstrumentNoteMap noteMap)
    {
        TimeSignature timeSignature = midiMeasure.MeasureInfo.TimeSignature;

        if (midiMeasure.IsEmpty)
        {
            SongPosition emptyMeasureSongPosition = new SongPosition(midiMeasure.MeasureInfo.MeasureNumber, 1);
            RestToken fullRest = new RestToken(emptyMeasureSongPosition, new TabElementLength(timeSignature.Denominator));
            return new TabMeasure(midiMeasure.MeasureInfo.MeasureNumber, midiMeasure.MeasureInfo.TimeSignature, [fullRest]);
        }
        
        List<TabToken> tokens = [];

        // Check for a rest at the start of this measure.
        if (midiMeasure.NoteGroups.Count > 0)
        {
            MidiNoteGroup firstGroup = midiMeasure.NoteGroups[0];
            if (TryConvertRest(midiMeasure.MeasureInfo.StartTick, firstGroup.TimeSpan.StartTick, timeSignature, out RestToken? startRest))
            {
                tokens.Add(startRest!);
            }
        }
        
        for (int i = 0; i < midiMeasure.NoteGroups.Count - 1; i++)
        {
            MidiNoteGroup group = midiMeasure.NoteGroups[i];
            MidiNoteGroup nextGroup = midiMeasure.NoteGroups[i + 1];
            
            NoteGroupToken noteGroup = ConvertNoteGroup(group, noteMap, timeSignature, midiMeasure);
            tokens.Add(noteGroup);

            if (TryConvertRest(group.TimeSpan.EndTick, nextGroup.TimeSpan.StartTick, timeSignature, out RestToken? rest))
            {
                // Guaranteed to have a value.
                tokens.Add(rest!);
            }
        }

        // Check for a rest at the end of this measure.
        MidiNoteGroup lastGroup = midiMeasure.NoteGroups[^1];
        tokens.Add(ConvertNoteGroup(lastGroup, noteMap, timeSignature, midiMeasure));
        bool doesLastNoteCarryOverIntoNextMeasure = lastGroup.TimeSpan.EndTick > midiMeasure.MeasureInfo.EndTick;
        if (!doesLastNoteCarryOverIntoNextMeasure && TryConvertRest(lastGroup.TimeSpan.EndTick, midiMeasure.MeasureInfo.EndTick, timeSignature, out RestToken? lastRest))
        {
            // Guaranteed to have a value.
            tokens.Add(lastRest!);
        }

        // _currentNoteIndex += lastGroup.Notes.Length;
        return new TabMeasure(midiMeasure.MeasureInfo.MeasureNumber, midiMeasure.MeasureInfo.TimeSignature, tokens);
    }

    private NoteGroupToken ConvertNoteGroup(MidiNoteGroup group, InstrumentNoteMap noteMap, TimeSignature timeSignature, MidiMeasure measure)
    {
        List<GuildWarsNote> notes = [];
        foreach (MidiNoteNumber midiNoteNumber in group.Notes)
        {
            if (!noteMap.IsNoteMapped(midiNoteNumber))
            {
                continue;
            }

            GuildWarsNote[] mappedNotes = noteMap.MapMidiToNotes(midiNoteNumber);
            Octave octave = _octaves[_currentNoteIndex];
            GuildWarsNote noteToAdd = mappedNotes.First(note => note.Octave == octave);
            notes.Add(noteToAdd);
            
            _currentNoteIndex++;
        }

        // Cut off notes that are tied to notes after this measure.
        MidiTimeSpan timeSpan = group.TimeSpan with { EndTick = Math.Min(group.TimeSpan.EndTick, measure.MeasureInfo.EndTick) };
        TabElementLength length = TabElementLength.FromMidiData(timeSpan, _timingMap.TicksPerQuarterNote, timeSignature);
        
        NoteGroupToken noteGroup = new NoteGroupToken(notes.ToArray(), group.Position, length);
        return noteGroup;
    }

    private bool TryConvertRest(long endOfEvent, long startOfNextEvent, TimeSignature timeSignature, out RestToken? restToken)
    {
        restToken = null;
        // TODO MP: This is a workaround for mini-rests that result from MIDI events ending one tick before the next.
        MidiTimeSpan timeSpan = new MidiTimeSpan(endOfEvent, startOfNextEvent);
        if (timeSpan.TickLength <= 10) return false;
        
        SongPosition position = _timingMap.GetPositionAtTick(endOfEvent);
        TabElementLength restLength = TabElementLength.FromMidiData(timeSpan, _timingMap.TicksPerQuarterNote, timeSignature);
        
        restToken = new RestToken(position, restLength);
        return true;
    }
}