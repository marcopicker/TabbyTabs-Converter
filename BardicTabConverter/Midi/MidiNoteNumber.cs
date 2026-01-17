using System;

namespace BardicTabConverter.Midi;

public readonly record struct MidiNoteNumber : IComparable<MidiNoteNumber>
{
    private static readonly string[] NoteNames =
    [
        "C",
        "C#",
        "D",
        "D#",
        "E",
        "F",
        "F#",
        "G",
        "G#",
        "A",
        "A#",
        "B"
    ];

    public MidiNoteNumber(int value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(value, 0, nameof(value));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value, value, nameof(value));
        Value = value;
    }

    public int Value { get; } = -1;

    public override string ToString()
    {
        string name = NoteNames[Value % 12];
        int octave = Value / 12;
        return $"{Value} ({name}{octave})";
    }

    public static implicit operator int(MidiNoteNumber number) => number.Value;

    public static implicit operator MidiNoteNumber(int value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(value, 0, nameof(value));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value, value, nameof(value));
        return new MidiNoteNumber(value);
    }

    public int CompareTo(MidiNoteNumber other)
    {
        return Value.CompareTo(other.Value);
    }

    public static bool operator <(MidiNoteNumber left, MidiNoteNumber right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(MidiNoteNumber left, MidiNoteNumber right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(MidiNoteNumber left, MidiNoteNumber right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(MidiNoteNumber left, MidiNoteNumber right)
    {
        return left.CompareTo(right) >= 0;
    }

    public int ToInt32()
    {
        return Value;
    }

    public static MidiNoteNumber FromInt32(int value)
    {
        return new MidiNoteNumber(value);
    }
}