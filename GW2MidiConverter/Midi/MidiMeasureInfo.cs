namespace GW2MidiConverter.Midi;

public record MidiMeasureInfo(long StartTick, long EndTick, int MeasureNumber, TimeSignature TimeSignature);