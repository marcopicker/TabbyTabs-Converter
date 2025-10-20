using System;
using System.Collections.Generic;
using GW2MidiConverter.General;
using GW2MidiConverter.Tab.Tokens;

namespace GW2MidiConverter.Tab;

public class TabTrack
{
    public TabTrack(TrackInfo trackInfo, IReadOnlyList<TabMeasure> measures)
    {
        ArgumentNullException.ThrowIfNull(trackInfo);
        ArgumentNullException.ThrowIfNull(measures);
        
        TrackInfo = trackInfo;
        Measures = measures;
    }

    public TrackInfo TrackInfo { get; }
    public IReadOnlyList<TabMeasure> Measures { get; }
}