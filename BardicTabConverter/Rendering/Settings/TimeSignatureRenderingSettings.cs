namespace BardicTabConverter.Rendering.Settings;

public class TimeSignatureRenderingSettings
{
    public TimeSignatureVisibility TimeSignatureVisibility { get; init; } = TimeSignatureVisibility.OnlyOnChanges;
    public TimeSignaturePositioning TimeSignaturePositioning { get; init; } = TimeSignaturePositioning.AboveMeasure;
}