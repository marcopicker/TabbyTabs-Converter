namespace GW2MidiConverter.Midi;

internal static class MidiUtilities
{
    public static long CalculateTicksForMeasure(TimeSignature timeSignature, long ticksPerQuarterNote)
    {
        int beats = timeSignature.Numerator;
        decimal ticksPerBeat = ticksPerQuarterNote * (4.0m / timeSignature.Denominator);
        return (long)(ticksPerBeat * beats);
    }
}