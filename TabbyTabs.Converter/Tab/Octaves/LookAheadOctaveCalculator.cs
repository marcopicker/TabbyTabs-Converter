using System;
using System.Collections.Generic;
using System.Linq;
using TabbyTabs.Converter.GuildWars;

namespace TabbyTabs.Converter.Tab.Octaves;

public class LookAheadOctaveCalculator : IOctaveCalculator
{
    public IReadOnlyList<Octave> CalculateOctaves(IReadOnlyList<Octave[]> potentialOctaves)
    {
        return potentialOctaves.Select((_, i) => DetermineOctaveForIndexRecursive(potentialOctaves, i)).ToList();
    }

    private static Octave DetermineOctaveForIndexRecursive(IReadOnlyList<Octave[]> potentialOctaves, int index)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(index);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, potentialOctaves.Count);

        Octave[] currentOctaves = potentialOctaves[index];

        // Only a single octave makes it easy.
        if (currentOctaves.Length == 1) return currentOctaves[0];

        // Is this the last element AND ambiguous?
        if (index == potentialOctaves.Count - 1) return GetCloserOctaveToMiddleOctave(currentOctaves);

        // "index + 1" is safe, as we have already covered the case of the last element.
        Octave nextOctave = DetermineOctaveForIndexRecursive(potentialOctaves, index + 1);

        // Prefer to stay in the same octave.
        if (currentOctaves.Contains(nextOctave)) return nextOctave;

        // We only get here if we're jumping to an octave we're not currently in.
        return currentOctaves.OrderBy(o => Math.Abs(o - nextOctave))
                             .ThenBy(o => Math.Abs(o))
                             .ThenBy(o => o)
                             .First();
    }

    private static Octave GetCloserOctaveToMiddleOctave(Octave[] octaves)
    {
        return octaves.MinBy(o => Math.Abs(0 - o.Value));
    }
}