using System;
using System.Collections.Generic;
using System.Text;
using GW2MidiConverter.Rendering.Settings;
using GW2MidiConverter.Tab;
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
            _measureStringBuilder.Append(RenderBeat(measure, i + 1));
            _measureStringBuilder.Append(new string(' ', _settings.SpacesBetweenBeats));
        }

        return _measureStringBuilder.ToString().Trim();
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
            stringBuilder.Append(_settings.GetTrailingSpacesForBeatLength(token.Length));
        }

        return stringBuilder.ToString().Trim();
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

        stringBuilder.Append(_settings.GetTrailingSpacesForBeatLength(token.Length));

        if (token.Length.Beats > 1m)
        {
            string rest = _restRenderer.RenderBeats(token.Length.Beats - 1m);
            stringBuilder.Append(rest);
        }

        return stringBuilder.ToString().Trim();
    }

    private static List<TabToken> PrepareTokensForBeat(TabMeasure measure, int beat)
    {
        IReadOnlyList<TabToken> tokens = GetAllTokensThatTouchBeat(measure, beat);
        List<TabToken> output = [];
        foreach (TabToken token in tokens)
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
        return output;
    }
    
    private static List<TabToken> GetAllTokensThatTouchBeat(TabMeasure measure, int beat)
    {
        List<TabToken> tokens = [];

        foreach (TabToken token in measure.Tokens)
        {
            // If they start in the given beat, it's straightforward. :)
            if (token.Position.Beat == beat)
            {
                tokens.Add(token);
                continue;
            }

            // Ignore tokens that start after the given beat.
            if (token.Position.Beat > beat) continue;

            // The token now begins before this beat.
            if (token.Position.Beat + token.Length.Beats >= beat)
            {
                tokens.Add(token);
            }
        }

        return tokens;
    }
}