using GW2MidiConverter.Midi;

namespace GW2MidiConverter.Tab.Tokens;

public abstract class TabToken(SongPosition position, TabElementLength length)
{
    public SongPosition Position { get; } = position;
    public TabElementLength Length { get; } = length;
}