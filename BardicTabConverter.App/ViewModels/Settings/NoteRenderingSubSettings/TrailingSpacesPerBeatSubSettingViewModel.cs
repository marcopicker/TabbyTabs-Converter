using System;
using System.Collections.Generic;
using System.Linq;
using BardicTabConverter.Tab;

namespace BardicTabConverter.App.ViewModels.Settings.NoteRenderingSubSettings;

internal class TrailingSpacesPerBeatSubSettingViewModel : DictionarySubSettingViewModel<TabElementLength, int, TrailingSpacesPerBeatItemViewModel>
{
    protected override TrailingSpacesPerBeatItemViewModel CreateItemViewModel(TabElementLength key, int value)
    {
        return new TrailingSpacesPerBeatItemViewModel(key, value);
    }

    protected override KeyValuePair<TabElementLength, int> CreateDictionaryEntryForItem(TrailingSpacesPerBeatItemViewModel item)
    {
        TabElementLength length = new TabElementLength(Math.Max(0, item.Length));
        return new KeyValuePair<TabElementLength, int>(length, item.SpaceCount);
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
            if (list.Count == 1) continue;
            foreach (TrailingSpacesPerBeatItemViewModel item in list)
            {
                item.SetInvalid("Lengths must be unique.");
                areAllSettingsValid = false;
            }
        }

        return areAllSettingsValid;
    }
}