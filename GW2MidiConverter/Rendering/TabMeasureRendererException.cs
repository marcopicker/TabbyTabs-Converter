using System;

namespace GW2MidiConverter.Rendering;

public class TabMeasureRendererException : Exception
{
    public TabMeasureRendererException() {}
    public TabMeasureRendererException(string message) : base(message) {}
    public TabMeasureRendererException(string message, Exception innerException) : base(message, innerException) {}
}