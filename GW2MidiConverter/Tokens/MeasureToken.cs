using GW2MidiConverter.Midi;

namespace GW2MidiConverter.Tokens;

public class MeasureToken(long startTime, long endTime, TimeSignature? newTimeSignature) : Token(startTime, endTime)
{
    public TimeSignature? NewTimeSignature { get; } = newTimeSignature;

    public bool HasTimeSignatureChange => NewTimeSignature != null;
}