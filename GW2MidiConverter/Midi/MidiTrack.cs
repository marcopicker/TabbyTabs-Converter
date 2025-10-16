using GW2MidiConverter.Shared;

namespace GW2MidiConverter.Midi;

public record MidiTrack(TrackInfo Info, MidiMeasure[] Measures)
{
    public int Length => Measures.Length;
}