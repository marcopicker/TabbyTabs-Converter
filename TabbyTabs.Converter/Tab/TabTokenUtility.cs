using System;
using TabbyTabs.Converter.Midi;
using TabbyTabs.Converter.Tab.Tokens;

namespace TabbyTabs.Converter.Tab;

internal static class TabTokenUtility
{
    public static bool TryCutTokenToFitBeat<T>(T token, int beat, out T? cutToken) where T : TabToken
    {
        ArgumentNullException.ThrowIfNull(token);

        decimal tokenStartBeat = token.Position.Beat + token.Position.BeatFraction;
        decimal tokenEndBeat = tokenStartBeat + token.Length.Beats;

        // Token ends before this beat.
        if (tokenEndBeat <= beat)
        {
            cutToken = null;
            return false;
        }

        // Token starts after this beat.
        if (tokenStartBeat >= beat + 1)
        {
            cutToken = null;
            return false;
        }

        decimal cutStartingBeat = Math.Max(beat, tokenStartBeat);
        SongPosition newPosition = new SongPosition(token.Position.Measure, cutStartingBeat);
        decimal cutLength = Math.Min(tokenEndBeat, beat + 1) - cutStartingBeat;

        cutToken = token with
        {
            Position = newPosition,
            Length = new TabElementLength(cutLength)
        };
        
        return true;
    }
}