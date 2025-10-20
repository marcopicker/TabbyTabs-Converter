namespace GW2MidiConverter.Rendering;

public record BracketPair(char OpeningBracket, char ClosingBracket)
{
    public char[] Brackets => [OpeningBracket, ClosingBracket];
    public bool IsOpeningBracket(char bracket) => bracket == OpeningBracket;
    public bool IsClosingBracket(char bracket) => bracket == ClosingBracket;
};