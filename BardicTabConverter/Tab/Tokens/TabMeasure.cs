using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using BardicTabConverter.Midi;

namespace BardicTabConverter.Tab.Tokens;

[DebuggerDisplay("Measure {MeasureNumber}, {TimeSignature}, {Tokens.Count} tokens.")]
public class TabMeasure(int measureNumber, TimeSignature timeSignature, IReadOnlyList<TabToken> tokens)
{
    public int MeasureNumber { get; } = measureNumber;
    public TimeSignature TimeSignature { get; } = timeSignature;
    public IReadOnlyList<TabToken> Tokens { get; } = tokens;

    public override string ToString()
    {
        return $"Measure {MeasureNumber}, {TimeSignature}, {Tokens.Count} tokens.";
    }
}