using System;

namespace GW2MidiConverter.Rendering.Settings;

public class TrackRenderingSettings
{
    private readonly int _measuresPerLine = 1;
    
    public int LineBreaksAfterTrackName { get; init; } = 2;
    public int LineBreaksBetweenTracks { get; init; } = 2;
    
    public int MaxMeasureCountBeforeLineBreak { get; init; } = 4;
    
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
}