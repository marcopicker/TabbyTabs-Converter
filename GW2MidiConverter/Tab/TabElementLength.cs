using System;
using GW2MidiConverter.Midi;

namespace GW2MidiConverter.Tab;

public readonly struct TabElementLength(decimal beats) : IComparable<TabElementLength>
{
    public decimal Beats { get; } = beats;

    public bool IsDotted => (Beats * 1000) % 1.5m == 0;

    public static implicit operator decimal(TabElementLength length) => length.Beats;
    public static implicit operator TabElementLength(decimal value) => new TabElementLength(value);

    public static TabElementLength FromMidiData(MidiTimeSpan timeSpan, int ticksPerQuarterNote, TimeSignature timeSignature)
    {
        decimal quarterNotes = (decimal) timeSpan.Length / ticksPerQuarterNote;
        decimal quarterNotesPerBeat = (decimal) 4 / timeSignature.Denominator;
        decimal beats = quarterNotes / quarterNotesPerBeat;
        return new TabElementLength(beats);
    }

    public int CompareTo(TabElementLength other)
    {
        return Beats.CompareTo(other.Beats);
    }
}