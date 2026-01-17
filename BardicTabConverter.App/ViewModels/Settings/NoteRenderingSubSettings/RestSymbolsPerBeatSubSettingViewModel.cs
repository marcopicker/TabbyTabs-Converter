using System;
using System.Collections.Generic;
using System.Linq;
using BardicTabConverter.Tab;

namespace BardicTabConverter.App.ViewModels.Settings.NoteRenderingSubSettings;

internal class RestSymbolsPerBeatSubSettingViewModel : DictionarySubSettingViewModel<TabElementLength, char, RestSymbolsPerBeatItemViewModel>
{
    protected override bool VerifySubSettingInContext()
    {
        bool areAllSettingsValid = true;
        List<IGrouping<decimal, RestSymbolsPerBeatItemViewModel>> lengthGroups = Items.GroupBy(i => i.Length).ToList();
        foreach (List<RestSymbolsPerBeatItemViewModel> list in lengthGroups.Select(group => group.ToList()))
        {
            if(list.Count == 1) continue;
            foreach (RestSymbolsPerBeatItemViewModel item in list)
            {
                item.SetInvalid("Lengths must be unique.");
                areAllSettingsValid = false;
            }
        }
        return areAllSettingsValid;
    }

    protected override bool VerifyItem(RestSymbolsPerBeatItemViewModel item, out string errorMessage)
    {
        if (item.Length < 0 ||
            item.Symbol == '\0' ||
            item.Symbol == ' ')
        {
            errorMessage = "Invalid character for symbol.";
            return false;
        }
        
        errorMessage = string.Empty;
        return true;
    }
    
    protected override RestSymbolsPerBeatItemViewModel CreateItemViewModel(TabElementLength key, char value)
    {
        return new RestSymbolsPerBeatItemViewModel(key, value);
    }

    protected override KeyValuePair<TabElementLength, char> CreateDictionaryEntryForItem(RestSymbolsPerBeatItemViewModel item)
    {
        TabElementLength length = new TabElementLength(Math.Max(0, item.Length));
        return new KeyValuePair<TabElementLength, char>(length, item.Symbol);
    }

    protected override RestSymbolsPerBeatItemViewModel CreateDefaultNewItemViewModel()
    {
        return new RestSymbolsPerBeatItemViewModel(new TabElementLength(0.5m), '-');
    }
}