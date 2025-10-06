using System.IO;

namespace GW2MidiConverter.Tests.Resources;

public static class MidiFiles
{
    private const string RESOURCE_FOLDER = "Resources";
    
    public static string NotAMidiFile => Path.Combine(RESOURCE_FOLDER, "not_a_midi_file.txt");

    /// <summary>
    /// The path to the "simple_two_track_piece_piano_bass" MIDI file.
    /// The MIDI file contains three tracks, one meta track and two note event tracks: "Piano" at index 1 and "Bass" at index 2.
    /// </summary>
    public static string SimpleTwoTrackPiecePianoBass => Path.Combine(RESOURCE_FOLDER, "simple_two_track_piece_piano_bass.mid");
}