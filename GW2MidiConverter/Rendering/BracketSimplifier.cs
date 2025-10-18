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

        StringBuilder builder = new StringBuilder();
        BracketPair? currentBracketScope = null;
        bool canBracketBeSkipped = false;
        for (int index = 0; index < input.Length; index++)
        {
            char currentCharacter = input[index];
            bool isBracket = TryGetBracketPairFromBracket(currentCharacter, out BracketPair? currentPair);
            if (!isBracket)
            {
                builder.Append(currentCharacter);
                continue;
            }

            bool isOpeningBracket = isBracket && currentPair!.IsOpeningBracket(currentCharacter); // pair is guaranteed to be not-null.
            if (currentBracketScope == null)
            {
                // We're outside a bracket-scope right now!
                if (!isOpeningBracket)
                {
                    // Example: "1 2 3]" is illegal.
                    throw new InvalidOperationException("Closing brackets cannot exist without a matching opening bracket.");
                }

                currentBracketScope = currentPair;
                if (!canBracketBeSkipped) builder.Append(currentCharacter);
                continue;
            }

            // We're inside a bracket scope.
            if (isOpeningBracket)
            {
                // Brackets cannot be nested.
                throw new InvalidOperationException("Brackets cannot be nested.");
            }

            bool bracketsMatch = currentBracketScope.IsClosingBracket(currentCharacter);
            if (!bracketsMatch)
            {
                // Example: "1 [2) 3" is illegal.
                throw new InvalidOperationException("Closing brackets must match the opening bracket.");
            }

            // We're now dealing with a matching closing bracket!
            int indexOfNextMatchingOpeningBracket = input.IndexOf(currentBracketScope.OpeningBracket, index);
            currentBracketScope = null;

            if (indexOfNextMatchingOpeningBracket < 0)
            {
                builder.Append(currentCharacter);
                continue;
            }

            string contentUntilNextOpeningBracket = input[(index + 1)..indexOfNextMatchingOpeningBracket];
            canBracketBeSkipped = contentUntilNextOpeningBracket.All(c => !char.IsNumber(c));
            if (canBracketBeSkipped)
            {
                continue;
            }

            builder.Append(currentCharacter);
        }

        return builder.ToString();
    }

    private bool TryGetBracketPairFromBracket(char bracket, out BracketPair? pair)
    {
        pair = _bracketPairs.FirstOrDefault(b => b.Brackets.Contains(bracket));
        return pair != null;
    }
}