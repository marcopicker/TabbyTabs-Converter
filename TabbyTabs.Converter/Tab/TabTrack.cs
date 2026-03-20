using System;
using System.Collections.Generic;
using TabbyTabs.Converter.General;
using TabbyTabs.Converter.Tab.Tokens;

namespace TabbyTabs.Converter.Tab;

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