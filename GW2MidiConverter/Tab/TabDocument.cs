using System.Collections.Generic;
using GW2MidiConverter.GuildWars.Instruments;
using GW2MidiConverter.Midi;
using GW2MidiConverter.Tab.Octaves;

namespace GW2MidiConverter.Tab;

public class TabDocument
{
    private readonly MidiDocument _midiDocument;
    private readonly IOctaveCalculator _octaveCalculator;
    private readonly Dictionary<int, TrackConverter> _trackConvertersByTrackIds = new Dictionary<int, TrackConverter>();
    private readonly Dictionary<int, Instrument> _instrumentsByTrackIds = new Dictionary<int, Instrument>();
    private readonly List<TabTrack> _tracks = [];

    public TabTrack[] Tracks => _tracks.ToArray();

    private TabDocument(MidiDocument midiDocument, IOctaveCalculator octaveCalculator)
    {
        _midiDocument = midiDocument;
        _octaveCalculator = octaveCalculator;
        InitialiseTrackConverters();
    }

    private void InitialiseTrackConverters()
    {
        foreach (MidiTrack track in _midiDocument.GetTracks())
        {
            _trackConvertersByTrackIds[track.Info.TrackIndex] = new TrackConverter(track, _midiDocument.TicksPerQuarterNote);
        }
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
            TrackConverter trackConverter = _trackConvertersByTrackIds[trackIndex];
            
            TabTrack track = trackConverter.Convert(instrument, _octaveCalculator);
            _tracks.Add(track);
        }
    }

    public static TabDocument FromMidiDocument(MidiDocument midiDocument, IOctaveCalculator octaveCalculator)
    {
        return new TabDocument(midiDocument, octaveCalculator);
    }
}