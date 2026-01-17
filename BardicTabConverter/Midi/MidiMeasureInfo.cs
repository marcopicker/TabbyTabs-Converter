namespace BardicTabConverter.Midi;

// TODO MP: Refactor this to use MidiTimeSpan.
public record MidiMeasureInfo(long StartTick, long EndTick, int MeasureNumber, TimeSignature TimeSignature);