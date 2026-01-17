using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using BardicTabConverter.Midi;
using Newtonsoft.Json;

namespace BardicTabConverter.GuildWars.Instruments;

[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public record Instrument
{
    public const string INSTRUMENT_FILE_EXTENSION = "gw2instrument";

    public Instrument(string name, IReadOnlyCollection<InstrumentNoteMapping> playableNotes)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("The name must not be empty.", nameof(name));
        ArgumentNullException.ThrowIfNull(playableNotes);

        Name = name;
        PlayableNotes = playableNotes;
    }

    public string Name { get; }

    [JsonProperty("Notes")] public IReadOnlyCollection<InstrumentNoteMapping> PlayableNotes { get; }

    public bool CanPlayNote(MidiNoteNumber note)
    {
        return PlayableNotes.Any(m => m.MidiNote == note);
    }

    public GuildWarsNote[] MapMidiToNotes(MidiNoteNumber midiNote)
    {
        if (!CanPlayNote(midiNote)) throw new ArgumentOutOfRangeException(nameof(midiNote), "The given MidiNoteNumber is not mapped to anything.");
        GuildWarsNote[] notes = PlayableNotes.Where(m => m.MidiNote == midiNote)
                                             .Select(m => m.InstrumentNote)
                                             .ToArray();
        return notes;
    }

    public override string ToString() => Name;

    /// <summary>
    /// Loads an instrument from a given .gw2instrument file.
    /// </summary>
    /// <param name="path">The path</param>
    /// <returns></returns>
    public static Instrument FromFile(string path)
    {
        FileInfo fileInfo = new FileInfo(path);
        if (!fileInfo.Exists) throw new FileNotFoundException("The given file does not exist.", path);
        if (fileInfo.Extension != $".{INSTRUMENT_FILE_EXTENSION}") throw new InstrumentException($"The path \"{path}\" does not point to a .gw2instrument file.");

        try
        {
            string json = File.ReadAllText(path);
            Instrument? instrument = JsonConvert.DeserializeObject<Instrument>(json, new InstrumentNoteMappingJsonConverter());
            if (instrument == null) throw new InstrumentException($"The given file is not a valid .{INSTRUMENT_FILE_EXTENSION} file.");
            return instrument;
        }
        catch (Exception e)
        {
            if (e is InstrumentException) throw;
            throw new InstrumentException($"An error occurred while loading the instrument from \"{path}\".", e);
        }
    }
}