using System.Collections.Generic;

namespace GW2MidiConverter.Midi;

internal class MeasureSpanListBuilder(long resolution, long startTime)
{
    private readonly List<MidiMeasureInfo> _spans = [];

    private long _lastTick = startTime;
    private int _currentMeasureNumber;

    public void AppendMeasures(TimeSignature timeSignature, int count)
    {
        long ticksPerMeasure = CalculateTicksPerMeasure(timeSignature);

        for (int i = 0; i < count; i++)
        {
            long measureStart = _lastTick;
            long measureEnd = measureStart + ticksPerMeasure;
            _spans.Add(new MidiMeasureInfo(measureStart, measureEnd, _currentMeasureNumber, timeSignature));

            _lastTick = measureEnd;
            _currentMeasureNumber++;
        }
    }

    public void AppendMeasuresFromTicks(TimeSignature timeSignature, long ticks)
    {
        long ticksPerMeasure = CalculateTicksPerMeasure(timeSignature);
        int elapsedMeasures = (int)(ticks / ticksPerMeasure);
        AppendMeasures(timeSignature, elapsedMeasures);
    }

    public MidiMeasureInfo[] Build() => _spans.ToArray();

    private long CalculateTicksPerMeasure(TimeSignature timeSignature)
    {
        return MidiUtilities.CalculateTicksForMeasure(timeSignature, resolution);
    }
}