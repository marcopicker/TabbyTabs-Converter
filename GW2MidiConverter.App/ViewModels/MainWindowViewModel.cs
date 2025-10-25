using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using GW2MidiConverter.App.ViewModels.Pages;
using GW2MidiConverter.App.Views;
using GW2MidiConverter.App.Views.Pages;
using Wpf.Ui.Controls;

namespace GW2MidiConverter.App.ViewModels;

public class MainWindowViewModel : ObservableObject
{
    private object _currentViewModel;
    private string _tabText;

    public object ApplicationTitle => "GW2 Tab Converter";

    public object CurrentViewModel
    {
        get => _currentViewModel;
        set => SetProperty(ref _currentViewModel, value);
    }

    public string TabText
    {
        get => _tabText;
        set => SetProperty(ref _tabText, value);
    }

    public MainWindowViewModel(IServiceProvider serviceProvider)
    {
        TabText = "Hello, I am a header!\n\nPiano 1:\n9/8: \n(3)    —   (2   3)    —   (4)    -  (3)    —   (2)\n\n4/8: \n8   7    —    —    -\n\n6/8: \n8    —   7   (1)    -  (2)    —\n\n5/8: \n(3)    —   (1)    —   -  (2)\n\n9/8: \n(3)    —   (2   3)    —   (4)    -  (5)    —   (4)\n(3)    —   (2)   8    —    —   6    ∙ -  7\n\n6/8: \n4/8    —   7   (1)    -  (2)    —\n3/8   7    —   -    ∙ -  6   7    -\n2/8    —   7   #4/(1)    -  (2)    —\n\n8/8: \n#2/8    —   3    —    —   6   7    -\n\n6/8: \n4/8    —   7   #4/(1)    -  (2)    —\n\n9/8: \n5/8   7    —    —   6    —   #5    -  —    -\n\n12/8: \n[4]/6    —   2    —    —   [5]/3    -  —   #1    —    —    -\n\n5/8: \n[6]/2    —    —    -  ∙\n—    —    —    —    —\n\n4/8: \n—    —    —    —\n\n5/8: \n—    —   6    —   -  7\n\n4/8: \n8    —   7   (1)    -\n\n9/8: \n(2)    —   (3)    —    —    —    —    -  ∙\n\n4/8: \n—    —    —    —\n—    —    —    —\n\n3/8: \n—   (2)    -  (3)\n\n4/8: \n#6/(2/4   3)    -  4/#6/(2)   8    -\n\n5/8: \n2/#4/6/(2)    —   -    -  ∙  —    -\n\n4/8: \n—    —    —    —\n—    —    —    —\n\n6/8: \n—    —    —   (2)    —   -    —\n(2)    —   (2   2)    —   (2)    -\n(2)    —   (#2   2)    —   (2)    -\n(4)    —   (4   4)    —   (#2)    -\n\n(2)   #6    -  4   2    —    —    -\n—    —    —    —    —    —\n1/#2/5/8    —   1/8   2/(2)    —   #2/(#2)    -\n(2)   2    -  5   2/#2/#6    -  [#6]    —\n\n#6    —   8  #6  6    —   5    -\n2   #4    -  6   (2)    -  (#4   6)    -\n(2/8)    —   (2)    —    —    -\n—    —    —    —    —    —\n\n—    —    —    —    —    —\n(6  4  6  4)    ∙ (2  4  6  4)    ∙ (6  4  2  4)    ∙\n(6  4  6  4)    ∙ (2)  6  (4  2)    ∙ 6  (2  6  4)    ∙\n(8  6  8  6)    ∙ (3  1  8  6)    ∙ (8  3  #6  3)    ∙\n\n(6  4  6  4)    ∙ 8  6  (6  4)    ∙ (6)  8  6  8    ∙\n2  5  #6  (2)    -  ∙  #6  2    ∙ 5  #6  (2)  #6    ∙\n6/8  4  6/8  4    ∙ 6   #6/(2)  4    ∙ #6/(2)  4  #6    -\n4/#6  2  4/#6  2    —   4/#6  2    ∙ 4/#6  2    -  ∙\n\n1   3    -  1   5    -  3   1    -\n5  #5  6  (1)    ∙ (2  #2  3  5)    ∙ (6  #6  6  5)    ∙\n(6#6 6 #66 #6 6#66)    ∙ (#66 #6 6#6 6 #66)    (#6 6#6 6 #66 #6)\n\n\nPiano 2:\n9/8: \n—    —    —    —    —    —    —    —\n\n4/8: \n—    —    —    —\n\n6/8: \n—    —    —    —    —    —\n\n5/8: \n—    —    —    —    —\n\n9/8: \n—    —    —    —    —    —    —    —\n—    —    —    —    —    —    —    —\n\n6/8: \n—    —    —    —    —    —\n—    —    —    —    —    —\n—    —    —    —    —    —\n\n8/8: \n—    —    —    —    —    —    —    —\n\n6/8: \n—    —    —    —    —    —\n\n9/8: \n—    —    —    —    —    —    —    —\n\n12/8: \n—    —    —    —    —    —    —    —\n\n5/8: \n—    —    —    —    —\n—    —   [6]    —   -  [7]\n\n4/8: \n[8]    —   [7]   1    -\n\n5/8: \n2    —   3    —    —\n\n4/8: \n—    —    —    —\n\n9/8: \n—    —    —    —   2    —   -  3    —    —\n\n4/8: \n[#6]/2/4    —   3   [4/#6]/2    -\n[8]    —   [2]/2    —\n\n3/8: \n—    —    —\n\n4/8: \n—    —    —    —\n\n5/8: \n—    —   2    —   -  3\n\n4/8: \n[5/#6]/4   3    -  [4/6]/2   [8]    -\n[2/5]/2    —   [7]    —\n\n6/8: \n[2/#4]/2    —    —    -  ∙  —\n[5]   2    -  5   #6    -  5   2    -\n[5]   2    -  5   #6    -  5   2    -\n[4]   1    -  4   6    -  4   1    -\n\n#6   4    -  2   [#6]    —    —    -\n—    —    —    —    —    —\n[5]    —    —    —    —    —\n5    —   [#6   5/#6]    —    —    -\n\n[#6   5]    -  [8  #6  6]    -  [5   5]    -\n[2   6]    -  [#4]   2    -  [6   #4]    -\n[6]    —   [2]    —    —    -\n6   4    -  2   4    -  2   [6]    -\n\n6   4    -  2   4    -  2   [6]    -\n[4/6]    —   [4/6   4/6]    —   [4/6]    -\n[2]    —   [#6]/2   [6]    —   [2]    -\n[1/3]/1/3    —   [8   8]    —   [#6]    -\n\n[4/6   8]    -  [#6   4]    -  [1   4]    -\n[5]    —   [5   6]    —   [#6]    -\n[6]    —   [#6 6 5 4]    —    —    -\n[4]    —   [6 5 4 3]    —   [2]    -\n\n[1   2]    -  ∙ [2   3]    —   [4  4]    -\n—    —    —    —    —    —\n—    —    —    —    —    —";
    }
}