using System;

namespace GW2MidiConverter.Midi;

public record SongPosition(int Measure, int Beat, long Ticks) : IComparable<SongPosition>
{
    public int CompareTo(SongPosition? other)
    {
        if(other == null) return 1;
        if (Measure != other.Measure) return Measure.CompareTo(other.Measure);
        if (Beat != other.Beat) return Beat.CompareTo(other.Beat); 
        return Ticks.CompareTo(other.Ticks);
    }

    public virtual bool Equals(SongPosition? other)
    {
        if(other == null) return false;
        return Measure == other.Measure && Beat == other.Beat && Ticks == other.Ticks;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Measure, Beat, Ticks);
    }
}