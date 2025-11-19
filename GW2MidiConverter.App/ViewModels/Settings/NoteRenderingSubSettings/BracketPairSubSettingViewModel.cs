using System.Collections.Generic;
using System.Linq;
using GW2MidiConverter.GuildWars;
using GW2MidiConverter.Rendering;

namespace GW2MidiConverter.App.ViewModels.Settings.NoteRenderingSubSettings;

internal class BracketPairSubSettingViewModel : DictionarySubSettingViewModel<Octave, BracketPair, BracketPairItemViewModel>
{
    protected override bool VerifySubSettingInContext()
    {
        bool isSettingValid = true;
        List<IGrouping<Octave, BracketPairItemViewModel>> octaveGroups = Items.GroupBy(i => i.GetOctave()).ToList();
        foreach (List<BracketPairItemViewModel> list in octaveGroups.Select(group => group.ToList()))
        {
            if (list.Count == 1) continue;
            foreach (BracketPairItemViewModel item in list)
            {
                item.SetInvalid("Octaves must be unique.");
                isSettingValid = false;
            }
        }

        foreach (BracketPairItemViewModel item in Items)
        {
            bool bracketsAreDuplicates = Items.Where(i => i != item)
                                              .Any(i => i.GetBracketPair().GetBracketCharacters().Contains(item.OpeningBracket) ||
                                                        i.GetBracketPair().GetBracketCharacters().Contains(item.ClosingBracket));
            if (!bracketsAreDuplicates) continue;
            item.SetInvalid("Brackets must be unique.");
            isSettingValid = false;
        }

        return isSettingValid;
    }

    protected override bool VerifyItem(BracketPairItemViewModel item, out string errorMessage)
    {
        if (item.OpeningBracket == '\0' ||
            item.OpeningBracket == ' ' ||
            item.ClosingBracket == '\0' ||
            item.ClosingBracket == ' ')
        {
            errorMessage = "Invalid character for opening or closing bracket.";
            return false;
        }

        if (item.OpeningBracket == item.ClosingBracket)
        {
            errorMessage = "Opening and closing brackets must be different.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    protected override BracketPairItemViewModel CreateItemViewModel(Octave key, BracketPair value)
    {
        return new BracketPairItemViewModel(key, value);
    }

    protected override KeyValuePair<Octave, BracketPair> CreateDictionaryEntryForItem(BracketPairItemViewModel item)
    {
        return new KeyValuePair<Octave, BracketPair>(item.GetOctave(), item.GetBracketPair());
    }

    protected override BracketPairItemViewModel CreateDefaultNewItemViewModel()
    {
        return new BracketPairItemViewModel(new Octave(0), new BracketPair('(', ')'));
    }
}