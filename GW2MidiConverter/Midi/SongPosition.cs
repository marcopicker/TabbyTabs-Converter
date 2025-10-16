using System;

namespace GW2MidiConverter.Midi;

public record SongPosition(int Measure, int Beat, long TicksInBeat) : IComparable<SongPosition>
{
    public int CompareTo(SongPosition? other)
    {
        if(other == null) return 1;
        if (Measure != other.Measure) return Measure.CompareTo(other.Measure);
        if (Beat != other.Beat) return Beat.CompareTo(other.Beat); 
        return TicksInBeat.CompareTo(other.TicksInBeat);
    }

    public virtual bool Equals(SongPosition? other)
    {
        if(other == null) return false;
        return Measure == other.Measure && Beat == other.Beat && TicksInBeat == other.TicksInBeat;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Measure, Beat, TicksInBeat);
    }
}