using System;

namespace GW2MidiConverter.App.ViewModels.Settings;

internal interface ISubSetting
{
    public event EventHandler? SettingChanged;
    public bool VerifySetting();
}