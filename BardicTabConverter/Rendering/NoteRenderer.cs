using System;
using System.Collections.Generic;
using System.Linq;
using BardicTabConverter.GuildWars;
using BardicTabConverter.Rendering.Settings;

namespace BardicTabConverter.Rendering;

public class NoteRenderer
{
    private readonly NoteRenderingSettings _settings;
    private readonly BracketSimplifier _bracketSimplifier;

    public NoteRenderer(NoteRenderingSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        _settings = settings;
        _bracketSimplifier = new BracketSimplifier(settings.BracketPairsPerOctave.Values.ToList());
    }

    public string RenderNotes(ICollection<GuildWarsNote> notes)
    {
        List<string> noteStrings = notes.Select(GetNoteString).ToList();
        string combinedNotes = string.Join("/", noteStrings);
        string simplifiedNotes = _bracketSimplifier.Simplify(combinedNotes);
        return simplifiedNotes;
    }

    private string GetNoteString(GuildWarsNote note)
    {
        string keyString = _settings.KeyNames[note.Key];
        string outputString = ApplyOctave(keyString, note.Octave);
        return outputString;
    }

    private string ApplyOctave(string noteString, Octave octave)
    {
        if (!_settings.BracketPairsPerOctave.TryGetValue(octave, out BracketPair? bracketPair))
        {
            return noteString;
        }
        
        return $"{bracketPair.OpeningBracket}{noteString}{bracketPair.ClosingBracket}";
    }
}