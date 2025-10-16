namespace GW2MidiConverter.Tab.Tokens;

public class RestToken(TabElementLength length) : TabToken
{
    public TabElementLength Length { get; } = length;

    public override string ToString()
    {
        string beatWord = Length.Beats == 1 ? "beat" : "beats";
        return $"{Length.Beats} {beatWord} of rest";
    }
}