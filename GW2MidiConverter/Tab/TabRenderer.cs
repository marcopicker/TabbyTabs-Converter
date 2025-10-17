using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GW2MidiConverter.Tab.Tokens;

namespace GW2MidiConverter.Tab;

public class TabRenderer
{
    private readonly TabDocument _tabDocument;
    private readonly TabSettings _settings;

    public TabRenderer(TabDocument tabDocument, TabSettings? settings = null)
    {
        ArgumentNullException.ThrowIfNull(tabDocument);

        _tabDocument = tabDocument;
        _settings = settings ?? new TabSettings();
    }

    public string Render()
    {
        if (_tabDocument.Tracks.Length == 0) return string.Empty;

        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(RenderHeader());
        for (int i = 0; i < _tabDocument.Tracks.Length; i++)
        {
            TabTrack track = _tabDocument.Tracks[i];
            stringBuilder.Append(RenderTrack(track));

            bool isLastTrack = i == _tabDocument.Tracks.Length - 1;
            if (!isLastTrack)
            {
                stringBuilder.Append(RenderPostTrackMargin());
            }
        }

        return stringBuilder.ToString();
    }

    private string RenderHeader()
    {
        return "Hello, I am a header!\n\n";
    }

    private string RenderTrack(TabTrack track)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"{track.TrackInfo.InstrumentName}:");
        stringBuilder.Append(new string('\n', _settings.LineBreaksAfterTrackName));
        
        foreach (TabMeasure measure in track.Measures)
        {
            stringBuilder.AppendLine(RenderMeasure(measure));
        }
        
        return stringBuilder.ToString();
    }

    private string RenderMeasure(TabMeasure measure)
    {
        StringBuilder stringBuilder = new StringBuilder();
        string timeSignature = $"{measure.TimeSignature.Numerator}/{measure.TimeSignature.Denominator}:\t";
        stringBuilder.Append(timeSignature);
        
        foreach (TabToken token in measure.Tokens)
        {
            switch (token)
            {
                case NoteGroupToken noteGroup:
                    stringBuilder.Append(RenderNoteGroup(noteGroup));
                    break;
                case RestToken restToken:
                    stringBuilder.Append(RenderRest(restToken));
                    break;
            }
        }
        
        return stringBuilder.ToString();
    }

    private string RenderNoteGroup(NoteGroupToken noteGroup)
    {
        List<string> notes = noteGroup.Notes.Select(n => _settings.KeyNames[n.Key]).ToList();
        bool isDotted = noteGroup.Length.Beats is 0.75m or 1.5m or 3m;
        string dotString = isDotted ? "." : string.Empty;
        return string.Join("/", notes) + dotString + " ";
    }

    private string RenderRest(RestToken restToken)
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
        return rests;
    }

    private string RenderPostTrackMargin()
    {
        return new string('\n', _settings.LineBreaksBetweenTracks);
    }
}