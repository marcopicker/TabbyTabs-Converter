using System;

namespace GW2MidiConverter.Midi;

public record class MidiNoteGroup
{
    public MidiNoteNumber[] Notes { get; init; }
    public SongPosition Position { get; init; }
    public long Length { get; init; }
    
    public bool IsChord => Notes.Length > 1;
    
    public MidiNoteGroup(MidiNoteNumber[] notes, SongPosition position, long length)
    {
        ArgumentNullException.ThrowIfNull(notes);
        if (notes.Length == 0) throw new ArgumentException("Notes must not be empty.", nameof(notes));
        
        Notes = notes;
        Position = position;
        Length = length;
    }
}