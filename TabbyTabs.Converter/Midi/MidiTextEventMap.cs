using System.Collections.Generic;
using System.Linq;
using NAudio.Midi;

namespace TabbyTabs.Converter.Midi;

internal class MidiTextEventMap(MidiFile midiFile, MidiTimingMap timingMap)
{
    public IReadOnlyList<RehearsalMark> GetAllRehearsalMarks()
    {
        List<RehearsalMark> rehearsalMarks = midiFile.Events
                                                      .GetTrackEvents(0)
                                                      .OfType<TextEvent>()
                                                      .Where(e => e.MetaEventType == MetaEventType.Marker)
                                                      .Select(e => new RehearsalMark(timingMap.GetPositionAtTick(e.AbsoluteTime).Measure, e.Text))
                                                      .ToList();
        return rehearsalMarks;
    }

    public string? GetRehearsalMarkForTick(long tick)
    {
        return midiFile.Events
                       .GetTrackEvents(0)
                       .OfType<TextEvent>()
                       .Where(e => e.MetaEventType == MetaEventType.Marker)
                       .Where(e => e.AbsoluteTime == tick)
                       .Select(e => e.Text)
                       .FirstOrDefault();
    }
}