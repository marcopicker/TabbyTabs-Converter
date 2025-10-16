using System.Collections.Generic;
using System.Linq;
using GW2MidiConverter.GuildWars.Instruments;
using GW2MidiConverter.Midi;

namespace GW2MidiConverter.Tab;

public class TabDocument
{
    private List<TabTrack> _tracks = [];

    public TabTrack[] Tracks => _tracks.ToArray();

    private TabDocument()
    {
    }

    public static TabDocument FromMidi(MidiDocument midi)
    {
        MidiDocumentConverter converter = new MidiDocumentConverter(midi);
        int[] indexes = midi.InstrumentTrackIndexes;
        List<TabTrack> tracks = indexes.Select(i => converter.ConvertTrack(i, new InstrumentNoteMap(InstrumentNoteMaps.Piano)))
                                       .ToList();
        
        TabDocument tabDocument = new TabDocument
        {
            _tracks = tracks
        };
        return tabDocument;
    }
}