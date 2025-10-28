using System.Collections.Generic;
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

    protected override bool VerifyItem(TrailingSpacesPerBeatItemViewModel item)
    {
        return item.Length >= 0;
    }
}