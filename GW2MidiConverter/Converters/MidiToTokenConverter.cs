using System;
using System.Collections.Generic;
using System.Linq;
using GW2MidiConverter.Midi;
using NAudio.Midi;

namespace GW2MidiConverter.Converters;

public class MidiToTokenConverter
{
    private readonly TokenConverterSettings _settings;
    private readonly MidiDocument _midiDocument;
    private readonly MidiTimingMap _timingMap;

    public MidiToTokenConverter(MidiDocument midiDocument, TokenConverterSettings? settings = null)
    {
        ArgumentNullException.ThrowIfNull(midiDocument);
        ArgumentNullException.ThrowIfNull(midiDocument.GetRawMidiFile().Events);
        
        _midiDocument = midiDocument;
        _timingMap = MidiTimingMap.FromMidiFile(_midiDocument.GetRawMidiFile());
        _settings = settings ?? new TokenConverterSettings();
    }

    public Dictionary<int, MidiTrack> ConvertTracks(List<int> trackIndexes)
    {
        if (trackIndexes.Count == 0) throw new ArgumentException("At least one track index must be specified.", nameof(trackIndexes));
        if (trackIndexes.Any(t => t < 0 || t >= _midiDocument.GetRawMidiFile().Tracks)) throw new ArgumentOutOfRangeException(nameof(trackIndexes), "One or more track indexes are out of range.");

        List<int> tracks = trackIndexes.Where(i => _midiDocument.InstrumentTrackIndexes.Contains(i)).ToList();
        return tracks.ToDictionary(track => track, ConvertTrack);
    }

    private MidiTrack ConvertTrack(int trackIndex)
    {
        // We really only care about note events.
        List<NoteOnEvent> eventsOfTrack = _midiDocument.GetRawMidiFile()
                                                       .Events
                                                       .GetTrackEvents(trackIndex)
                                                       .OfType<NoteOnEvent>()
                                                       .Where(e => e.NoteLength >= _settings.MinimumNoteDuration)
                                                       .ToList();
        List<MidiMeasure> measures = [];
        foreach (MidiMeasureInfo measureInfo in _timingMap.Measures)
        {
            MidiMeasure measure = ConvertMeasure(measureInfo, eventsOfTrack);
            measures.Add(measure);
        }

        MidiTrackInfo info = GetMidiTrackInfoForTrack(trackIndex);
        return new MidiTrack(info, measures.ToArray());
    }

    private MidiTrackInfo GetMidiTrackInfoForTrack(int trackIndex)
    {
        TextEvent? trackNameEvent = _midiDocument.GetRawMidiFile()
                                                 .Events
                                                 .GetTrackEvents(trackIndex)
                                                 .OfType<TextEvent>()
                                                 .FirstOrDefault(t => t.MetaEventType == MetaEventType.SequenceTrackName);
        string trackName = trackNameEvent == null ? string.Empty : trackNameEvent.Text;
        return new MidiTrackInfo(trackIndex, trackName);
    }

    private MidiMeasure ConvertMeasure(MidiMeasureInfo midiMeasureInfo, List<NoteOnEvent> noteEventsInTrack)
    {
        List<long> eventTimesInMeasure = noteEventsInTrack.Where(e => e.AbsoluteTime >= midiMeasureInfo.StartTick && e.AbsoluteTime < midiMeasureInfo.EndTick)
                                                      .Select(e => e.AbsoluteTime)
                                                      .Distinct().Order().ToList();
        List<MidiNoteGroup> noteGroups = [];
        foreach (long time in eventTimesInMeasure)
        {
            List<NoteOnEvent> eventsAtTime = noteEventsInTrack.Where(e => e.AbsoluteTime == time).ToList();
            MidiNoteGroup group = ConvertNoteEvents(eventsAtTime);
            noteGroups.Add(group);
        }

        return new MidiMeasure(midiMeasureInfo, noteGroups.ToArray());
    }

    private MidiNoteGroup ConvertNoteEvents(List<NoteOnEvent> events)
    {
        bool areNotesOfSameLength = events.All(e => e.NoteLength == events.First().NoteLength);
        if (!areNotesOfSameLength)
        {
            // TODO MP: Add warning.
        }

        // For chords, we can use any of the played notes for time information.
        NoteOnEvent firstEvent = events.First();
        MidiNoteNumber[] pitches = events.Select(e => (MidiNoteNumber)e.NoteNumber).Distinct().Order().ToArray();
        SongPosition position = _timingMap.GetPositionAtTick(firstEvent.AbsoluteTime);
        MidiNoteLength length = MidiNoteLength.FromTicks(firstEvent.NoteLength, _midiDocument.GetRawMidiFile().DeltaTicksPerQuarterNote);
        
        return new MidiNoteGroup(pitches, position, length);
    }
}