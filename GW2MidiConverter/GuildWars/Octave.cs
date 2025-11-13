using System;
using System.ComponentModel;

namespace GW2MidiConverter.GuildWars;

[TypeConverter(typeof(OctaveTypeConverter))]
public readonly record struct Octave(int Value) : IComparable<Octave>
{
    public static implicit operator int(Octave octave) => octave.Value;
    public static implicit operator Octave(int value) => new Octave(value);

    public int CompareTo(Octave other)
    {
        return Value.CompareTo(other.Value);
    }

    public static bool operator <(Octave left, Octave right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(Octave left, Octave right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(Octave left, Octave right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(Octave left, Octave right)
    {
        return left.CompareTo(right) >= 0;
    }

    public int ToInt32()
    {
        return Value;
    }

    public static Octave FromInt32(int value)
    {
        return new Octave(value);
    }
}