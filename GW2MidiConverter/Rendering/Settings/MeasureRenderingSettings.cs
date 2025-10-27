namespace GW2MidiConverter.Rendering.Settings;

public class MeasureRenderingSettings
{
    public bool ShowMeasureNumbers { get; init; }
    public bool DrawMeasureDelimiters { get; init; }
    public char MeasureDelimiter { get; init; } = '|';
    public int SpacesBetweenBeats { get; init; } = 4;
}