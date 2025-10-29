using CommunityToolkit.Mvvm.ComponentModel;

namespace GW2MidiConverter.App.ViewModels.Settings;

internal abstract class DictionarySubSettingItemViewModel : ObservableObject
{
    private bool _isValid = true;
    private string _invalidMessage = string.Empty;

    public bool IsValid
    {
        get => _isValid;
        private set => SetProperty(ref _isValid, value);
    }

    public string InvalidMessage
    {
        get => _invalidMessage;
        private set => SetProperty(ref _invalidMessage, value);
    }

    public void SetValid() => IsValid = true;

    public void SetInvalid(string message)
    {
        IsValid = false;
        InvalidMessage = message;
    }
}