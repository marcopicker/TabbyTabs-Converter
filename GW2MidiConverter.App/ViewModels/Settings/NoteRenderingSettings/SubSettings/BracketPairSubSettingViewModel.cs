using System.Collections.Generic;
using System.Linq;
using GW2MidiConverter.GuildWars;
using GW2MidiConverter.Rendering;

namespace GW2MidiConverter.App.ViewModels.Settings.NoteRenderingSettings.SubSettings;

internal class BracketPairSubSettingViewModel : DictionarySubSettingViewModel<Octave, BracketPair, BracketPairItemViewModel>
{
    public override bool VerifySetting()
    {
        bool areOctavesUnique = Items.Select(item => item.GetOctave()).Distinct().Count() == Items.Count;
        return areOctavesUnique && Items.All(VerifyItem);
    }

    protected override bool VerifyItem(BracketPairItemViewModel item)
    {
        return item.OpeningBracket != '\0' &&
               item.OpeningBracket != ' ' &&
               item.ClosingBracket != '\0' &&
               item.ClosingBracket != ' ' &&
               item.OpeningBracket != item.ClosingBracket;
    }

    protected override BracketPairItemViewModel CreateItemViewModel(Octave key, BracketPair value)
    {
        return new BracketPairItemViewModel(key, value);
    }

    protected override KeyValuePair<Octave, BracketPair> CreateDictionaryEntryForItem(BracketPairItemViewModel item)
    {
        return new KeyValuePair<Octave, BracketPair>(item.GetOctave(), item.GetBracketPair());
    }
}