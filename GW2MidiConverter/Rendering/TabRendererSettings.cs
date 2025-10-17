using System.Collections.Generic;
using GW2MidiConverter.GuildWars;

namespace GW2MidiConverter.Rendering;

public class TabRendererSettings
{
    public int LineBreaksAfterTrackName { get; set; } = 0;
    public int LineBreaksBetweenTracks { get; set; } = 2;
    public OctaveRenderingMode OctaveRenderingMode { get; set; } = OctaveRenderingMode.PerMeasure;
    public TimeSignatureVisibility TimeSignatureVisibility { get; set; } = TimeSignatureVisibility.OnlyOnChanges;
    public TimeSignaturePositioning TimeSignaturePositioning { get; set; } = TimeSignaturePositioning.AboveMeasure;
    public int LinesBeforeTimeSignature { get; set; } = 1;
    public int LinesAfterTimeSignature { get; set; } = 0;
    public int MaxMeasureCountBeforeSpacing { get; set; } = 4;
    
    public char MeasureDelimiter { get; set; } = '|';
    public bool DrawMeasureDelimiters { get; set; } = false;

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