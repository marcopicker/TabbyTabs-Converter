using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GW2MidiConverter.GuildWars;

internal class OctaveJsonConverter : JsonConverter<Octave>
{
    private const string OCTAVE_PROPERTY_NAME = "Octave";

    public override void WriteJson(JsonWriter writer, Octave octave, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName(OCTAVE_PROPERTY_NAME);
        writer.WriteValue(octave.Value);
        writer.WriteEndObject();
    }

    public override Octave ReadJson(JsonReader reader, Type objectType, Octave existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject obj = JObject.Load(reader);
        if (!obj.ContainsKey(OCTAVE_PROPERTY_NAME) || obj[OCTAVE_PROPERTY_NAME] == null) throw new JsonSerializationException($"JSON object is missing required '{OCTAVE_PROPERTY_NAME}' property.");

        int octaveValue = obj[OCTAVE_PROPERTY_NAME]!.ToObject<int>();
        return new Octave(octaveValue);
    }
}