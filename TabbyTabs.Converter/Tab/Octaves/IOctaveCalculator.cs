using System.Collections.Generic;
using TabbyTabs.Converter.GuildWars;

namespace TabbyTabs.Converter.Tab.Octaves;

public interface IOctaveCalculator
{
    IReadOnlyList<Octave> CalculateOctaves(IReadOnlyList<Octave[]> potentialOctaves);
}