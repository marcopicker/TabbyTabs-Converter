using System.Collections.Generic;
using GW2MidiConverter.GuildWars;

namespace GW2MidiConverter.Tab;

public class TabSettings
{
    public int LineBreaksAfterTrackName { get; set; } = 0;
    public int LineBreaksBetweenTracks { get; set; } = 2;

    public Dictionary<GuildWarsNoteKey, string> KeyNames { get; private set; } = new Dictionary<GuildWarsNoteKey, string>()
    {
        { GuildWarsNoteKey.Note1, "1" },
        { GuildWarsNoteKey.Note1Sharp, "#1" },
        { GuildWarsNoteKey.Note2, "2" },
        { GuildWarsNoteKey.Note2Sharp, "#2" },
        { GuildWarsNoteKey.Note3, "3" },
        { GuildWarsNoteKey.Note4, "4" },
        { GuildWarsNoteKey.Note4Sharp, "#4" },
        { GuildWarsNoteKey.Note5, "5" },
        { GuildWarsNoteKey.Note5Sharp, "#5" },
        { GuildWarsNoteKey.Note6, "6" },
        { GuildWarsNoteKey.Note6Sharp, "#6" },
        { GuildWarsNoteKey.Note7, "7" },
        { GuildWarsNoteKey.Note8, "8" },
    };
}