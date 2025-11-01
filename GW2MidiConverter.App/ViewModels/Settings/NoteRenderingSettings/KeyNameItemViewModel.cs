using GW2MidiConverter.GuildWars;

namespace GW2MidiConverter.App.ViewModels.Settings.NoteRenderingSettings;

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