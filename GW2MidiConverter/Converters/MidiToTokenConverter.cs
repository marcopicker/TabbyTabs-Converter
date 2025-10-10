using System;
using System.Collections.Generic;
using System.Linq;
using GW2MidiConverter.Midi;
using GW2MidiConverter.Tokens;
using NAudio.Midi;

namespace GW2MidiConverter;

public class MidiToTokenConverter(MidiConverterSettings? settings = null)
{
    private readonly MidiConverterSettings _settings = settings ?? new MidiConverterSettings();

    public Dictionary<int, List<Token>> ConvertTracks(MidiDocument midiDocument, params int[] trackIndexes)
    {
        ArgumentNullException.ThrowIfNull(midiDocument);
        if (trackIndexes.Length == 0) throw new ArgumentException("At least one track index must be specified.", nameof(trackIndexes));
        if (midiDocument.InstrumentCount == 0) throw new InvalidMidiDocumentException("The MIDI document must contain at least one instrument.");

        MidiTimingMap timingMap = MidiTimingMap.FromMidiFile(midiDocument.GetRawMidiFile());
        
        Dictionary<int, List<Token>> output = new Dictionary<int, List<Token>>();
        foreach (int index in trackIndexes)
        {
            if (!midiDocument.InstrumentTrackIndexes.Contains(index)) continue;
            List<Token> tokens = ConvertTrack(midiDocument, index, timingMap);
            output.Add(index, tokens);
        }

        return output;
    }

    private List<Token> ConvertTrack(MidiDocument midiDocument, int trackIndex, MidiTimingMap timingMap)
    {
        if (trackIndex < 0 || trackIndex >= midiDocument.GetRawMidiFile().Tracks) throw new IndexOutOfRangeException(nameof(trackIndex));
        List<MidiEvent>? events = midiDocument.GetRawMidiFile().Events?.GetTrackEvents(trackIndex).ToList();
        if (events == null) throw new Exception("Could not get events for track.");

        List<long> eventTimes = events.Select(e => e.AbsoluteTime).Distinct().Order().ToList();

        
        return [];
    }
}