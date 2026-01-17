using System;
using NAudio.Midi;

namespace BardicTabConverter.Midi;

public readonly struct TimeSignature(int numerator, int denominator) : IEquatable<TimeSignature>
{
    /// <summary>
    /// The top number of the fraction. 
    /// </summary>
    public int Numerator { get; } = numerator;
    
    /// <summary>
    /// The bottom number of the fraction.
    /// </summary>
    public int Denominator { get; } = denominator;

    public static TimeSignature FromTimeSignatureEvent(TimeSignatureEvent timeSignatureEvent)
    {
        ArgumentNullException.ThrowIfNull(timeSignatureEvent);
        return new TimeSignature(timeSignatureEvent.Numerator, (int) Math.Pow(2, timeSignatureEvent.Denominator));   
    }
    
    public static TimeSignature CommonTime => new TimeSignature(4, 4);

    public override string ToString() => $"{Numerator}/{Denominator}";

    public override bool Equals(object? obj)
    {
        return obj is TimeSignature other &&
               Numerator == other.Numerator &&
               Denominator == other.Denominator;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Numerator, Denominator);
    }

    public static bool operator ==(TimeSignature left, TimeSignature right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(TimeSignature left, TimeSignature right)
    {
        return !(left == right);
    }

    public bool Equals(TimeSignature other)
    {
        return Numerator == other.Numerator && Denominator == other.Denominator;
    }
}