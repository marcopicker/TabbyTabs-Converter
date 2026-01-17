using System;

namespace BardicTabConverter.App.ViewModels.Settings;

internal interface ISettingsViewModel
{
    public event EventHandler? SubSettingChanged;
}