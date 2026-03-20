using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TabbyTabs.Converter.App.ViewModels.Settings;

public abstract class SubSettingViewModel<TSetting> : ObservableObject, ISubSetting
{
    private bool _isValid;
    public event EventHandler? SettingChanged;

    public bool IsValid
    {
        get => _isValid;
        set => SetProperty(ref _isValid, value);
    }

    public abstract bool VerifySetting();
    public abstract void ApplySubSettingObject(TSetting setting);
    public abstract TSetting? GetSubSettingObject();

    protected void NotifySettingChanged()
    {
        SettingChanged?.Invoke(this, EventArgs.Empty);
    }
}