using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GW2MidiConverter.Tab.Tokens;

public class TabMeasure(List<TabToken> tokens) : TabToken
{
    public readonly TabToken[] Tokens = tokens.ToArray();
}