namespace GW2MidiConverter.Midi;

public record MidiMeasureSpan(long StartTick, long EndTick, TimeSignature TimeSignature);