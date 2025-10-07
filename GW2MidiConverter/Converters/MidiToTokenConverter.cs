using System;
using System.Collections.Generic;
using System.Linq;
using GW2MidiConverter.Midi;
using GW2MidiConverter.Tokens;
using NAudio.Midi;

namespace GW2MidiConverter;

public class MidiToTokenConverter
{
    private readonly MidiConverterSettings _settings;

    public MidiToTokenConverter(MidiConverterSettings? settings = null)
    {
        _settings = settings ?? new MidiConverterSettings();
    }

    public Dictionary<int, List<IToken>> ConvertTracks(MidiDocument midiDocument, params int[] trackIndexes)
    {
        ArgumentNullException.ThrowIfNull(midiDocument);
        if (trackIndexes.Length == 0) throw new ArgumentException("At least one track index must be specified.", nameof(trackIndexes));
        if (midiDocument.InstrumentCount == 0) throw new InvalidMidiDocumentException("The MIDI document must contain at least one instrument.");

        Dictionary<int, List<IToken>> output = new Dictionary<int, List<IToken>>();
        foreach (int index in trackIndexes)
        {
            List<IToken> tokens = ConvertTrack(midiDocument, index);
            if (tokens.Count > 0) output.Add(index, tokens);
        }

        return output;
    }

    private List<IToken> ConvertTrack(MidiDocument midiDocument, int trackIndex)
    {
        if (trackIndex < 0 || trackIndex >= midiDocument.GetRawMidiFile().Tracks) throw new IndexOutOfRangeException(nameof(trackIndex));
        if (!midiDocument.InstrumentTrackIndexes.Contains(trackIndex)) return [];

        List<MidiEvent>? events = midiDocument.GetRawMidiFile().Events?.GetTrackEvents(trackIndex).ToList();
        if (events == null) throw new Exception("Could not get events for track.");

        List<long> eventTimes = events.Select(e => e.AbsoluteTime).Distinct().ToList();
        return [];
    }
}