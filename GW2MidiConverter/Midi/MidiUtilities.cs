namespace GW2MidiConverter.Midi;

internal static class MidiUtilities
{
    public static long CalculateTicksForMeasure(TimeSignature timeSignature, long ticksPerQuarterNote)
    {
        int beats = timeSignature.Numerator;
        long ticksPerBeat = CalculateTicksPerBeat(timeSignature, ticksPerQuarterNote);
        return ticksPerBeat * beats;
    }

    public static long CalculateTicksPerBeat(TimeSignature timeSignature, long ticksPerQuarterNote)
    {
        return (long)(ticksPerQuarterNote * (4.0m / timeSignature.Denominator));
    }
}