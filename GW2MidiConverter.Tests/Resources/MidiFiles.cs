using System.IO;

namespace GW2MidiConverter.Tests.Resources;

public static class MidiFiles
{
    private const string RESOURCE_FOLDER = "Resources";
    
    public static string NotAMidiFile => Path.Combine(RESOURCE_FOLDER, "not_a_midi_file.txt");

    /// <summary>
    /// The path to a MIDI file without any instrument tracks.
    /// </summary>
    public static string EmptyMidiFile => Path.Combine(RESOURCE_FOLDER, "empty.mid");
    
    /// <summary>
    /// The path to the "simple_two_track_piece_piano_bass" MIDI file.
    /// The MIDI file contains three tracks, one meta track and two note event tracks: "Piano" at index 1 and "Bass" at index 2.
    /// </summary>
    public static string SimpleTwoTrackPiecePianoBass => Path.Combine(RESOURCE_FOLDER, "simple_two_track_piece_piano_bass.mid");
    
    /// <summary>
    /// The path to a MIDI file that contains various time signature and tempo changes. The piece is four bars long. The note C4 (60) is repeated in quarter notes.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Bar</term>
    ///         <description>Timing</description>
    ///     </listheader>
    ///     <item>
    ///         <term>1</term>
    ///         <description>Initial values of 4/4 at 120 BPM</description>
    ///     </item>
    ///     <item>
    ///         <term>2</term>
    ///         <description>Time signature changes to 3/4.</description>
    ///     </item>
    ///     <item>
    ///         <term>3</term>
    ///         <description>Time signature changes to 4/4</description>
    ///     </item>
    ///     <item>
    ///         <term>4</term>
    ///         <description>BPM changes to 90.</description>
    ///     </item>
    /// </list>
    /// </summary>
    public static string TimingTestFile => Path.Combine(RESOURCE_FOLDER, "time_tests.mid");
    
    /// <summary>
    /// The path to a MIDI file that contains various time signature and tempo changes alongside different note lengths, including dotted ones.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Bar</term>
    ///         <description>Description</description>
    ///     </listheader>
    ///     <item>
    ///         <term>1</term>
    ///         <description>4/4, four quarter notes: 'C', 'D', 'E', 'G'.</description>
    ///     </item>
    ///     <item>
    ///         <term>2</term>
    ///         <description>3/4, dotted quarter note 'C', eighth note 'D' at the end of the bar.</description>
    ///     </item>
    ///     <item>
    ///         <term>3</term>
    ///         <description>3/4, half note 'C' at the start of bar.</description>
    ///     </item>
    ///     <item>
    ///         <term>4</term>
    ///         <description>4/4, one whole note 'C'.</description>
    ///     </item>
    /// </list>
    /// </summary>
    public static string LengthTestFile => Path.Combine(RESOURCE_FOLDER, "note_lengths.mid");
    
    /// <summary>
    /// The path to a MIDI file that contains various time signature and tempo changes alongside different note lengths, including dotted ones.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Bar</term>
    ///         <description>Description</description>
    ///     </listheader>
    ///     <item>
    ///         <term>1</term>
    ///         <description>4/4, four quarter notes: 'C', 'D', 'E', 'G'.</description>
    ///     </item>
    ///     <item>
    ///         <term>2</term>
    ///         <description>3/4, dotted quarter note 'C', eighth note 'D' at the end of the bar.</description>
    ///     </item>
    ///     <item>
    ///         <term>3</term>
    ///         <description>3/4, half note 'C' at the start of bar.</description>
    ///     </item>
    ///     <item>
    ///         <term>4</term>
    ///         <description>4/4, one C major chord at half-length and an A minor chord at quarter-length.</description>
    ///     </item>
    /// </list>
    /// </summary>
    public static string ChordTestFile => Path.Combine(RESOURCE_FOLDER, "chord_tests.mid");
    
    /// <summary>
    /// The path to a MIDI file that contains various odd time signatures.<br/>
    /// Bars:
    /// <list type="number">
    /// <item><description>9/8, 6 notes.</description></item>
    /// <item><description>4/8, 2 notes.</description></item>
    /// <item><description>6/8, 4 notes.</description></item>
    /// <item><description>5/8, 3 notes.</description></item>
    /// </list>
    /// </summary>
    public static string OddTimeSignatures => Path.Combine(RESOURCE_FOLDER, "odd_time_signatures.mid");
}