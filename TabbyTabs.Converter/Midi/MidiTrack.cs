using System.Collections.Generic;
using TabbyTabs.Converter.General;

namespace TabbyTabs.Converter.Midi;

public record MidiTrack(TrackInfo Info, IReadOnlyList<MidiMeasure> Measures)
{
    public int Length => Measures.Count;
}