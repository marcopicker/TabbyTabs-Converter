using System;
using NAudio.Midi;

namespace GW2MidiConverter.Midi;

public class TimeSignature(int numerator, int denominator)
{
    public readonly int Numerator = numerator;
    public readonly int Denominator = denominator;

    public static TimeSignature FromTimeSignatureEvent(TimeSignatureEvent timeSignatureEvent)
    {
        return new TimeSignature(timeSignatureEvent.Numerator, (int) Math.Pow(2, timeSignatureEvent.Denominator));   
    }

    public override string ToString() => $"{Numerator}/{Denominator}";

    public override bool Equals(object? obj)
    {
        return obj is TimeSignature other &&
               Numerator == other.Numerator &&
               Denominator == other.Denominator;
    }
}