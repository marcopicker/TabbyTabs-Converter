namespace GW2MidiConverter.Converters.Tokens.Midi;

public abstract class MidiToken(long startTime, long endTime)
{
    public long StartTime { get; } = startTime;
    public long EndTime { get; } = endTime;
}