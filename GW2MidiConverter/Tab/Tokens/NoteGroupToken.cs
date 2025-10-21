using System.Collections.Generic;
using System.Linq;
using GW2MidiConverter.GuildWars;
using GW2MidiConverter.Midi;

namespace GW2MidiConverter.Tab.Tokens;

public record NoteGroupToken(ICollection<GuildWarsNote> Notes, SongPosition Position, TabElementLength Length) : TabToken(Position, Length)
{
    public override string ToString()
    {
        string notes = string.Join("/", Notes.Select(n => n.ToString()));
        string beatWord = Length.Beats == 1 ? "beat" : "beats";
        return $"{Length.Beats} {beatWord} of {notes}";
    }
}