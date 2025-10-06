using GW2MidiConverter.App.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace GW2MidiConverter.App.Views.Pages;

public partial class MyPage : INavigableView<MyPageViewModel>
{
    public MyPageViewModel ViewModel { get; }

    public MyPage(MyPageViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}