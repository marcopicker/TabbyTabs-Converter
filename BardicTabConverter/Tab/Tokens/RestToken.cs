using BardicTabConverter.Midi;

namespace BardicTabConverter.Tab.Tokens;

public record RestToken(SongPosition Position, TabElementLength Length) : TabToken(Position, Length)
{
    public override string ToString()
    {
        string beatWord = Length.Beats == 1 ? "beat" : "beats";
        return $"{Length.Beats} {beatWord} of rest";
    }
}