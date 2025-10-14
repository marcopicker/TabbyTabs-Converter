using System;

namespace GW2MidiConverter.Midi;

public record MidiNoteGroup
{
    public MidiNoteNumber[] Notes { get; init; }
    public SongPosition Position { get; init; }
    public MidiNoteLength Length { get; init; }
    
    public bool IsChord => Notes.Length > 1;

    public MidiNoteGroup(MidiNoteNumber[] notes, SongPosition position, MidiNoteLength length)
    {
        ArgumentNullException.ThrowIfNull(notes);
        if (notes.Length == 0) throw new ArgumentException("Notes must not be empty.", nameof(notes));

        Notes = notes;
        Position = position;
        Length = length;
    }
}