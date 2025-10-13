using System;
using GW2MidiConverter.GuildWars;

namespace GW2MidiConverter.Converters.Tokens.Tab;

public class NotesTabToken : TabToken
{
    public NotesTabToken(GuildWarsNote[] notes)
    {
        if (notes.Length == 0) throw new ArgumentException("Notes must not be empty.", nameof(notes));
        Notes = notes;
    }

    public GuildWarsNote[] Notes { get; }
}