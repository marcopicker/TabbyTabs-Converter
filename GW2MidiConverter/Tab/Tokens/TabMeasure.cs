using System.Collections.Generic;
using System.Linq;
using System.Text;
using GW2MidiConverter.Midi;

namespace GW2MidiConverter.Tab.Tokens;

public class TabMeasure(int measureNumber, TimeSignature timeSignature, List<TabToken> tokens) : TabToken
{
    public readonly int MeasureNumber = measureNumber;
    public readonly TimeSignature TimeSignature = timeSignature;
    public readonly TabToken[] Tokens = tokens.ToArray();
}