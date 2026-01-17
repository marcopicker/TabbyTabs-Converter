using System;
using System.ComponentModel;
using System.Globalization;
using BardicTabConverter.Midi;

namespace BardicTabConverter.Tab;

[TypeConverter(typeof(TabElementLengthConverter))]
public readonly struct TabElementLength(decimal beats) : IComparable<TabElementLength>, IEquatable<TabElementLength>
{
    public decimal Beats { get; } = beats;

    public static implicit operator decimal(TabElementLength length) => length.Beats;
    public static implicit operator TabElementLength(decimal value) => new TabElementLength(value);

    public static TabElementLength FromMidiData(MidiTimeSpan timeSpan, int ticksPerQuarterNote, TimeSignature timeSignature)
    {
        decimal quarterNotes = (decimal)timeSpan.TickLength / ticksPerQuarterNote;
        decimal quarterNotesPerBeat = (decimal)4 / timeSignature.Denominator;
        decimal beats = quarterNotes / quarterNotesPerBeat;
        return new TabElementLength(beats);
    }

    public override string ToString()
    {
        return Beats.ToString("0.00", CultureInfo.InvariantCulture);
    }

    public int CompareTo(TabElementLength other)
    {
        return Beats.CompareTo(other.Beats);
    }

    public decimal ToDecimal()
    {
        return Beats;
    }

    public static TabElementLength FromDecimal(decimal value)
    {
        return new TabElementLength(value);
    }

    public override bool Equals(object? obj)
    {
        if (obj is not TabElementLength other) return false;
        return Beats == other.Beats;
    }

    public override int GetHashCode()
    {
        return Beats.GetHashCode();
    }

    public static bool operator ==(TabElementLength left, TabElementLength right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(TabElementLength left, TabElementLength right)
    {
        return !(left == right);
    }

    public static bool operator <(TabElementLength left, TabElementLength right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(TabElementLength left, TabElementLength right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(TabElementLength left, TabElementLength right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(TabElementLength left, TabElementLength right)
    {
        return left.CompareTo(right) >= 0;
    }

    public bool Equals(TabElementLength other)
    {
        return Beats == other.Beats;
    }
}