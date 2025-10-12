using System;

namespace GW2MidiConverter.Midi;

public readonly record struct MidiNoteNumber
{
    public MidiNoteNumber(byte value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(0, value, nameof(value));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(127, value, nameof(value));
        Value = value;
    }

    public byte Value { get; }

    public static implicit operator byte(MidiNoteNumber number) => number.Value;
    public static implicit operator MidiNoteNumber(byte value)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(127, value, nameof(value));
        return new MidiNoteNumber(value);
    }

    public static implicit operator MidiNoteNumber(int value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(0, value, nameof(value));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(127, value, nameof(value));
        return new MidiNoteNumber((byte)value);
    }
}