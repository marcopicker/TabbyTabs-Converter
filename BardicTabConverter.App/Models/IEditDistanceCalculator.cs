namespace BardicTabConverter.App.Models;

internal interface IEditDistanceCalculator
{
    int CalculateDistance(string first, string second);
}