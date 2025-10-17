using System.Collections.Generic;
using GW2MidiConverter.GuildWars;

namespace GW2MidiConverter.Tab.Octaves;

public interface IOctaveCalculator
{
    Octave[] CalculateOctaves(List<GuildWarsNote[]> potentialNotes);
}