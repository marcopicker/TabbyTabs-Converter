using System;
using System.Collections.Generic;
using System.Linq;
using NAudio.Midi;

namespace GW2MidiConverter.Midi;

internal class MidiTimingMap
{
    private readonly MidiFile _midiFile;
    private int _resolution;
    private List<TimeSignatureEvent> _timeSignatureEvents = [];
    private List<TempoEvent> _tempoEvents = [];

    private MidiTimingMap(MidiFile midiFile)
    {
        _midiFile = midiFile;
        _resolution = midiFile.DeltaTicksPerQuarterNote;
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

    public SongTime GetTimeAtTick(long absoluteTime)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(absoluteTime);
        List<TimeSignatureEvent> leadingTimeSignatureEvents = _timeSignatureEvents.Where(t => t.AbsoluteTime <= absoluteTime).ToList();

        // Count all measures leading up to the tick.
        int measure = 1;
        TimeSignatureEvent lastTimeSignatureEvent = leadingTimeSignatureEvents.First();
        foreach (TimeSignatureEvent timeSignatureEvent in leadingTimeSignatureEvents)
        {
            int lastNumerator = lastTimeSignatureEvent.Numerator;
            int lastDenominator = (int) MathF.Pow(2, lastTimeSignatureEvent.Denominator);
            
            long timeSinceLastEvent = timeSignatureEvent.AbsoluteTime - lastTimeSignatureEvent.AbsoluteTime;
            long currentTicksPerMeasure = (lastNumerator * _resolution) / (4 / lastDenominator);
            int elapsedMeasures = (int)(timeSinceLastEvent / currentTicksPerMeasure);
            measure += elapsedMeasures;
            lastTimeSignatureEvent = timeSignatureEvent;
        }
        
        // We have arrived at the current measure! Time to calculate the beat.
        long ticksInCurrentMeasure = absoluteTime - lastTimeSignatureEvent.AbsoluteTime;
        int beat = (int)(ticksInCurrentMeasure / _resolution) + 1;
        
        return new SongTime(measure, beat);
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

    public static MidiTimingMap FromMidiFile(MidiFile midiFile)
    {
        ArgumentNullException.ThrowIfNull(midiFile);

        MidiTimingMap map = new MidiTimingMap(midiFile);
        map.BuildMaps();

        return map;
    }
}