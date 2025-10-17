using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GW2MidiConverter.GuildWars;
using GW2MidiConverter.Midi;
using GW2MidiConverter.Tab;
using GW2MidiConverter.Tab.Tokens;

namespace GW2MidiConverter.Rendering;

internal class TrackRenderer(TabRendererSettings settings)
{
    private readonly StringBuilder _stringBuilder = new StringBuilder();

    /// <summary>
    /// Renders the given track. This method only renders the actual content, and no headers like the instrument name.
    /// </summary>
    /// <param name="track">The track to render.</param>
    /// <returns>The rendered track.</returns>
    public string Render(TabTrack track)
    {
        if (track.Measures.Length == 0) return string.Empty;

        _stringBuilder.Clear();
        TimeSignature previousTimeSignature = new TimeSignature(0, 0);
        for (int i = 0; i < track.Measures.Length; i++)
        {
            TabMeasure measure = track.Measures[i];

            bool isLineBreakDue = i > 0 &&
                                  i < track.Measures.Length &&
                                  i % settings.MaxMeasureCountBeforeSpacing == 0;
            if (isLineBreakDue) _stringBuilder.AppendLine();

            RenderMeasureTimeSignature(measure.TimeSignature, previousTimeSignature);
            RenderMeasureDelimiter();
            RenderMeasure(measure);
            RenderMeasureDelimiter();

            _stringBuilder.AppendLine();
            previousTimeSignature = measure.TimeSignature;
        }

        return _stringBuilder.ToString().TrimStart().TrimStart('\n');
    }

    private void RenderMeasureDelimiter()
    {
        if (!settings.DrawMeasureDelimiters) return;
        _stringBuilder.Append(settings.MeasureDelimiter);
    }

    private void RenderMeasureTimeSignature(TimeSignature timeSignature, TimeSignature previousTimeSignature)
    {
        bool showTimeSignature = settings.TimeSignatureVisibility == TimeSignatureVisibility.AlwaysShow ||
                                 (settings.TimeSignatureVisibility == TimeSignatureVisibility.OnlyOnChanges && timeSignature != previousTimeSignature);
        if (!showTimeSignature)
        {
            return;
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
    }

    private void RenderMeasure(TabMeasure measure)
    {
        for (int i = 0; i < measure.Tokens.Length; i++)
        {
            TabToken token = measure.Tokens[i];
            switch (token)
            {
                case NoteGroupToken noteGroup:
                    _stringBuilder.Append(GetRenderedNoteGroup(noteGroup));
                    break;
                case RestToken restToken:
                    _stringBuilder.Append(GetRenderedRest(restToken));
                    break;
            }

            if (i < measure.Tokens.Length) _stringBuilder.Append(' ');
        }
    }

    private string GetRenderedNoteGroup(NoteGroupToken noteGroup)
    {
        List<string> notes = noteGroup.Notes.Select(GetNoteString).ToList();
        bool isDotted = noteGroup.Length.IsDotted;
        string dotString = isDotted ? "." : string.Empty;
        return string.Join("/", notes) + dotString;
    }

    private string GetNoteString(GuildWarsNote note)
    {
        string noteString = settings.KeyNames[note.Key];
        if (note.Octave == 1) return $"({noteString})";
        if (note.Octave == -1) return $"[{noteString}]";
        return noteString;
    }

    private string GetRenderedRest(RestToken restToken)
    {
        string rests = string.Empty;
        decimal remainingBeats = restToken.Length;
        while (remainingBeats > 0)
        {
            decimal decrement = remainingBeats >= 1 ? 1 : remainingBeats;

            string rest = decrement == 1 ? "—" : "-";
            rests += rest + " ";

            remainingBeats -= decrement;
        }

        return rests.TrimEnd();
    }
}