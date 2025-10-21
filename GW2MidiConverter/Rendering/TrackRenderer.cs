using System.Collections.Generic;
using System.Linq;
using System.Text;
using GW2MidiConverter.GuildWars;
using GW2MidiConverter.Midi;
using GW2MidiConverter.Rendering.Settings;
using GW2MidiConverter.Tab;
using GW2MidiConverter.Tab.Tokens;

namespace GW2MidiConverter.Rendering;

internal class TrackRenderer(TabRendererSettings settings)
{
    private readonly StringBuilder _stringBuilder = new StringBuilder();
    private readonly RestRenderer _restRenderer = new RestRenderer(settings);

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
            _stringBuilder.Append(GetRenderedMeasure(measure));
            RenderMeasureDelimiter();

            _stringBuilder.AppendLine();
            previousTimeSignature = measure.TimeSignature;
            measureInRowsCounter++;
        }

        return _stringBuilder.ToString().TrimStart().TrimStart('\n');
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

    private string GetRenderedMeasure(TabMeasure measure)
    {
        StringBuilder stringBuilder = new StringBuilder();
        // stringBuilder.Append($"{(measure.MeasureNumber + 1).ToString("D2")}: ");
        bool skipNextSpace = false;
        for (int i = 0; i < measure.Tokens.Count; i++)
        {
            TabToken token = measure.Tokens[i];
            switch (token)
            {
                case NoteGroupToken noteGroup:
                    stringBuilder.Append(GetRenderedNoteGroup(noteGroup));
                    if (noteGroup.Length.Beats < 0.25m) skipNextSpace = true;
                    break;
                case RestToken restToken:
                    stringBuilder.Append(_restRenderer.RenderBeats(restToken.Length.Beats));
                    break;
            }

            if (!skipNextSpace && i < measure.Tokens.Count) stringBuilder.Append(' ');
            skipNextSpace = false;
        }

        string output = stringBuilder.ToString();
        return RemoveUnnecessaryOctaveSymbols(output);
    }

    // TODO MP: Store this in a member variable.
    private string RemoveUnnecessaryOctaveSymbols(string input)
    {
        BracketSimplifier simplifier = new BracketSimplifier(settings.BracketPairsPerOctave.Values.ToList());
        return simplifier.Simplify(input);
    }

    private string GetRenderedNoteGroup(NoteGroupToken noteGroup)
    {
        List<string> notes = noteGroup.Notes.Select(GetNoteString).ToList();
        string notesString = string.Join("/", notes);
        string restString = string.Empty;
        if (noteGroup.Length.Beats > 1m)
        {
            restString = _restRenderer.RenderBeats(noteGroup.Length.Beats - 1m);
        }

        return notesString + restString;
    }

    private string GetNoteString(GuildWarsNote note)
    {
        string noteString = settings.KeyNames[note.Key];
        if (note.Octave == 1) return $"({noteString})";
        if (note.Octave == -1) return $"[{noteString}]";
        return noteString;
    }
}