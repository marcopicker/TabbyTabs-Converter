using GW2MidiConverter.App.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace GW2MidiConverter.App.Views.Pages;

public partial class PreferencesPage : INavigableView<PreferencePageViewModel>
{
    public PreferencePageViewModel ViewModel { get; }

    public PreferencesPage(PreferencePageViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}