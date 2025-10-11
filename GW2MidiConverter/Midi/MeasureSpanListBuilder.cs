using System.Collections.Generic;
using System.Linq;

namespace GW2MidiConverter.Midi;

internal class MeasureSpanListBuilder(long resolution, long startTime)
{
    private readonly List<MidiMeasureSpan> _spans = [];

    private long _lastTick = startTime;

    public void AppendMeasures(TimeSignature timeSignature, int count)
    {
        long ticksPerMeasure = CalculateTicksPerMeasure(timeSignature);

        for (int i = 0; i < count; i++)
        {
            long measureStart = _lastTick;
            long measureEnd = measureStart + ticksPerMeasure;
            bool isNewTimeSignature = IsTimeSignatureNew(timeSignature);
            _spans.Add(new MidiMeasureSpan(measureStart, measureEnd, timeSignature, isNewTimeSignature));

            _lastTick = measureEnd;
        }
    }

    public void AppendMeasuresFromTicks(TimeSignature timeSignature, long ticks)
    {
        long ticksPerMeasure = CalculateTicksPerMeasure(timeSignature);
        int elapsedMeasures = (int)(ticks / ticksPerMeasure);
        AppendMeasures(timeSignature, elapsedMeasures);
    }

    public MidiMeasureSpan[] Build() => _spans.ToArray();

    private long CalculateTicksPerMeasure(TimeSignature timeSignature)
    {
        return (timeSignature.Numerator * resolution) / (4 / timeSignature.Denominator);
    }

    private bool IsTimeSignatureNew(TimeSignature timeSignature)
    {
        if (_spans.Count == 0) return true;
        return !Equals(_spans.Last().TimeSignature, timeSignature);
    }
}