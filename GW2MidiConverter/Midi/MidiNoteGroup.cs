using System;
using System.Collections.Generic;

namespace GW2MidiConverter.Midi;

public record MidiNoteGroup
{
    public IReadOnlyList<MidiNoteNumber> Notes { get; }
    public SongPosition Position { get; }
    public MidiTimeSpan TimeSpan { get; }

    public bool IsChord => Notes.Count > 1;

    public MidiNoteGroup(IReadOnlyList<MidiNoteNumber> notes, SongPosition position, MidiTimeSpan timeSpan)
    {
        ArgumentNullException.ThrowIfNull(notes);
        if (notes.Count == 0) throw new ArgumentException("Notes must not be empty.", nameof(notes));

        Notes = notes;
        Position = position;
        TimeSpan = timeSpan;
    }
}