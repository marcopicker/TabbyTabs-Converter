using System;
using System.Collections.Generic;
using System.Linq;
using GW2MidiConverter.GuildWars;
using GW2MidiConverter.GuildWars.Instruments;
using GW2MidiConverter.Midi;
using GW2MidiConverter.Shared;
using GW2MidiConverter.Tab.Octaves;
using GW2MidiConverter.Tab.Tokens;

namespace GW2MidiConverter.Tab;

internal class TrackConverter
{
    private readonly MidiTrack _track;
    private readonly int _ticksPerQuarterNote;

    private List<GuildWarsNote[]> _potentialNotes = [];
    private Octave[] _octaves = [];
    
    private int _currentNoteGroupIndex;

    public TrackConverter(MidiTrack track, int ticksPerQuarterNote)
    {
        ArgumentNullException.ThrowIfNull(track);
        ArgumentOutOfRangeException.ThrowIfLessThan(ticksPerQuarterNote, 0, nameof(ticksPerQuarterNote));

        _track = track;
        _ticksPerQuarterNote = ticksPerQuarterNote;
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
        _octaves = octaveCalculator.CalculateOctaves(_potentialNotes);

        _currentNoteGroupIndex = 0;
        List<TabMeasure> measures = _track.Measures.Select(m => ConvertMeasure(m, instrument.NoteMap)).ToList();
        TrackInfo trackInfo = _track.Info with { InstrumentName = instrument.Name };
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
        _potentialNotes.Clear();
        List<MidiNoteNumber> allNoteNumbers = GetNoteNumbersInTrack();
        List<GuildWarsNote[]> potentialNotes = allNoteNumbers.Select(n => instrument.NoteMap.MapMidiToNotes(n))
                                                             .ToList();
        _potentialNotes = potentialNotes;
    }

    private TabMeasure ConvertMeasure(MidiMeasure midiMeasure, InstrumentNoteMap noteMap)
    {
        TimeSignature timeSignature = midiMeasure.MeasureInfo.TimeSignature;

        if (midiMeasure.IsEmpty)
        {
            RestToken fullRest = new RestToken(timeSignature.Denominator);
            return new TabMeasure(midiMeasure.MeasureInfo.MeasureNumber, midiMeasure.MeasureInfo.TimeSignature, [fullRest]);
        }

        List<TabToken> tokens = [];

        for (int i = 0; i < midiMeasure.NoteGroups.Length - 1; i++)
        {
            MidiNoteGroup group = midiMeasure.NoteGroups[i];
            MidiNoteGroup nextGroup = midiMeasure.NoteGroups[i + 1];

            NoteGroupToken noteGroup = ConvertNoteGroup(group, noteMap, timeSignature);
            tokens.Add(noteGroup);

            if (TryConvertRest(group.TimeSpan.EndTick, nextGroup.TimeSpan.StartTick, timeSignature, out RestToken? rest))
            {
                // Guaranteed to have a value.
                tokens.Add(rest!);
            }

            _currentNoteGroupIndex++;
        }

        MidiNoteGroup lastGroup = midiMeasure.NoteGroups.Last();
        tokens.Add(ConvertNoteGroup(lastGroup, noteMap, timeSignature));
        if (TryConvertRest(lastGroup.TimeSpan.EndTick, midiMeasure.MeasureInfo.EndTick, timeSignature, out RestToken? lastRest))
        {
            // Guaranteed to have a value.
            tokens.Add(lastRest!);
        }

        _currentNoteGroupIndex++;
        return new TabMeasure(midiMeasure.MeasureInfo.MeasureNumber, midiMeasure.MeasureInfo.TimeSignature, tokens);
    }

    private NoteGroupToken ConvertNoteGroup(MidiNoteGroup group, InstrumentNoteMap noteMap, TimeSignature timeSignature)
    {
        GuildWarsNote[] notes = group.Notes.Where(noteMap.IsNoteMapped)
                                    .Select(noteMap.MapMidiToNotes)
                                    .Select(n => n.First(note => note.Octave == _octaves[_currentNoteGroupIndex]))
                                    .ToArray();
        TabElementLength length = TabElementLength.FromMidiData(group.TimeSpan, _ticksPerQuarterNote, timeSignature);
        NoteGroupToken noteGroup = new NoteGroupToken(notes, length);
        return noteGroup;
    }

    private bool TryConvertRest(long endOfEvent, long startOfNextEvent, TimeSignature timeSignature, out RestToken? restToken)
    {
        restToken = null;
        MidiTimeSpan timeSpan = new MidiTimeSpan(endOfEvent, startOfNextEvent);
        if (timeSpan.Length == 0) return false;
        TabElementLength restLength = TabElementLength.FromMidiData(timeSpan, _ticksPerQuarterNote, timeSignature);
        restToken = new RestToken(restLength);
        return true;
    }
}