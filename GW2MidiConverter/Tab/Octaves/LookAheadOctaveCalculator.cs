using System;
using System.Collections.Generic;
using System.Linq;
using GW2MidiConverter.GuildWars;

namespace GW2MidiConverter.Tab.Octaves;

public class LookAheadOctaveCalculator : IOctaveCalculator
{
    public Octave[] CalculateOctaves(List<GuildWarsNote[]> potentialNotes)
    {
        return potentialNotes.Select((_, i) => DetermineOctaveForIndexRecursive(potentialNotes, i))
                             .ToArray();
    }

    private static Octave DetermineOctaveForIndex(List<GuildWarsNote[]> potentialNotes, int index)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(index, 0, nameof(index));
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, potentialNotes.Count, nameof(index));

        for (int i = index; i < potentialNotes.Count; i++)
        {
            Octave[] potentialOctaves = GetOctavesOfNotes(potentialNotes[i]);

            // If we only have one option, this is an easy decision.
            if (potentialOctaves.Length == 1) return potentialOctaves[0];

            // If this is the last pair of notes, choose the octave closer to 0.
            if (i + 1 >= potentialNotes.Count) return GetCloserOctaveToMiddleOctave(potentialOctaves);

            Octave[] nextPotentialOctaves = GetOctavesOfNotes(potentialNotes[i + 1]);
            if (nextPotentialOctaves.Length == 1)
            {
                Octave nextOctave = nextPotentialOctaves[0];
                if (potentialOctaves.Contains(nextOctave)) return nextOctave;
                int octaveSwapDirection = nextOctave - potentialOctaves[0];

                // If we're going up: Prefer the upper one.
                if (octaveSwapDirection > 0) return potentialOctaves.MaxBy(n => n);

                // We're going down: Prefer the lower one.
                return potentialOctaves.MinBy(n => n);
            }

            // The next potential octave is still ambiguous. Continue the search.
        }

        return potentialNotes.Last().MinBy(n => Math.Abs(0 - n.Octave.Value)).Octave;
    }

    private static Octave DetermineOctaveForIndexRecursive(List<GuildWarsNote[]> potentialNotes, int index)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(index);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, potentialNotes.Count);

        Octave[] currentOctaves = GetOctavesOfNotes(potentialNotes[index]);

        // Only a single octave makes it easy.
        if (currentOctaves.Length == 1) return currentOctaves[0];

        // Is this the last element AND ambiguous?
        if (index == potentialNotes.Count - 1) return GetCloserOctaveToMiddleOctave(currentOctaves);

        // "index + 1" is safe, as we have already covered the case of the last element.
        Octave nextOctave = DetermineOctaveForIndexRecursive(potentialNotes, index + 1);

        // Prefer to stay in the same octave.
        if (currentOctaves.Contains(nextOctave)) return nextOctave;

        // We only get here if we're jumping to an octave we're not currently in.
        return currentOctaves.OrderBy(o => Math.Abs(o - nextOctave))
                             .ThenBy(o => Math.Abs(o))
                             .ThenBy(o => o)
                             .First();
    }

    private static Octave[] GetOctavesOfNotes(GuildWarsNote[] notes)
    {
        return notes.Select(n => n.Octave).ToArray();
    }

    private static Octave GetCloserOctaveToMiddleOctave(Octave[] octaves)
    {
        return octaves.MinBy(o => Math.Abs(0 - o.Value));
    }
}