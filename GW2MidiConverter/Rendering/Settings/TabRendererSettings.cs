using System.Collections.Generic;
using GW2MidiConverter.GuildWars;

namespace GW2MidiConverter.Rendering.Settings;

public class TabRendererSettings
{
    public int LineBreaksAfterTrackName { get; init; } = 0;
    public int LineBreaksBetweenTracks { get; init; } = 2;
    public OctaveRenderingMode OctaveRenderingMode { get; init; } = OctaveRenderingMode.PerMeasure;
    public TimeSignatureVisibility TimeSignatureVisibility { get; init; } = TimeSignatureVisibility.OnlyOnChanges;
    public TimeSignaturePositioning TimeSignaturePositioning { get; init; } = TimeSignaturePositioning.AboveMeasure;
    public int LinesBeforeTimeSignature { get; init; } = 1;
    public int LinesAfterTimeSignature { get; init; } = 0;
    public int MaxMeasureCountBeforeSpacing { get; init; } = 4;

    public char MeasureDelimiter { get; init; } = '|';
    public bool DrawMeasureDelimiters { get; init; } = false;

    public Dictionary<Octave, BracketPair> BracketPairsPerOctave { get; init; } = new Dictionary<Octave, BracketPair>
    {
        [new Octave(-1)] = new BracketPair('[', ']'),
        [new Octave(1)] = new BracketPair('(', ')'),
    };
    
    public Dictionary<decimal, char> RestSymbolsPerBeat { get; init; } = new Dictionary<decimal, char>
    {
        [1.00m] = '—',
        [0.50m] = '-',
        [0.25m] = '∙'
    };

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