using System;
using System.Collections.Generic;
using System.Linq;
using NAudio.Midi;

namespace GW2MidiConverter.Midi;

internal class MidiTimingMap
{
    private readonly MidiFile _midiFile;
    private readonly int _resolution;
    private List<TimeSignatureEvent> _timeSignatureEvents = [];
    private List<TempoEvent> _tempoEvents = [];

    public MidiMeasureInfo[] Measures { get; private set; }

    private MidiTimingMap(MidiFile midiFile)
    {
        _midiFile = midiFile;
        _resolution = midiFile.DeltaTicksPerQuarterNote;
        Measures = [];
    }

    public TimeSignature GetTimeSignatureAtTick(long absoluteTime)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(absoluteTime);
        TimeSignatureEvent? timeSignatureEvent = GetLatestEventAtTick(_timeSignatureEvents, absoluteTime);
        int numerator = timeSignatureEvent?.Numerator ?? 4;
        int denominator = timeSignatureEvent != null ? (int)MathF.Pow(2, timeSignatureEvent.Denominator) : 4;
        return new TimeSignature(numerator, denominator);
    }

    public double GetTempoAtTick(long absoluteTime)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(absoluteTime);
        TempoEvent? tempoEvent = GetLatestEventAtTick(_tempoEvents, absoluteTime);
        return tempoEvent?.Tempo ?? 120;
    }

    public SongPosition GetPositionAtTick(long absoluteTime)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(absoluteTime);
        List<TimeSignatureEvent> leadingTimeSignatureEvents = _timeSignatureEvents.Where(t => t.AbsoluteTime <= absoluteTime).ToList();

        // Count all measures leading up to the tick.
        int measure = 1;
        TimeSignatureEvent lastTimeSignatureEvent = leadingTimeSignatureEvents.First();
        foreach (TimeSignatureEvent timeSignatureEvent in leadingTimeSignatureEvents)
        {
            int lastNumerator = lastTimeSignatureEvent.Numerator;
            int lastDenominator = (int)MathF.Pow(2, lastTimeSignatureEvent.Denominator);

            long timeSinceLastEvent = timeSignatureEvent.AbsoluteTime - lastTimeSignatureEvent.AbsoluteTime;
            long currentTicksPerMeasure = (lastNumerator * _resolution) / (4 / lastDenominator);
            int elapsedMeasures = (int)(timeSinceLastEvent / currentTicksPerMeasure);
            measure += elapsedMeasures;
            lastTimeSignatureEvent = timeSignatureEvent;
        }

        // We have arrived at the current measure! Time to calculate the beat.
        long ticksInCurrentMeasure = absoluteTime - lastTimeSignatureEvent.AbsoluteTime;
        int beat = (int)(ticksInCurrentMeasure / _resolution) + 1;
        long ticksInCurrentBeat = ticksInCurrentMeasure - (beat - 1) * _resolution;
        
        return new SongPosition(measure, beat, ticksInCurrentBeat);
    }

    private void BuildMaps()
    {
        List<MidiEvent> events = GetTimingChangeEvents();
        _timeSignatureEvents = events.OfType<TimeSignatureEvent>().OrderBy(e => e.AbsoluteTime).ToList();
        bool hasDefaultTimeSignature = _timeSignatureEvents.Count > 0 && _timeSignatureEvents.First().AbsoluteTime == 0;
        if (!hasDefaultTimeSignature)
        {
            TimeSignatureEvent defaultTimeSignature = new TimeSignatureEvent(0, 4, 2, 24, 8);
            _timeSignatureEvents.Add(defaultTimeSignature);
        }

        _tempoEvents = events.OfType<TempoEvent>().OrderBy(e => e.AbsoluteTime).ToList();
        bool hasDefaultTempo = _tempoEvents.Count > 0 && _tempoEvents.First().AbsoluteTime == 0;
        if (!hasDefaultTempo)
        {
            TempoEvent defaultTempo = new TempoEvent(500000, 0);
            _tempoEvents.Add(defaultTempo);
        }
    }

    private void CalculateMeasures()
    {
        MeasureSpanListBuilder builder = new MeasureSpanListBuilder(_resolution, _timeSignatureEvents.First().AbsoluteTime);

        TimeSignatureEvent previousTimeSignatureEvent = _timeSignatureEvents.First();
        TimeSignature previousTimeSignature = TimeSignature.FromTimeSignatureEvent(previousTimeSignatureEvent);
        foreach (TimeSignatureEvent timeSignatureEvent in _timeSignatureEvents.Skip(1))
        {
            long timeSinceLastEvent = timeSignatureEvent.AbsoluteTime - previousTimeSignatureEvent.AbsoluteTime;
            builder.AppendMeasuresFromTicks(previousTimeSignature, timeSinceLastEvent);

            previousTimeSignatureEvent = timeSignatureEvent;
            previousTimeSignature = TimeSignature.FromTimeSignatureEvent(timeSignatureEvent);
        }

        long remainingTime = GetLastEventTime() - previousTimeSignatureEvent.AbsoluteTime;
        builder.AppendMeasuresFromTicks(previousTimeSignature, remainingTime);

        Measures = builder.Build();
    }

    private List<MidiEvent> GetTimingChangeEvents()
    {
        List<MidiEvent> timingChangeEvents = [];
        for (int track = 0; track < _midiFile.Tracks; track++)
        {
            IList<MidiEvent>? trackEvents = _midiFile.Events?.GetTrackEvents(track);
            if (trackEvents == null) continue;

            IEnumerable<MidiEvent> timeEvents = trackEvents.Where(e => e is MetaEvent { MetaEventType: MetaEventType.SetTempo or MetaEventType.TimeSignature });
            timingChangeEvents.AddRange(timeEvents);
        }

        return timingChangeEvents;
    }

    private static T? GetLatestEventAtTick<T>(List<T> events, long absoluteTime) where T : MidiEvent
    {
        return events.Where(e => e.AbsoluteTime <= absoluteTime)
                     .MaxBy(e => e.AbsoluteTime);
    }

    private long GetLastEventTime()
    {
        long lastEventTime = 0;
        for (int i = 0; i < _midiFile.Tracks; i++)
        {
            IList<MidiEvent>? events = _midiFile.Events?.GetTrackEvents(i);
            long? lastTick = events?.Max(e => e.AbsoluteTime);
            if (lastTick > lastEventTime) lastEventTime = lastTick.Value;
        }

        return lastEventTime;
    }

    public static MidiTimingMap FromMidiFile(MidiFile midiFile)
    {
        ArgumentNullException.ThrowIfNull(midiFile);

        MidiTimingMap map = new MidiTimingMap(midiFile);
        map.BuildMaps();
        map.CalculateMeasures();

        return map;
    }
}