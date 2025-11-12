using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using GW2MidiConverter.Midi;
using GW2MidiConverter.Rendering.Settings;
using GW2MidiConverter.Tab;
using GW2MidiConverter.Tab.Tokens;

namespace GW2MidiConverter.Rendering;

internal class TabTrackRenderer(
    TrackRenderingSettings trackRenderingSettings,
    MeasureRenderingSettings measureRenderingSettings,
    NoteRenderingSettings noteRenderingSettings,
    TimeSignatureRenderingSettings timeSignatureRenderingSettings)
{
    private readonly StringBuilder _trackStringBuilder = new StringBuilder();
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
        _trackStringBuilder.Clear();

        RenderTrackHeader(track);

        TimeSignature previousTimeSignature = new TimeSignature(0, 0);
        int consecutiveLines = 0;
        int consecutiveMeasures = 0;
        for (int i = 0; i < track.Measures.Count; i++)
        {
            TabMeasure currentMeasure = track.Measures[i];
            TabMeasure? nextMeasure = i + 1 < track.Measures.Count ? track.Measures[i + 1] : null;

            TimeSignature currentTimeSignature = currentMeasure.TimeSignature;
            bool isTimeSignatureRendered = TryRenderMeasureTimeSignature(currentTimeSignature, previousTimeSignature, out string timeSignatureString);
            previousTimeSignature = currentTimeSignature;

            if (isTimeSignatureRendered)
            {
                bool isFirstMeasure = i == 0;
                if (isFirstMeasure) timeSignatureString = timeSignatureString.TrimStart();
                _trackStringBuilder.Append(timeSignatureString);
                consecutiveLines = 0;
            }

            RenderMeasure(currentMeasure);
            consecutiveMeasures++;

            bool isLastMeasure = nextMeasure == null;
            bool isNextMeasureTimeSignatureChange = nextMeasure != null && currentTimeSignature != nextMeasure.TimeSignature;
            bool isMeasureLimitReached = consecutiveMeasures == trackRenderingSettings.MeasuresPerLine;

            if (isLastMeasure) break;

            if (isNextMeasureTimeSignatureChange)
            {
                _trackStringBuilder.AppendLine();
                consecutiveMeasures = 0;
                consecutiveLines++;
                continue;
            }

            if (isMeasureLimitReached)
            {
                _trackStringBuilder.AppendLine();
                consecutiveLines++;
                consecutiveMeasures = 0;
            }
            else
            {
                _trackStringBuilder.Append(new string(' ', measureRenderingSettings.SpacesBetweenBeats));
            }

            bool isLineLimitReached = consecutiveLines == trackRenderingSettings.MaxConsecutiveRowCount;
            if (isLineLimitReached)
            {
                _trackStringBuilder.AppendLine();
                consecutiveMeasures = 0;
                consecutiveLines = 0;
            }
        }

        return _trackStringBuilder.ToString().Trim().Trim('\n');
    }

    private void RenderMeasure(TabMeasure measure)
    {
        string renderedMeasure = _measureRenderer.RenderMeasure(measure);
        string padding = new string(' ', measureRenderingSettings.MeasureDelimiterPadding);

        _trackStringBuilder.Append(RenderMeasureNumber(measure.MeasureNumber));
        _trackStringBuilder.Append(RenderMeasureDelimiter());
        if (measureRenderingSettings.DrawMeasureDelimiters) _trackStringBuilder.Append(padding);
        _trackStringBuilder.Append(renderedMeasure);
        if (measureRenderingSettings.DrawMeasureDelimiters) _trackStringBuilder.Append(padding);
        _trackStringBuilder.Append(RenderMeasureDelimiter());
    }

    private void RenderTrackHeader(TabTrack track)
    {
        if (!trackRenderingSettings.ShowTrackName) return;

        _trackStringBuilder.AppendLine(CultureInfo.InvariantCulture, $"{track.TrackInfo.InstrumentName}:");
        _trackStringBuilder.Append(new string('\n', trackRenderingSettings.LineBreaksAfterTrackName));
    }

    private string RenderMeasureNumber(int measureMeasureNumber)
    {
        if (!measureRenderingSettings.ShowMeasureNumbers) return string.Empty;
        string measureNumberString = measureMeasureNumber.ToString("D2", CultureInfo.InvariantCulture);
        return $"{measureNumberString}: ";
    }

    private string RenderMeasureDelimiter()
    {
        if (!measureRenderingSettings.DrawMeasureDelimiters) return string.Empty;
        return measureRenderingSettings.MeasureDelimiter.ToString();
    }

    private bool TryRenderMeasureTimeSignature(TimeSignature timeSignature, TimeSignature previousTimeSignature, out string output)
    {
        bool showTimeSignature = timeSignatureRenderingSettings.TimeSignatureVisibility == TimeSignatureVisibility.AlwaysShow ||
                                 (timeSignatureRenderingSettings.TimeSignatureVisibility == TimeSignatureVisibility.OnlyOnChanges && timeSignature != previousTimeSignature);
        if (!showTimeSignature)
        {
            output = string.Empty;
            return false;
        }

        StringBuilder stringBuilder = new StringBuilder();

        string timeSignatureString = $"{timeSignature.Numerator}/{timeSignature.Denominator}: ";
        switch (timeSignatureRenderingSettings.TimeSignaturePositioning)
        {
            default:
            case TimeSignaturePositioning.AboveMeasure:
                stringBuilder.Append(new string('\n', trackRenderingSettings.LinesBeforeTimeSignature));
                stringBuilder.AppendLine(timeSignatureString);
                stringBuilder.Append(new string('\n', trackRenderingSettings.LinesAfterTimeSignature));
                break;
            case TimeSignaturePositioning.InFrontOfMeasure:
                stringBuilder.Append(new string('\n', trackRenderingSettings.LinesBeforeTimeSignature));
                stringBuilder.Append(timeSignatureString);
                break;
        }

        output = stringBuilder.ToString();
        return true;
    }
}