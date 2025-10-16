using System.Collections.Generic;
using GW2MidiConverter.GuildWars.Instruments;
using GW2MidiConverter.Midi;

namespace GW2MidiConverter.Tab;

public class TabDocument
{
    private readonly MidiDocumentConverter _midiDocumentConverter;
    
    private readonly Dictionary<int, Instrument> _instrumentsByTrackIds = new Dictionary<int, Instrument>();
    private readonly List<TabTrack> _tracks = [];

    public TabTrack[] Tracks => _tracks.ToArray();

    private TabDocument(MidiDocument midiDocument)
    {
        _midiDocumentConverter = new MidiDocumentConverter(midiDocument);
    }

    public void DisableTrack(int trackIndex)
    {
        _instrumentsByTrackIds.Remove(trackIndex);
        RebuildTracks();
    }

    public void EnableTrack(int trackIndex, Instrument instrument)
    {
        // No need to rebuild when nothing changes.
        if(_instrumentsByTrackIds.TryGetValue(trackIndex, out Instrument? value) && value == instrument) return;
        
        _instrumentsByTrackIds[trackIndex] = instrument;
        RebuildTracks();
    }

    private void RebuildTracks()
    {
        _tracks.Clear();
        foreach (int trackIndex in _instrumentsByTrackIds.Keys)
        {
            Instrument instrument = _instrumentsByTrackIds[trackIndex];
            TabTrack track = _midiDocumentConverter.ConvertTrack(trackIndex, instrument.NoteMap, instrument.Name);
            _tracks.Add(track);
        }
    }

    public static TabDocument FromMidiDocument(MidiDocument midiDocument)
    {
        return new TabDocument(midiDocument);
    }
}