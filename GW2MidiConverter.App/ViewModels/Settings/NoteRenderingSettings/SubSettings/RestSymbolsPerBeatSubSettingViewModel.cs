using System.Collections.Generic;
using GW2MidiConverter.Tab;

namespace GW2MidiConverter.App.ViewModels.Settings.NoteRenderingSettings.SubSettings;

public class RestSymbolsPerBeatSubSettingViewModel : DictionarySubSettingViewModel<TabElementLength, char, RestSymbolsPerBeatItemViewModel>
{
    public override bool VerifySetting()
    {
        // TODO MP: Verify!
        return true;
    }

    protected override bool VerifyItem(RestSymbolsPerBeatItemViewModel item)
    {
        return item.Length >= 0 &&
               item.Symbol != '\0' &&
               item.Symbol != ' ';
    }
    
    protected override RestSymbolsPerBeatItemViewModel CreateItemViewModel(TabElementLength key, char value)
    {
        return new RestSymbolsPerBeatItemViewModel(key, value);
    }

    protected override KeyValuePair<TabElementLength, char> CreateDictionaryEntryForItem(RestSymbolsPerBeatItemViewModel item)
    {
        return new KeyValuePair<TabElementLength, char>(new TabElementLength(item.Length), item.Symbol);
    }
}