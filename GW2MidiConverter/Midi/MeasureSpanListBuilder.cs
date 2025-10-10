using System.Collections.Generic;

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
            _spans.Add(new MidiMeasureSpan(measureStart, measureEnd, timeSignature));

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
}