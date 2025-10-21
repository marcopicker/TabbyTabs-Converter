using GW2MidiConverter.Midi;

namespace GW2MidiConverter.Tab.Tokens;

public class RestToken(SongPosition position, TabElementLength length) : TabToken(position, length)
{
    public override string ToString()
    {
        string beatWord = Length.Beats == 1 ? "beat" : "beats";
        return $"{Length.Beats} {beatWord} of rest";
    }
}