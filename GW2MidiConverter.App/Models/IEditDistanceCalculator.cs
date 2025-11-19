namespace GW2MidiConverter.App.Models;

internal interface IEditDistanceCalculator
{
    int CalculateDistance(string first, string second);
}