using System;
using System.Collections.Generic;
using System.Linq;
using NAudio.Midi;

namespace TabbyTabs.Converter.Midi;

// TODO MP: Make this public.
internal class MidiTimingMap
{
    private readonly MidiFile _midiFile;
    private List<TimeSignatureEvent> _timeSignatureEvents = [];
    private List<TempoEvent> _tempoEvents = [];

    public int TicksPerQuarterNote { get; }

    public MidiMeasureInfo[] Measures { get; private set; }


    private MidiTimingMap(MidiFile midiFile)
    {
        _midiFile = midiFile;
        TicksPerQuarterNote = midiFile.DeltaTicksPerQuarterNote;
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

        int measure = 1;
        decimal beats = 1m;
        for (int i = 0; i < leadingTimeSignatureEvents.Count; i++)
        {
            bool isLastEvent = i == leadingTimeSignatureEvents.Count - 1;

            TimeSignatureEvent currentEvent = leadingTimeSignatureEvents[i];
            TimeSignature timeSignature = TimeSignature.FromTimeSignatureEvent(currentEvent);

            long nextEventTime = isLastEvent ? absoluteTime : leadingTimeSignatureEvents[i + 1].AbsoluteTime;
            long ticksInThisTimeSignature = nextEventTime - currentEvent.AbsoluteTime;
            long ticksPerMeasure = MidiUtilities.CalculateTicksForMeasure(timeSignature, TicksPerQuarterNote);

            int measuresInThisTimeSignature = (int)(ticksInThisTimeSignature / ticksPerMeasure);
            measure += measuresInThisTimeSignature;
            if (!isLastEvent) continue;

            long leftoverTicks = ticksInThisTimeSignature % ticksPerMeasure;
            long ticksPerBeat = MidiUtilities.CalculateTicksPerBeat(timeSignature, TicksPerQuarterNote);
            beats = 1m + (decimal)leftoverTicks / ticksPerBeat;
            break;
        }

        return new SongPosition(measure, beats);
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
        MeasureSpanListBuilder builder = new MeasureSpanListBuilder(TicksPerQuarterNote, _timeSignatureEvents.First().AbsoluteTime);
        
        long lastEventTime = GetLastEventTime();
        for (int i = 0; i < _timeSignatureEvents.Count; i++)
        {
            bool isLastEvent = i == _timeSignatureEvents.Count - 1;

            TimeSignatureEvent currentEvent = _timeSignatureEvents[i];
            TimeSignature timeSignature = TimeSignature.FromTimeSignatureEvent(currentEvent);

            long nextEventTime = isLastEvent ? lastEventTime : _timeSignatureEvents[i + 1].AbsoluteTime;
            long ticksInThisTimeSignature = nextEventTime - currentEvent.AbsoluteTime;
            long ticksPerMeasure = MidiUtilities.CalculateTicksForMeasure(timeSignature, TicksPerQuarterNote);

            int measuresInThisTimeSignature = (int)Math.Ceiling((decimal)ticksInThisTimeSignature / ticksPerMeasure);
            builder.AppendMeasures(timeSignature, measuresInThisTimeSignature);
        }
        
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