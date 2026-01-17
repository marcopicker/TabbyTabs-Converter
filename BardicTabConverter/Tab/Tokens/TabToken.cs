using BardicTabConverter.Midi;

namespace BardicTabConverter.Tab.Tokens;

public abstract record TabToken(SongPosition Position, TabElementLength Length)
{
    public void Deconstruct(out SongPosition position, out TabElementLength length)
    {
        position = Position;
        length = Length;
    }
}