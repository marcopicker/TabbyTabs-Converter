using System;

namespace BardicTabConverter.Midi;

public readonly record struct MidiTimeSpan
{
    public MidiTimeSpan(long startTick, long endTick)
    {
        if (endTick < startTick) throw new ArgumentException("The end tick must be greater than the start tick.", nameof(endTick));
        StartTick = startTick;
        EndTick = endTick;
    }

    public long TickLength => EndTick - StartTick;
    public long StartTick { get; init; }
    public long EndTick { get; init; }
};