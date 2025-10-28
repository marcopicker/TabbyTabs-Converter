using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace GW2MidiConverter.App.ViewModels.Settings;

public abstract class SubSettingViewModel<TSetting> : ObservableObject, ISubSetting
{
    public event EventHandler? SettingChanged;
    
    public abstract bool VerifySetting();
    public abstract void ApplySubSettingObject(TSetting setting);
    public abstract TSetting GetSubSettingObject();

    protected void OnSettingChanged()
    {
        SettingChanged?.Invoke(this, EventArgs.Empty);
    }
}