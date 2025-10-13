namespace GW2MidiConverter.Midi;

public record MidiTrack(MidiTrackInfo Info, MidiMeasure[] Measures)
{
    public int Length => Measures.Length;
}