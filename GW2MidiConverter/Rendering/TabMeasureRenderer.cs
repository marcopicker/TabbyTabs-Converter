using System;
using System.Text;
using GW2MidiConverter.Rendering.Settings;
using GW2MidiConverter.Tab.Tokens;

namespace GW2MidiConverter.Rendering;

public class TabMeasureRenderer
{
    private readonly TabRendererSettings _settings;
    private readonly NoteRenderer _noteRenderer;
    private readonly RestRenderer _restRenderer;

    private readonly StringBuilder _measureStringBuilder = new StringBuilder();

    public TabMeasureRenderer(TabRendererSettings settings)
    {
        _settings = settings;
        ArgumentNullException.ThrowIfNull(settings);
        _noteRenderer = new NoteRenderer(settings);
        _restRenderer = new RestRenderer(settings);
    }

    public string RenderMeasure(TabMeasure measure)
    {
        ArgumentNullException.ThrowIfNull(measure);
        if (measure.Tokens.Count == 0) return string.Empty;

        _measureStringBuilder.Clear();
        int beats = measure.TimeSignature.Numerator;
        for (int i = 0; i < beats; i++)
        {
            _measureStringBuilder.Append(RenderBeat(measure, i));
            _measureStringBuilder.Append(new string(' ', _settings.SpacesBetweenBeats));
        }

        return _measureStringBuilder.ToString().Trim();
    }

    public string RenderBeat(TabMeasure measure, int beat)
    {
        ArgumentNullException.ThrowIfNull(measure);
        ArgumentOutOfRangeException.ThrowIfLessThan(beat, 0, nameof(beat));
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(beat, measure.TimeSignature.Numerator, nameof(beat));

        if (measure.Tokens.Count == 0) return string.Empty;

        StringBuilder stringBuilder = new StringBuilder();

        decimal currentPosition = 0;
        foreach (TabToken token in measure.Tokens)
        {
            bool isCorrectBeat = Math.Truncate(currentPosition) == beat;
            if (!isCorrectBeat)
            {
                currentPosition += token.Length.Beats;
                continue;
            }

            switch (token)
            {
                case NoteGroupToken noteGroupToken:
                    stringBuilder.Append(RenderNoteGroup(noteGroupToken));
                    break;
                case RestToken restToken:
                    stringBuilder.Append(_restRenderer.RenderBeats(restToken.Length.Beats));
                    break;
            }

            stringBuilder.Append(_settings.GetTrailingSpacesForBeatLength(token.Length));
            currentPosition += token.Length.Beats;
        }

        return stringBuilder.ToString().Trim();
    }

    private string RenderNoteGroup(NoteGroupToken token)
    {
        StringBuilder stringBuilder = new StringBuilder();
        string notes = _noteRenderer.RenderNotes(token.Notes);
        stringBuilder.Append(notes);

        stringBuilder.Append(_settings.GetTrailingSpacesForBeatLength(token.Length));

        if (token.Length.Beats > 1m)
        {
            string rest = _restRenderer.RenderBeats(token.Length.Beats - 1m);
            stringBuilder.Append(rest);
        }

        return stringBuilder.ToString().Trim();
    }
}