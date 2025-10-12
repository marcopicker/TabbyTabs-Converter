using GW2MidiConverter.Midi;

namespace GW2MidiConverter.Converters.Tokens.Midi;

public class MeasureMidiToken(long startTime, long endTime, TimeSignature? newTimeSignature, MeasureTokenType type) : MidiToken(startTime, endTime)
{
    public TimeSignature? NewTimeSignature { get; } = newTimeSignature;
    
    public MeasureTokenType Type { get; } = type;

    public bool HasTimeSignatureChange => NewTimeSignature != null;
}