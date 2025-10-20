using System.Collections.Generic;
using System.Linq;
using GW2MidiConverter.GuildWars;

namespace GW2MidiConverter.Tab.Tokens;

public class NoteGroupToken(ICollection<GuildWarsNote> notes, TabElementLength length) : TabToken
{
    public ICollection<GuildWarsNote> Notes { get; } = notes;
    public TabElementLength Length { get; } = length;
    
    public override string ToString()
    {
        string notes = string.Join("/", Notes.Select(n => n.ToString()));
        string beatWord = Length.Beats == 1 ? "beat" : "beats";
        return $"{Length.Beats} {beatWord} of {notes}";
    }
}