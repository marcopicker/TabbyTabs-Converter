using GW2MidiConverter.App.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace GW2MidiConverter.App.Views.Pages;

public partial class AboutPage : INavigableView<AboutPageViewModel>
{
    public AboutPageViewModel ViewModel { get; }

    public AboutPage(AboutPageViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}