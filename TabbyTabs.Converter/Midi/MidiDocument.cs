using System;
using System.Collections.Generic;
using System.Linq;
using NAudio.Midi;
using TabbyTabs.Converter.Converters;
using TabbyTabs.Converter.General;

namespace TabbyTabs.Converter.Midi;

/// <summary>
/// Represents a MIDI document that provides information about a given MIDI file.
/// </summary>
public class MidiDocument
{
    private readonly MidiFile _midiFile;
    private readonly List<int> _instrumentTrackIndexes = [];
    private readonly MidiTrack[] _tracks = [];

    private MidiDocument(MidiFile midiFile, MidiConverterSettings? settings = null)
    {
        _midiFile = midiFile;
        TimingMap = MidiTimingMap.FromMidiFile(midiFile);
        FindInstrumentTracks();

        if (_instrumentTrackIndexes.Count == 0) return;
        MidiTrackConverter converter = new MidiTrackConverter(this, settings);
        _tracks = converter.ConvertTracks(_instrumentTrackIndexes).Values.ToArray();
    }

    // TODO MP: Make this public.
    internal MidiTimingMap TimingMap { get; }

    /// <summary>
    /// Returns the count of instrument-tracks in this MIDI document.
    /// </summary>
    /// <remarks>
    /// <i>Instrument-tracks</i> are tracks with at least one MIDI note event.
    /// </remarks>
    public int InstrumentCount => _instrumentTrackIndexes.Count;

    /// <summary>
    /// Returns the track indexes for tracks that contain instruments.
    /// </summary>
    /// <remarks>
    /// <i>Instrument-tracks</i> are tracks with at least one MIDI note event.
    /// </remarks>
    public IReadOnlyCollection<int> InstrumentTrackIndexes => _instrumentTrackIndexes;

    public int TicksPerQuarterNote => _midiFile.DeltaTicksPerQuarterNote;

    /// <summary>
    /// Returns the <see cref="TrackInfo"/> for the given track index.
    /// </summary>
    /// <param name="trackIndex">The zero-based index of the MIDI track.</param>
    /// <returns>A <see cref="TrackInfo"/> object for the given track index.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the index does not relate to a track in the document.</exception>
    public TrackInfo GetTrackInfo(int trackIndex)
    {
        return GetTrack(trackIndex).Info;
    }

    /// <summary>
    /// Returns the <see cref="MidiTrack"/> for the given track index.
    /// </summary>
    /// <param name="trackIndex">The zero-based index of the MIDI track.</param>
    /// <returns>A <see cref="MidiTrack"/> object for the given track index.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the index does not relate to a track in the document.</exception>
    public MidiTrack GetTrack(int trackIndex)
    {
        if (trackIndex < 0 || trackIndex >= _midiFile.Tracks) throw new ArgumentOutOfRangeException(nameof(trackIndex));
        if (!_instrumentTrackIndexes.Contains(trackIndex)) throw new ArgumentOutOfRangeException(nameof(trackIndex), "The given track index does not relate to an instrument track.");
        return _tracks.First(t => t.Info.TrackIndex == trackIndex);
    }

    public MidiTrack[] GetTracks()
    {
        return _tracks;
    }

    private void FindInstrumentTracks()
    {
        for (int i = 0; i < _midiFile.Tracks; i++)
        {
            IList<MidiEvent>? events = _midiFile.Events.GetTrackEvents(i);
            if (events == null) continue;

            bool isInstrumentTrack = events.Any(e => e is NoteEvent);
            if (isInstrumentTrack) _instrumentTrackIndexes.Add(i);
        }
    }

    #region Loading Data

    public static MidiDocument LoadFromMidiFile(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("The path must not be empty.", nameof(path));
        
        try
        {
            MidiFile midiFile = new MidiFile(path, true);
            return new MidiDocument(midiFile);
        }
        catch (FormatException e)
        {
            throw new InvalidMidiDocumentException("The MIDI file is either not a MIDI file or corrupted.", e);
        }
        catch (Exception e)
        {
            throw new MidiDocumentException("An unexpected error occurred while loading the MIDI file.", e);
        }
    }

    #endregion

    internal MidiFile GetRawMidiFile()
    {
        return _midiFile;
    }
}