using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using GW2MidiConverter.App.Views.Pages;
using Wpf.Ui.Controls;

namespace GW2MidiConverter.App.ViewModels;

public class MainWindowViewModel : ObservableObject
{
    public ObservableCollection<object> MenuItems
    {
        get => _menuItems;
        set => SetProperty(ref _menuItems, value);
    }

    public ObservableCollection<object> FooterMenuItems
    {
        get => _footerMenuItems;
        set => SetProperty(ref _footerMenuItems, value);
    }

    public object ApplicationTitle => "GW2MidiConverter.App";

    private ObservableCollection<object> _menuItems =
    [
        new NavigationViewItem("MyPage", SymbolRegular.Home24, typeof(MyPage)),
    ];

    private ObservableCollection<object> _footerMenuItems =
    [
        new NavigationViewItem("About", SymbolRegular.Info24, typeof(AboutPage)),
        new NavigationViewItemSeparator(),
        new NavigationViewItem("Settings", SymbolRegular.Settings24, typeof(PreferencesPage))
    ];
}