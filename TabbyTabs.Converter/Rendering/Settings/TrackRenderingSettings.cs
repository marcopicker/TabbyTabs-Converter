using System;

namespace TabbyTabs.Converter.Rendering.Settings;

public class TrackRenderingSettings
{
    private readonly int _measuresPerLine = 1;

    public int LineBreaksAfterTrackName { get; init; } = 1;
    public int LineBreaksBetweenTracks { get; init; } = 4;
    
    // TODO MP: Rename row to line
    public int MaxConsecutiveRowCount { get; init; } = 4;
    
    public int LinesBeforeTimeSignature { get; init; } = 1;
    public int LinesAfterTimeSignature { get; init; }
    
    public OctaveRenderingMode OctaveRenderingMode { get; init; } = OctaveRenderingMode.PerMeasure;
    public int MeasuresPerLine
    {
        get => _measuresPerLine;
        init
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value), $"The value of {nameof(MeasuresPerLine)} may not be less than 1.");
            _measuresPerLine = value;
        }
    }

    public bool ShowTrackName { get; set; } = true;
}