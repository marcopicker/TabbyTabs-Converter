using System;
using System.Collections.Generic;
using System.Linq;
using NAudio.Midi;

namespace GW2MidiConverter.Midi;

internal class MidiTimingMap
{
    private readonly MidiFile _midiFile;
    private List<TimeSignatureEvent> _timeSignatureEvents = [];
    private List<TempoEvent> _tempoEvents = [];

    private MidiTimingMap(MidiFile midiFile)
    {
        _midiFile = midiFile;
    }

    public TimeSignature GetTimeSignatureAtTick(long absoluteTime)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(absoluteTime);
        TimeSignatureEvent? timeSignatureEvent = _timeSignatureEvents.Where(e => e.AbsoluteTime <= absoluteTime)
                                                                     .MaxBy(e => e.AbsoluteTime);
        if (timeSignatureEvent == null) return new TimeSignature(4, 4);
        return new TimeSignature(timeSignatureEvent.Numerator, (int)MathF.Pow(2, timeSignatureEvent.Denominator));
    }
    
    private void BuildMaps()
    {
        List<MidiEvent> events = GetTimingChangeEvents();
        _timeSignatureEvents = events.OfType<TimeSignatureEvent>().OrderBy(e => e.AbsoluteTime).ToList();
        _tempoEvents = events.OfType<TempoEvent>().OrderBy(e => e.AbsoluteTime).ToList();
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

    public static MidiTimingMap FromMidiFile(MidiFile midiFile)
    {
        ArgumentNullException.ThrowIfNull(midiFile);
        
        MidiTimingMap map = new MidiTimingMap(midiFile);
        map.BuildMaps();
        
        return map;
    }
}