using System.Collections.Generic;
using GW2MidiConverter.GuildWars;

namespace GW2MidiConverter.App.ViewModels.Settings.NoteRenderingSettings.SubSettings;

internal class KeyNamesSubSettingViewModel : DictionarySubSettingViewModel<GuildWarsNoteKey, string, KeyNameItemViewModel>
{
    protected override bool VerifyItem(KeyNameItemViewModel item, out string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(item.Token))
        {
            errorMessage = "Text must not be empty.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    protected override KeyNameItemViewModel CreateItemViewModel(GuildWarsNoteKey key, string value)
    {
        return new KeyNameItemViewModel(key, value);
    }

    protected override KeyValuePair<GuildWarsNoteKey, string> CreateDictionaryEntryForItem(KeyNameItemViewModel item)
    {
        return new KeyValuePair<GuildWarsNoteKey, string>(item.Key, item.Token);
    }
}