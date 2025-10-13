using System;

namespace GW2MidiConverter.Midi;

public record MidiMeasure
{
    public MidiMeasureInfo MeasureInfo { get; init; }
    public MidiNoteGroup[] NoteGroups { get; init; }
    
    public bool IsEmpty => NoteGroups.Length == 0;
    
    public MidiMeasure(MidiMeasureInfo measureInfo, MidiNoteGroup[] noteGroups)
    {
        ArgumentNullException.ThrowIfNull(noteGroups);
        MeasureInfo = measureInfo;
        NoteGroups = noteGroups;
    }
}