using System.Collections.Generic;
using GW2MidiConverter.General;

namespace GW2MidiConverter.Midi;

public record MidiTrack(TrackInfo Info, IReadOnlyList<MidiMeasure> Measures)
{
    public int Length => Measures.Count;
}