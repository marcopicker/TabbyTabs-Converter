using System;

namespace BardicTabConverter.App.ViewModels.Settings;

internal interface ISubSetting
{
    public event EventHandler? SettingChanged;
    public bool VerifySetting();
}