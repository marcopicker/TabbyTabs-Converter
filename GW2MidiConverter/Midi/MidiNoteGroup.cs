using System;

namespace GW2MidiConverter.Midi;

public record MidiNoteGroup
{
    public MidiNoteNumber[] Notes { get; init; }
    public SongPosition Position { get; init; }
    public MidiTimeSpan TimeSpan { get; init; }
    
    public bool IsChord => Notes.Length > 1;

    public MidiNoteGroup(MidiNoteNumber[] notes, SongPosition position, MidiTimeSpan timeSpan)
    {
        ArgumentNullException.ThrowIfNull(notes);
        if (notes.Length == 0) throw new ArgumentException("Notes must not be empty.", nameof(notes));

        Notes = notes;
        Position = position;
        TimeSpan = timeSpan;
    }
}