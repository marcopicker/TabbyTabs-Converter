using GW2MidiConverter.Midi;

namespace GW2MidiConverter.Converters.Tokens;

public class MeasureToken(long startTime, long endTime, TimeSignature? newTimeSignature, MeasureTokenType type) : Token(startTime, endTime)
{
    public TimeSignature? NewTimeSignature { get; } = newTimeSignature;
    
    public MeasureTokenType Type { get; } = type;

    public bool HasTimeSignatureChange => NewTimeSignature != null;
}