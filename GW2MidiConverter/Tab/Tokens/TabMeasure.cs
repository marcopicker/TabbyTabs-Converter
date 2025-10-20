using System.Collections.Generic;
using System.Linq;
using System.Text;
using GW2MidiConverter.Midi;

namespace GW2MidiConverter.Tab.Tokens;

public class TabMeasure(int measureNumber, TimeSignature timeSignature, IReadOnlyList<TabToken> tokens) : TabToken
{
    public int MeasureNumber { get; } = measureNumber;
    public TimeSignature TimeSignature { get; } = timeSignature;
    public IReadOnlyList<TabToken> Tokens { get; } = tokens;
}