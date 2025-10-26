using System.Globalization;
using System.Text;
using GW2MidiConverter.Midi;
using GW2MidiConverter.Rendering.Settings;
using GW2MidiConverter.Tab;
using GW2MidiConverter.Tab.Tokens;

namespace GW2MidiConverter.Rendering;

internal class TabTrackRenderer(TabRendererSettings settings)
{
    private readonly StringBuilder _stringBuilder = new StringBuilder();
    private readonly TabMeasureRenderer _measureRenderer = new TabMeasureRenderer(settings);

    /// <summary>
    /// Renders the given track. This method only renders the actual content, and no headers like the instrument name.
    /// </summary>
    /// <param name="track">The track to render.</param>
    /// <returns>The rendered track.</returns>
    public string Render(TabTrack track)
    {
        if (track.Measures.Count == 0) return string.Empty;

        _stringBuilder.Clear();
        TimeSignature previousTimeSignature = new TimeSignature(0, 0);
        int measureInRowsCounter = 0;
        for (int i = 0; i < track.Measures.Count; i++)
        {
            TabMeasure measure = track.Measures[i];

            bool isLineBreakDue = i > 0 &&
                                  i < track.Measures.Count &&
                                  measureInRowsCounter % settings.MaxMeasureCountBeforeSpacing == 0;
            if (isLineBreakDue)
            {
                _stringBuilder.AppendLine();
                measureInRowsCounter = 0;
            }

            if (RenderMeasureTimeSignature(measure.TimeSignature, previousTimeSignature))
            {
                measureInRowsCounter = 0;
            }

            RenderMeasureDelimiter();
            RenderMeasureNumber(measure.MeasureNumber);
            _stringBuilder.Append(_measureRenderer.RenderMeasure(measure));
            RenderMeasureDelimiter();

            _stringBuilder.AppendLine();
            previousTimeSignature = measure.TimeSignature;
            measureInRowsCounter++;
        }

        return _stringBuilder.ToString().Trim().TrimStart('\n');
    }

    private void RenderMeasureNumber(int measureMeasureNumber)
    {
        if (!settings.ShowMeasureNumbers) return;
        _stringBuilder.Append(CultureInfo.InvariantCulture, $"{(measureMeasureNumber + 1).ToString("D2", CultureInfo.InvariantCulture)}: ");
    }

    private void RenderMeasureDelimiter()
    {
        if (!settings.DrawMeasureDelimiters) return;
        _stringBuilder.Append(settings.MeasureDelimiter);
    }

    private bool RenderMeasureTimeSignature(TimeSignature timeSignature, TimeSignature previousTimeSignature)
    {
        bool showTimeSignature = settings.TimeSignatureVisibility == TimeSignatureVisibility.AlwaysShow ||
                                 (settings.TimeSignatureVisibility == TimeSignatureVisibility.OnlyOnChanges && timeSignature != previousTimeSignature);
        if (!showTimeSignature)
        {
            return false;
        }

        string timeSignatureString = $"{timeSignature.Numerator}/{timeSignature.Denominator}: ";
        switch (settings.TimeSignaturePositioning)
        {
            default:
            case TimeSignaturePositioning.AboveMeasure:
                _stringBuilder.Append(new string('\n', settings.LinesBeforeTimeSignature));
                _stringBuilder.AppendLine(timeSignatureString);
                _stringBuilder.Append(new string('\n', settings.LinesAfterTimeSignature));
                break;
            case TimeSignaturePositioning.InFrontOfMeasure:
                _stringBuilder.Append(new string('\n', settings.LinesBeforeTimeSignature));
                _stringBuilder.Append(timeSignatureString);
                break;
        }

        return true;
    }
}