namespace TabbyTabs.Converter.Rendering.Settings;

public class RehearsalMarkRenderingSettings
{
    public bool DrawRehearsalMarks { get; init; } = true;
    public string Format { get; init; } = "{0}";
    
    public int LinesBeforeRehearsalMark { get; init; } = 1;
    public int LinesAfterReheasalMark { get; init; } = 1;
}