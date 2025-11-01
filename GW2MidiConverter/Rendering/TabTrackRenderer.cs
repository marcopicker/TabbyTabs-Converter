using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using GW2MidiConverter.Midi;
using GW2MidiConverter.Rendering.Settings;
using GW2MidiConverter.Tab;
using GW2MidiConverter.Tab.Tokens;

namespace GW2MidiConverter.Rendering;

internal class TabTrackRenderer(TrackRenderingSettings trackRenderingSettings,
                                MeasureRenderingSettings measureRenderingSettings,
                                NoteRenderingSettings noteRenderingSettings,
                                TimeSignatureRenderingSettings timeSignatureRenderingSettings)
{
    private readonly StringBuilder _stringBuilder = new StringBuilder();
    private readonly TabMeasureRenderer _measureRenderer = new TabMeasureRenderer(measureRenderingSettings, noteRenderingSettings);

    /// <summary>
    /// Renders the given track. This method only renders the actual content, and no headers like the instrument name.
    /// </summary>
    /// <param name="track">The track to render.</param>
    /// <returns>The rendered track.</returns>
    [SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "The instrument name is invariant.")]
    public string Render(TabTrack track)
    {
        if (track.Measures.Count == 0) return string.Empty;
        _stringBuilder.Clear();

        RenderTrackHeader(track);
        
        TimeSignature previousTimeSignature = new TimeSignature(0, 0);
        int measureInRowsCounter = 0;
        for (int i = 0; i < track.Measures.Count; i++)
        {
            TabMeasure measure = track.Measures[i];

            bool isLineBreakDue = i > 0 &&
                                  i < track.Measures.Count &&
                                  measureInRowsCounter % trackRenderingSettings.MaxMeasureCountBeforeLineBreak == 0;
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

    private void RenderTrackHeader(TabTrack track)
    {
        if (!trackRenderingSettings.ShowTrackName) return;
        _stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"{track.TrackInfo.InstrumentName}:");
        _stringBuilder.Append(new string('\n', trackRenderingSettings.LineBreaksAfterTrackName));
    }

    private void RenderMeasureNumber(int measureMeasureNumber)
    {
        if (!measureRenderingSettings.ShowMeasureNumbers) return;
        _stringBuilder.Append(CultureInfo.InvariantCulture, $"{(measureMeasureNumber + 1).ToString("D2", CultureInfo.InvariantCulture)}: ");
    }

    private void RenderMeasureDelimiter()
    {
        if (!measureRenderingSettings.DrawMeasureDelimiters) return;
        _stringBuilder.Append(measureRenderingSettings.MeasureDelimiter);
    }

    private bool RenderMeasureTimeSignature(TimeSignature timeSignature, TimeSignature previousTimeSignature)
    {
        bool showTimeSignature = timeSignatureRenderingSettings.TimeSignatureVisibility == TimeSignatureVisibility.AlwaysShow ||
                                 (timeSignatureRenderingSettings.TimeSignatureVisibility == TimeSignatureVisibility.OnlyOnChanges && timeSignature != previousTimeSignature);
        if (!showTimeSignature)
        {
            return false;
        }

        string timeSignatureString = $"{timeSignature.Numerator}/{timeSignature.Denominator}: ";
        switch (timeSignatureRenderingSettings.TimeSignaturePositioning)
        {
            default:
            case TimeSignaturePositioning.AboveMeasure:
                _stringBuilder.Append(new string('\n', trackRenderingSettings.LinesBeforeTimeSignature));
                _stringBuilder.AppendLine(timeSignatureString);
                _stringBuilder.Append(new string('\n', trackRenderingSettings.LinesAfterTimeSignature));
                break;
            case TimeSignaturePositioning.InFrontOfMeasure:
                _stringBuilder.Append(new string('\n', trackRenderingSettings.LinesBeforeTimeSignature));
                _stringBuilder.Append(timeSignatureString);
                break;
        }

        return true;
    }
}