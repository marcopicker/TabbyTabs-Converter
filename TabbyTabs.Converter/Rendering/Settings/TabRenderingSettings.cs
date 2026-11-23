using System.IO;
using Newtonsoft.Json;
using TabbyTabs.Converter.GuildWars;

namespace TabbyTabs.Converter.Rendering.Settings;

public class TabRenderingSettings
{
    public TrackRenderingSettings TrackRenderingSettings { get; init; } = new TrackRenderingSettings();
    public MeasureRenderingSettings MeasureRenderingSettings { get; init; } = new MeasureRenderingSettings();
    public NoteRenderingSettings NoteRenderingSettings { get; init; } = new NoteRenderingSettings();
    public TimeSignatureRenderingSettings TimeSignatureRenderingSettings { get; init; } = new TimeSignatureRenderingSettings();
    public RehearsalMarkRenderingSettings RehearsalMarkRenderingSettings { get; init; } = new RehearsalMarkRenderingSettings();


    public static TabRenderingSettings FromFile(string path)
    {
        FileInfo file = new FileInfo(path);
        if (!file.Exists) throw new FileNotFoundException("The specified file does not exist.", path);

        JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
        serializerSettings.Converters.Add(new OctaveJsonConverter());

        TabRenderingSettings? style = JsonConvert.DeserializeObject<TabRenderingSettings>(File.ReadAllText(path), serializerSettings);
        if (style == null)
        {
            throw new JsonSerializationException("The specified file could not be deserialized.");
        }

        return style;
    }

    public void SaveToFile(string path)
    {
        JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented
        };
        serializerSettings.Converters.Add(new OctaveJsonConverter());

        string jsonString = JsonConvert.SerializeObject(this, serializerSettings);
        File.WriteAllText(path, jsonString);
    }
}