using System;

namespace GW2MidiConverter.Midi;

public readonly record struct MidiNoteNumber
{
    public MidiNoteNumber(int value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(value, 0, nameof(value));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value, value, nameof(value));
        Value = value;
    }

    public int Value { get; }

    public static implicit operator int(MidiNoteNumber number) => number.Value;
    public static implicit operator MidiNoteNumber(int value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(value, 0, nameof(value));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value, value, nameof(value));
        return new MidiNoteNumber(value);
    }
}