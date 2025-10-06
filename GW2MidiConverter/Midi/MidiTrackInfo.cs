namespace GW2MidiConverter.Midi;

/// <summary>
/// Represents a track in a MIDI file. Used by the <see cref="MidiDocument"/> class.
/// </summary>
/// <seealso cref="MidiDocument"/>
/// <param name="TrackIndex">The zero-based index of the track.</param>
/// <param name="InstrumentName">The track's name or instrument name, if present.</param>
public record MidiTrackInfo(int TrackIndex, string InstrumentName);