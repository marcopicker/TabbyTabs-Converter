using TabbyTabs.Converter.GuildWars;

namespace TabbyTabs.Converter.App.ViewModels.Settings.NoteRenderingSubSettings;

internal class KeyNameItemViewModel(GuildWarsNoteKey key, string token) : DictionarySubSettingItemViewModel
{
    private string _token = token;

    public GuildWarsNoteKey Key { get; } = key;

    public string Token
    {
        get => _token;
        set => SetProperty(ref _token, value);
    }
}