using System.Collections.Generic;
using GW2MidiConverter.Shared;
using GW2MidiConverter.Tab.Tokens;

namespace GW2MidiConverter.Tab;

public class TabTrack
{
    public TabTrack(TrackInfo trackInfo, List<TabMeasure> measures)
    {
        TrackInfo = trackInfo;
        Measures = measures.ToArray();
    }

    public TrackInfo TrackInfo { get; private set; }
    public TabMeasure[] Measures { get; private set; }
}