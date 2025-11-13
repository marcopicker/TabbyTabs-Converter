using System;

namespace GW2MidiConverter.App.ViewModels.Settings;

internal class AtomicSubSettingViewModel<T> : SubSettingViewModel<T>
{
    private string? _errorMessage;
    private T? _value;

    public T? Value
    {
        get => _value;
        set
        {
            if (!SetProperty(ref _value, value)) return;
            NotifySettingChanged();
        }
    }

    public Func<bool>? VerifySettingFunc { get; set; }
    
    public string? ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public AtomicSubSettingViewModel()
    {
        _value = Value = default;
    }

    public override bool VerifySetting()
    {
        return VerifySettingFunc?.Invoke() ?? true;
    }

    public override void ApplySubSettingObject(T setting)
    {
        Value = setting;
    }

    public override T? GetSubSettingObject()
    {
        return Value;
    }
}