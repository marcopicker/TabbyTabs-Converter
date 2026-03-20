using System;
using System.Collections.Generic;
using System.Linq;
using TabbyTabs.Converter.General;
using TabbyTabs.Converter.GuildWars;
using TabbyTabs.Converter.GuildWars.Instruments;
using TabbyTabs.Converter.Midi;
using TabbyTabs.Converter.Tab.Octaves;
using TabbyTabs.Converter.Tab.Tokens;

namespace TabbyTabs.Converter.Tab;

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

        return _track.Measures
                     .SelectMany(m => m.NoteGroups)
                     .SelectMany(n => n.Notes)
                     .All(instrument.CanPlayNote);
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
        List<TabMeasure> measures = _track.Measures.Select(m => ConvertMeasure(m, instrument)).ToList();
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
        List<GuildWarsNote[]> potentialNotes = allNoteNumbers.Where(instrument.CanPlayNote)
                                                             .Select(instrument.MapMidiToNotes)
                                                             .ToList();
        _potentialNotes = potentialNotes;
    }

    private TabMeasure ConvertMeasure(MidiMeasure midiMeasure, Instrument instrument)
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

            NoteGroupToken noteGroup = ConvertNoteGroup(group, instrument, timeSignature, midiMeasure);
            tokens.Add(noteGroup);

            if (TryConvertRest(group.TimeSpan.EndTick, nextGroup.TimeSpan.StartTick, timeSignature, out RestToken? rest))
            {
                // Guaranteed to have a value.
                tokens.Add(rest!);
            }
        }

        // Check for a rest at the end of this measure.
        MidiNoteGroup lastGroup = midiMeasure.NoteGroups[^1];
        tokens.Add(ConvertNoteGroup(lastGroup, instrument, timeSignature, midiMeasure));
        bool doesLastNoteCarryOverIntoNextMeasure = lastGroup.TimeSpan.EndTick > midiMeasure.MeasureInfo.EndTick;
        if (!doesLastNoteCarryOverIntoNextMeasure && TryConvertRest(lastGroup.TimeSpan.EndTick, midiMeasure.MeasureInfo.EndTick, timeSignature, out RestToken? lastRest))
        {
            // Guaranteed to have a value.
            tokens.Add(lastRest!);
        }

        // _currentNoteIndex += lastGroup.Notes.Length;
        return new TabMeasure(midiMeasure.MeasureInfo.MeasureNumber, midiMeasure.MeasureInfo.TimeSignature, tokens);
    }

    private NoteGroupToken ConvertNoteGroup(MidiNoteGroup group, Instrument instrument, TimeSignature timeSignature, MidiMeasure measure)
    {
        List<GuildWarsNote> notes = [];
        foreach (MidiNoteNumber midiNoteNumber in group.Notes)
        {
            if (!instrument.CanPlayNote(midiNoteNumber))
            {
                continue;
            }

            GuildWarsNote[] mappedNotes = instrument.MapMidiToNotes(midiNoteNumber);
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