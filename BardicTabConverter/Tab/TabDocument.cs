using System;
using System.Collections.Generic;
using BardicTabConverter.GuildWars.Instruments;
using BardicTabConverter.Midi;
using BardicTabConverter.Tab.Octaves;

namespace BardicTabConverter.Tab;

public class TabDocument
{
    private readonly MidiDocument _midiDocument;
    private readonly IOctaveCalculator _octaveCalculator;
    private readonly Dictionary<int, TrackConverter> _trackConvertersByTrackIds = new Dictionary<int, TrackConverter>();
    private readonly Dictionary<int, Instrument> _instrumentsByTrackIds = new Dictionary<int, Instrument>();
    private readonly List<TabTrack> _tracks = [];

    public IReadOnlyList<TabTrack> Tracks => _tracks;

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
            _trackConvertersByTrackIds[track.Info.TrackIndex] = new TrackConverter(track, _midiDocument.TimingMap);
        }
    }

    public void DisableTrack(int trackIndex)
    {
        _instrumentsByTrackIds.Remove(trackIndex);
        RebuildTracks();
    }

    public void DisableAllTracks()
    {
        _instrumentsByTrackIds.Clear();
        RebuildTracks();
    }

    public void EnableTrack(int trackIndex, Instrument instrument)
    {
        // No need to rebuild when nothing changes.
        if (_instrumentsByTrackIds.TryGetValue(trackIndex, out Instrument? value) && value == instrument) return;

        _instrumentsByTrackIds[trackIndex] = instrument;
        RebuildTracks();
    }

    public void EnableTracks(IReadOnlyCollection<TabDocumentTrackConfig> configs)
    {
        ArgumentNullException.ThrowIfNull(configs);
        foreach (TabDocumentTrackConfig config in configs)
        {
            if (config.Instrument == null) continue;
            _instrumentsByTrackIds[config.TrackIndex] = config.Instrument;
        }
        RebuildTracks();
    }

    public bool CanInstrumentPlayTrack(int trackIndex, Instrument instrument)
    {
        ArgumentNullException.ThrowIfNull(instrument);
        return _trackConvertersByTrackIds[trackIndex].CanConvert(instrument);
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