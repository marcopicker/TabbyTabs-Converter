using System.Collections.Generic;
using BardicTabConverter.General;

namespace BardicTabConverter.Midi;

public record MidiTrack(TrackInfo Info, IReadOnlyList<MidiMeasure> Measures)
{
    public int Length => Measures.Count;
}