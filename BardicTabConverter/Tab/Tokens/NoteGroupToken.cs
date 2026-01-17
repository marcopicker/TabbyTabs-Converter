using System.Collections.Generic;
using System.Linq;
using BardicTabConverter.GuildWars;
using BardicTabConverter.Midi;

namespace BardicTabConverter.Tab.Tokens;

public record NoteGroupToken(ICollection<GuildWarsNote> Notes, SongPosition Position, TabElementLength Length) : TabToken(Position, Length)
{
    public override string ToString()
    {
        string notes = string.Join("/", Notes.Select(n => n.ToString()));
        string beatWord = Length.Beats == 1 ? "beat" : "beats";
        return $"{Length.Beats} {beatWord} of {notes}";
    }
}