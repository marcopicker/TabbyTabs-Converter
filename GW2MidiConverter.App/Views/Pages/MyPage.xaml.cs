using GW2MidiConverter.App.ViewModels.Pages;

namespace GW2MidiConverter.App.Views.Pages;

public partial class MyPage
{
    public MyPageViewModel ViewModel { get; }

    public MyPage(MyPageViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}