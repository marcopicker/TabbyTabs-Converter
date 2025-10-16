using System;
using System.Collections.Generic;
using System.Linq;
using GW2MidiConverter.GuildWars;
using GW2MidiConverter.GuildWars.Instruments;
using GW2MidiConverter.Midi;
using GW2MidiConverter.Shared;
using GW2MidiConverter.Tab.Tokens;

namespace GW2MidiConverter.Tab;

public class MidiDocumentConverter
{
    private readonly MidiDocument _midiDocument;
    private Octave _currentOctave = new Octave(0);

    public MidiDocumentConverter(MidiDocument midiDocument)
    {
        ArgumentNullException.ThrowIfNull(midiDocument);
        _midiDocument = midiDocument;
    }

    public TabTrack ConvertTrack(int trackIndex, InstrumentNoteMap noteMap, string? trackName = null)
    {
        if (!_midiDocument.InstrumentTrackIndexes.Contains(trackIndex)) throw new ArgumentOutOfRangeException(nameof(trackIndex), "The given track index is not an instrument track.");

        MidiTrack midiTrack = _midiDocument.GetTrack(trackIndex);
        if (midiTrack.Length == 0) return new TabTrack(midiTrack.Info, []);

        List<TabMeasure> measures = midiTrack.Measures.Select(m => ConvertMeasure(m, noteMap)).ToList();
        TrackInfo trackInfo = string.IsNullOrWhiteSpace(trackName) ? midiTrack.Info : midiTrack.Info with { InstrumentName = trackName };
        return new TabTrack(trackInfo, measures);
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
        }
        
        MidiNoteGroup lastGroup = midiMeasure.NoteGroups.Last();
        tokens.Add(ConvertNoteGroup(lastGroup, noteMap, timeSignature));
        if (TryConvertRest(lastGroup.TimeSpan.EndTick, midiMeasure.MeasureInfo.EndTick, timeSignature, out RestToken? lastRest))
        {
            // Guaranteed to have a value.
            tokens.Add(lastRest!);
        }
        
        return new TabMeasure(midiMeasure.MeasureInfo.MeasureNumber, midiMeasure.MeasureInfo.TimeSignature, tokens);
    }

    private NoteGroupToken ConvertNoteGroup(MidiNoteGroup group, InstrumentNoteMap noteMap, TimeSignature timeSignature)
    {
        // TODO MP: This is disgusting. It should not always default to the lower octave.
        GuildWarsNote[] notes = group.Notes.Where(noteMap.IsNoteMapped)
                                     .Select(n => noteMap.MapMidiToNotes(n)[0])
                                     .ToArray();
        TabElementLength length = TabElementLength.FromMidiData(group.TimeSpan, _midiDocument.TicksPerQuarterNote, timeSignature);
        NoteGroupToken noteGroup = new NoteGroupToken(notes, length);
        return noteGroup;
    }

    private bool TryConvertRest(long endOfEvent, long startOfNextEvent, TimeSignature timeSignature, out RestToken? restToken)
    {
        restToken = null;
        MidiTimeSpan timeSpan = new MidiTimeSpan(endOfEvent, startOfNextEvent);
        if(timeSpan.Length == 0) return false;
        TabElementLength restLength = TabElementLength.FromMidiData(timeSpan, _midiDocument.TicksPerQuarterNote, timeSignature);
        restToken = new RestToken(restLength);
        return true;
    }
}