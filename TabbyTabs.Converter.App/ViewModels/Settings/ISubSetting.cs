using System;

namespace TabbyTabs.Converter.App.ViewModels.Settings;

internal interface ISubSetting
{
    public event EventHandler? SettingChanged;
    public bool VerifySetting();
}