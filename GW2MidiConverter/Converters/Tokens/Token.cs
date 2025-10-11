namespace GW2MidiConverter.Converters.Tokens;

public abstract class Token(long startTime, long endTime)
{
    public long StartTime { get; } = startTime;
    public long EndTime { get; } = endTime;
}