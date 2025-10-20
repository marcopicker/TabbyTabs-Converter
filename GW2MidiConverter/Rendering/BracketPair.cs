using System.Collections.Generic;

namespace GW2MidiConverter.Rendering;

public record BracketPair(char OpeningBracket, char ClosingBracket)
{
    public char[] GetBracketCharacters() => [OpeningBracket, ClosingBracket];
    public bool IsOpeningBracket(char bracket) => bracket == OpeningBracket;
    public bool IsClosingBracket(char bracket) => bracket == ClosingBracket;
};