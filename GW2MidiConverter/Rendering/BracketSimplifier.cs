using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GW2MidiConverter.Rendering;

public class BracketSimplifier
{
    private readonly List<BracketPair> _bracketPairs;

    public BracketSimplifier(List<BracketPair> bracketPairs)
    {
        ArgumentNullException.ThrowIfNull(bracketPairs);
        if (bracketPairs.Count == 0) throw new ArgumentException("Bracket pairs must not be empty.", nameof(bracketPairs));

        _bracketPairs = bracketPairs;
    }

    public string Simplify(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return input;

        // Example: "[1] (2) [3] [4] 5" -> "[1] (2) [3 4] 5"

        // Find any closing bracket -> In this case the ']' at index 2.
        // Print the string until index 1. (bracket - 1).

        // Find the next opening bracket -> In this case the '(' at index 4.
        // Do they match? If not, print until index 4.
        // If they match, print until index 3 and continue at index 5. (Ignore the bracket basically.)

        // Repeat from the last index until no more brackets are found.

        StringBuilder builder = new StringBuilder();
        int currentIndex = 0;
        while (currentIndex < input.Length)
        {
            int nextClosingBracket = FindAnyNextClosingBracket(input, currentIndex);
            if (nextClosingBracket < 0)
            {
                builder.Append(input[currentIndex..]);
                break;
            }
            
            builder.Append(input.Substring(currentIndex, nextClosingBracket - currentIndex));
            currentIndex = nextClosingBracket;
            
            int nextOpeningBracket = FindAnyNextOpeningBracket(input, currentIndex);
            if (nextOpeningBracket < 0)
            {
                builder.Append(input[currentIndex..]);
                break;
            }
            
            BracketPair closingPair = GetBracketPairFromBracket(input[nextClosingBracket]);
            BracketPair openingPair = GetBracketPairFromBracket(input[nextOpeningBracket]);
            bool bracketsMatch = closingPair.Equals(openingPair);
            if (bracketsMatch)
            {
                // Find where original closing is
                // 
                builder.Append(input[(currentIndex + 1) .. nextOpeningBracket]);
                currentIndex = nextOpeningBracket + 1;
            }
            else
            {
                builder.Append(input.Substring(currentIndex, nextOpeningBracket - currentIndex + 1));
                currentIndex = nextOpeningBracket + 1;
            }
        }
        return builder.ToString();
    }

    private int FindAnyNextOpeningBracket(string input, int startIndex)
    {
        char[] brackets = _bracketPairs.Select(b => b.OpeningBracket).ToArray();
        return input.IndexOfAny(brackets, startIndex);
    }
    
    private int FindAnyNextClosingBracket(string input, int startIndex)
    {
        char[] brackets = _bracketPairs.Select(b => b.ClosingBracket).ToArray();
        return input.IndexOfAny(brackets, startIndex);
    }

    private BracketPair GetBracketPairFromBracket(char bracket)
    {
        BracketPair? pair = _bracketPairs.FirstOrDefault(b => b.Brackets.Contains(bracket));
        if (pair == null) throw new InvalidOperationException($"No bracket pair found for the given bracket '{bracket}'.");
        return pair;
    }
}