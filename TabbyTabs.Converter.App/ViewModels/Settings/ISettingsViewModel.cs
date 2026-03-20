using System;

namespace TabbyTabs.Converter.App.ViewModels.Settings;

internal interface ISettingsViewModel
{
    public event EventHandler? SubSettingChanged;
}