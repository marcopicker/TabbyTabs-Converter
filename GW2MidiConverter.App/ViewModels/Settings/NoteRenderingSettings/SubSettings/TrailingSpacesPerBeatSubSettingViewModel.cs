using System.Collections.Generic;
using System.Linq;
using GW2MidiConverter.Tab;

namespace GW2MidiConverter.App.ViewModels.Settings.NoteRenderingSettings.SubSettings;

internal class TrailingSpacesPerBeatSubSettingViewModel : DictionarySubSettingViewModel<TabElementLength, int, TrailingSpacesPerBeatItemViewModel>
{
    protected override TrailingSpacesPerBeatItemViewModel CreateItemViewModel(TabElementLength key, int value)
    {
        return new TrailingSpacesPerBeatItemViewModel(key, value);
    }

    protected override KeyValuePair<TabElementLength, int> CreateDictionaryEntryForItem(TrailingSpacesPerBeatItemViewModel item)
    {
        return new KeyValuePair<TabElementLength, int>(new TabElementLength(item.Length), item.SpaceCount);
    }

    protected override TrailingSpacesPerBeatItemViewModel CreateDefaultNewItemViewModel()
    {
        return new TrailingSpacesPerBeatItemViewModel(new TabElementLength(0.5m), 0);
    }

    protected override bool VerifyItem(TrailingSpacesPerBeatItemViewModel item, out string errorMessage)
    {
        if (item.Length < 0)
        {
            errorMessage = "Length must not be negative.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    protected override bool VerifySubSettingInContext()
    {
        bool areAllSettingsValid = true;
        List<IGrouping<decimal, TrailingSpacesPerBeatItemViewModel>> lengthGroups = Items.GroupBy(i => i.Length).ToList();
        foreach (List<TrailingSpacesPerBeatItemViewModel> list in lengthGroups.Select(group => group.ToList()))
        {
            if(list.Count == 1) continue;
            foreach (TrailingSpacesPerBeatItemViewModel item in list)
            {
                item.SetInvalid("Lengths must be unique.");
                areAllSettingsValid = false;
            }
        }
        return areAllSettingsValid;
    }
}