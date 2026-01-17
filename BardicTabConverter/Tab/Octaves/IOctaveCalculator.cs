using System.Collections.Generic;
using BardicTabConverter.GuildWars;

namespace BardicTabConverter.Tab.Octaves;

public interface IOctaveCalculator
{
    IReadOnlyList<Octave> CalculateOctaves(IReadOnlyList<Octave[]> potentialOctaves);
}