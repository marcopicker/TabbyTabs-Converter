namespace GW2MidiConverter.Midi;

public record struct MidiNoteLength(long Ticks, decimal QuarterNotes)
{
    public static MidiNoteLength FromTicks(long ticks, int resolution) => new MidiNoteLength(ticks, (decimal)ticks / resolution);
};