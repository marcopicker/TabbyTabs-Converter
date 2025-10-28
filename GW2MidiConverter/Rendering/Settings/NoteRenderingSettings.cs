using System.Collections.Generic;
using GW2MidiConverter.GuildWars;
using GW2MidiConverter.Tab;

namespace GW2MidiConverter.Rendering.Settings;

public record NoteRenderingSettings
{
    public IReadOnlyDictionary<GuildWarsNoteKey, string> KeyNames { get; init; } = new Dictionary<GuildWarsNoteKey, string>
    {
        [GuildWarsNoteKey.Note1] = "1",
        [GuildWarsNoteKey.Note1Sharp] = "#1",
        [GuildWarsNoteKey.Note2] = "2",
        [GuildWarsNoteKey.Note2Sharp] = "#2",
        [GuildWarsNoteKey.Note3] = "3",
        [GuildWarsNoteKey.Note4] = "4",
        [GuildWarsNoteKey.Note4Sharp] = "#4",
        [GuildWarsNoteKey.Note5] = "5",
        [GuildWarsNoteKey.Note5Sharp] = "#5",
        [GuildWarsNoteKey.Note6] = "6",
        [GuildWarsNoteKey.Note6Sharp] = "#6",
        [GuildWarsNoteKey.Note7] = "7",
        [GuildWarsNoteKey.Note8] = "8",
    };
    
    public IReadOnlyDictionary<Octave, BracketPair> BracketPairsPerOctave { get; init; } = new Dictionary<Octave, BracketPair>
    {
        [new Octave(-1)] = new BracketPair('[', ']'),
        [new Octave(1)] = new BracketPair('(', ')'),
    };
    
    public IReadOnlyDictionary<TabElementLength, char> RestSymbolsPerBeat { get; init; } = new Dictionary<TabElementLength, char>
    {
        [1.00m] = '—',
        [0.50m] = '-',
        [0.25m] = '∙'
    };
    
    public IReadOnlyDictionary<TabElementLength, int> TrailingSpacesPerBeatLength { get; init; } = new Dictionary<TabElementLength, int>
    {
        [new TabElementLength(1.00m)] = 3,
        [new TabElementLength(0.50m)] = 2,
        [new TabElementLength(0.25m)] = 1,
    };
}