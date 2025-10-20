using System;
using GW2MidiConverter.Rendering.Settings;
using GW2MidiConverter.Tab;
using GW2MidiConverter.Tab.Tokens;

namespace GW2MidiConverter.Rendering;

public class NoteGroupTokenRenderer
{
    private readonly NoteRenderer _noteRenderer;
    private readonly RestTokenRenderer _restTokenRenderer;

    public NoteGroupTokenRenderer(TabRendererSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);
        _noteRenderer = new NoteRenderer(settings);
        _restTokenRenderer = new RestTokenRenderer(settings);
    }

    public string RenderToken(NoteGroupToken token)
    {
        ArgumentNullException.ThrowIfNull(token);
        if (token.Length == new TabElementLength(0)) return string.Empty;

        string noteString = _noteRenderer.RenderNotes(token.Notes);
        if (token.Length <= new TabElementLength(1)) return noteString;
        
        TabElementLength remainingLength = new TabElementLength(token.Length.Beats - 1);
        string restString = _restTokenRenderer.RenderToken(new RestToken(remainingLength));
        return $"{noteString} {restString}";
    }
}