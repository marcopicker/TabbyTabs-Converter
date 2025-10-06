using System;
using System.Collections.Generic;
using System.Linq;
using NAudio.Midi;

namespace GW2MidiConverter.Midi;

public class MidiDocument
{
    private readonly MidiFile _midiFile;
    private readonly List<int> _instrumentTrackIndexes = [];

    private MidiDocument(MidiFile midiFile)
    {
        _midiFile = midiFile;
        FindInstrumentTracks();
    }

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
    public int[] InstrumentTrackIndexes => _instrumentTrackIndexes.ToArray();

    public MidiTrackInfo GetTrackInfo(int trackIndex)
    {
        if (trackIndex < 0 || trackIndex >= _midiFile.Tracks) throw new ArgumentOutOfRangeException(nameof(trackIndex));
        string instrumentName = GetInstrumentNameForTrack(trackIndex);
        return new MidiTrackInfo(trackIndex, instrumentName);
    }

    private string GetInstrumentNameForTrack(int trackIndex)
    {
        if(!_instrumentTrackIndexes.Contains(trackIndex)) return string.Empty;
        
        IList<MidiEvent>? events = _midiFile.Events.GetTrackEvents(trackIndex);
        if (events == null) return string.Empty;
        
        TextEvent? trackNameEvent = events.FirstOrDefault(e => e is TextEvent { MetaEventType: MetaEventType.SequenceTrackName }) as TextEvent;
        if (trackNameEvent == null) return string.Empty;
        
        return trackNameEvent.Text;
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
        MidiFile midiFile = new MidiFile(path, true);
        return new MidiDocument(midiFile);
    }

    #endregion

    internal MidiFile GetRawMidiFile()
    {
        return _midiFile;
    }
}