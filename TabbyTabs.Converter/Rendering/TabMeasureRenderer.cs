using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TabbyTabs.Converter.Rendering.Settings;
using TabbyTabs.Converter.Tab;
using TabbyTabs.Converter.Tab.Tokens;

namespace TabbyTabs.Converter.Rendering;

internal class TabMeasureRenderer
{
    private readonly MeasureRenderingSettings _measureRenderingSettings;
    private readonly NoteRenderingSettings _noteRenderingSettings;
    private readonly NoteRenderer _noteRenderer;
    private readonly RestRenderer _restRenderer;
    private readonly BracketSimplifier _bracketSimplifier;

    private readonly StringBuilder _measureStringBuilder = new StringBuilder();

    public TabMeasureRenderer(MeasureRenderingSettings measureRenderingSettings, NoteRenderingSettings noteRenderingSettings)
    {
        _measureRenderingSettings = measureRenderingSettings;
        _noteRenderingSettings = noteRenderingSettings;
        ArgumentNullException.ThrowIfNull(noteRenderingSettings);
        _noteRenderer = new NoteRenderer(noteRenderingSettings);
        _restRenderer = new RestRenderer(noteRenderingSettings);
        _bracketSimplifier = new BracketSimplifier(noteRenderingSettings.BracketPairsPerOctave.Values.ToList());
    }

    public string RenderMeasure(TabMeasure measure)
    {
        ArgumentNullException.ThrowIfNull(measure);
        if (measure.Tokens.Count == 0) return string.Empty;

        _measureStringBuilder.Clear();
        int beats = measure.TimeSignature.Numerator;
        for (int i = 0; i < beats; i++)
        {
            _measureStringBuilder.Append(RenderBeat(measure, i + 1));
            _measureStringBuilder.Append(new string(' ', _measureRenderingSettings.SpacesBetweenBeats));
        }

        return _measureStringBuilder.ToString().Trim();
    }

    public IReadOnlyList<string> RenderBeats(TabMeasure measure)
    {
        ArgumentNullException.ThrowIfNull(measure);
        if (measure.Tokens.Count == 0) return [];

        int beats = measure.TimeSignature.Numerator;
        List<string> renderedBeats = new List<string>(beats);
        for (int i = 0; i < beats; i++)
        {
            renderedBeats.Add(RenderBeat(measure, i + 1));
        }

        return renderedBeats;
    }

    public string RenderBeat(TabMeasure measure, int beat)
    {
        ArgumentNullException.ThrowIfNull(measure);
        ArgumentOutOfRangeException.ThrowIfLessThan(beat, 1, nameof(beat));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(beat, measure.TimeSignature.Numerator, nameof(beat));

        if (measure.Tokens.Count == 0) return string.Empty;

        StringBuilder stringBuilder = new StringBuilder();

        IReadOnlyList<TabToken> preparedTokens = PrepareTokensForBeat(measure, beat);
        foreach (TabToken token in preparedTokens)
        {
            stringBuilder.Append(RenderToken(token));
            stringBuilder.Append(_noteRenderingSettings.GetTrailingSpacesForBeatLength(token.Length));
        }

        string renderedBeat = stringBuilder.ToString().Trim();

        try
        {
            return _bracketSimplifier.Simplify(renderedBeat);
        }
        catch (BracketSimplifierException e)
        {
            throw new TabMeasureRendererException($"Encountered an exception while simplifying brackets in measure {measure.MeasureNumber} - beat {beat}.", e);
        }
    }

    private string RenderToken(TabToken token)
    {
        return token switch
        {
            NoteGroupToken noteGroupToken => RenderNoteGroup(noteGroupToken),
            RestToken restToken => _restRenderer.RenderBeats(restToken.Length.Beats),
            _ => string.Empty
        };
    }

    private string RenderNoteGroup(NoteGroupToken token)
    {
        StringBuilder stringBuilder = new StringBuilder();
        string notes = _noteRenderer.RenderNotes(token.Notes);
        stringBuilder.Append(notes);

        stringBuilder.Append(_noteRenderingSettings.GetTrailingSpacesForBeatLength(token.Length));

        if (token.Length.Beats > 1m)
        {
            string rest = _restRenderer.RenderBeats(token.Length.Beats - 1m);
            stringBuilder.Append(rest);
        }

        return stringBuilder.ToString().Trim();
    }

    private static List<TabToken> PrepareTokensForBeat(TabMeasure measure, int beat)
    {
        List<TabToken> output = [];
        foreach (TabToken token in measure.Tokens)
        {
            TabToken tokenToAdd = token;

            // NoteGroupTokens that started before this beat have already had their numbers rendered.
            // We want to display rests instead.
            if (token is NoteGroupToken noteToken)
            {
                if (noteToken.Position.Beat < beat)
                {
                    tokenToAdd = new RestToken(noteToken.Position, noteToken.Length);
                }
            }

            if (TabTokenUtility.TryCutTokenToFitBeat(tokenToAdd, beat, out TabToken? cutToken)) output.Add(cutToken!);
        }

        return output.Distinct().OrderBy(t => t.Position).ToList();
    }
}