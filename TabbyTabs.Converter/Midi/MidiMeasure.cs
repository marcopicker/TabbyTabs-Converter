using System;
using System.Collections.Generic;

namespace TabbyTabs.Converter.Midi;

public record MidiMeasure
{
    public MidiMeasureInfo MeasureInfo { get; init; }
    public IReadOnlyList<MidiNoteGroup> NoteGroups { get; init; }

    public bool IsEmpty => NoteGroups.Count == 0;

    public MidiMeasure(MidiMeasureInfo measureInfo, IReadOnlyList<MidiNoteGroup> noteGroups)
    {
        ArgumentNullException.ThrowIfNull(noteGroups);
        MeasureInfo = measureInfo;
        NoteGroups = noteGroups;
    }
}