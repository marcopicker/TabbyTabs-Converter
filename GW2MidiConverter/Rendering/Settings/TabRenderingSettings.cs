namespace GW2MidiConverter.Rendering.Settings;

public class TabRenderingSettings
{
    public TrackRenderingSettings TrackRenderingSettings { get; init; } = new TrackRenderingSettings();
    public MeasureRenderingSettings MeasureRenderingSettings { get; init; } = new MeasureRenderingSettings();
    public NoteRenderingSettings NoteRenderingSettings { get; init; } = new NoteRenderingSettings();
    public TimeSignatureRenderingSettings TimeSignatureRenderingSettings { get; init; } = new TimeSignatureRenderingSettings();
}