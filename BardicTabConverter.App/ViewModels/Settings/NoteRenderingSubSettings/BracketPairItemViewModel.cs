using BardicTabConverter.GuildWars;
using BardicTabConverter.Rendering;

namespace BardicTabConverter.App.ViewModels.Settings.NoteRenderingSubSettings;

internal class BracketPairItemViewModel(Octave octave, BracketPair pair) : DictionarySubSettingItemViewModel
{
    private int _octave = octave.ToInt32();
    private char _openingBracket = pair.OpeningBracket;
    private char _closingBracket = pair.ClosingBracket;

    public int Octave
    {
        get => _octave;
        set => SetProperty(ref _octave, value);
    }

    public char OpeningBracket
    {
        get => _openingBracket;
        set => SetProperty(ref _openingBracket, value);
    }

    public char ClosingBracket
    {
        get => _closingBracket;
        set => SetProperty(ref _closingBracket, value);
    }
    
    public Octave GetOctave() => new Octave(Octave);

    public BracketPair GetBracketPair() => new BracketPair(OpeningBracket, ClosingBracket);
}