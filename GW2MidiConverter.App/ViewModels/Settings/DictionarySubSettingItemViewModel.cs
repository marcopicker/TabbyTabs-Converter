using CommunityToolkit.Mvvm.ComponentModel;
using GW2MidiConverter.App.Controls;

namespace GW2MidiConverter.App.ViewModels.Settings;

internal abstract class DictionarySubSettingItemViewModel : ObservableObject
{
    private ValidityType _validity = ValidityType.Valid;
    private string? _invalidMessage = string.Empty;

    public ValidityType Validity
    {
        get => _validity;
        private set => SetProperty(ref _validity, value);
    }

    public string? InvalidMessage
    {
        get => _invalidMessage;
        private set => SetProperty(ref _invalidMessage, value);
    }

    public void SetValid()
    {
        Validity = ValidityType.Valid;
        InvalidMessage = null;
    }

    public void SetInvalid(string? message)
    {
        Validity = ValidityType.Invalid;
        InvalidMessage = message;
    }
}