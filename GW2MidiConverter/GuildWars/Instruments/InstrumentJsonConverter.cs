using System;
using System.Collections.Generic;
using GW2MidiConverter.Midi;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GW2MidiConverter.GuildWars.Instruments;

internal class InstrumentNoteMappingJsonConverter : JsonConverter<InstrumentNoteMapping>
{
    private const string MIDI_NOTE_PROPERTY_NAME = "MidiNote";
    private const string KEY_PROPERTY_NAME = "Key";
    private const string OCTAVE_PROPERTY_NAME = "Octave";

    public override void WriteJson(JsonWriter writer, InstrumentNoteMapping? mapping, JsonSerializer serializer)
    {
        if (mapping == null) return;

        writer.WriteStartObject();

        writer.WritePropertyName(MIDI_NOTE_PROPERTY_NAME);
        writer.WriteValue(mapping.MidiNote.Value);

        writer.WritePropertyName(KEY_PROPERTY_NAME);
        writer.WriteValue(mapping.InstrumentNote.Key.ToString());

        writer.WritePropertyName(OCTAVE_PROPERTY_NAME);
        writer.WriteValue(mapping.InstrumentNote.Octave.Value);

        writer.WriteEndObject();
    }

    public override InstrumentNoteMapping ReadJson(JsonReader reader, Type objectType, InstrumentNoteMapping? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject obj = JObject.Load(reader);
        if (!obj.ContainsKey(MIDI_NOTE_PROPERTY_NAME) || obj[MIDI_NOTE_PROPERTY_NAME] == null) throw new JsonSerializationException($"JSON object is missing required '{MIDI_NOTE_PROPERTY_NAME}' property.");
        if (!obj.ContainsKey(KEY_PROPERTY_NAME) || obj[KEY_PROPERTY_NAME] == null) throw new JsonSerializationException($"JSON object is missing required '{KEY_PROPERTY_NAME}' property.");
        if (!obj.ContainsKey(OCTAVE_PROPERTY_NAME) || obj[OCTAVE_PROPERTY_NAME] == null) throw new JsonSerializationException($"JSON object is missing required '{OCTAVE_PROPERTY_NAME}' property.");
        
        int midiNoteValue = obj[MIDI_NOTE_PROPERTY_NAME]!.ToObject<int>();
        MidiNoteNumber midiNoteNumber = new MidiNoteNumber(midiNoteValue);
        
        string? key = obj[KEY_PROPERTY_NAME]!.ToObject<string>();
        if(!Enum.TryParse(key, out GuildWarsNoteKey keyEnum))
        {
            throw new JsonSerializationException($"The value of the '{KEY_PROPERTY_NAME}' property is not a valid {nameof(GuildWarsNoteKey)}.");
        }
        
        int octaveValue = obj[OCTAVE_PROPERTY_NAME]!.ToObject<int>();
        Octave octave = new Octave(octaveValue);

        return new InstrumentNoteMapping(midiNoteNumber, new GuildWarsNote(keyEnum, octave));
    }
}