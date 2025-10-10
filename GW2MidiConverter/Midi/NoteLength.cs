using System;
using System.Collections.Generic;
using System.Linq;

namespace GW2MidiConverter.Midi;

public class NoteLength
{
    public readonly NoteName Name;
    public readonly NoteDot Dot;

    private NoteLength(NoteName name, NoteDot dot)
    {
        Name = name;
        Dot = dot;
    }

    public static NoteLength FromQuarterNotes(decimal quarterNotes)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(quarterNotes);

        decimal closestLength = LengthsByQuarterNotes.Keys.MinBy(length => Math.Abs(quarterNotes - length));
        return LengthsByQuarterNotes[closestLength];
    }

    private static readonly Dictionary<decimal, NoteLength> LengthsByQuarterNotes = new Dictionary<decimal, NoteLength>
    {
        { 0.25m, new NoteLength(NoteName.Sixteenth, NoteDot.NotDotted) }, // Sixteenth
        { 0.375m, new NoteLength(NoteName.Sixteenth, NoteDot.Dotted) }, // Sixteenth.
        
        { 0.50m, new NoteLength(NoteName.Eighth, NoteDot.NotDotted) }, // Eighth
        { 0.75m, new NoteLength(NoteName.Eighth, NoteDot.Dotted) }, // Eighth.
        
        { 1.00m, new NoteLength(NoteName.Quarter, NoteDot.NotDotted) }, // Quarter
        { 1.50m, new NoteLength(NoteName.Quarter, NoteDot.Dotted) }, // Quarter.
        
        { 2.00m, new NoteLength(NoteName.Half, NoteDot.NotDotted) }, // Half
        { 3.00m, new NoteLength(NoteName.Half, NoteDot.Dotted) }, // Half.
        
        { 4.00m, new NoteLength(NoteName.Whole, NoteDot.NotDotted) }, // Whole.
    };
}