using System;

namespace BardicTabConverter.Rendering;

public class TabMeasureRendererException : Exception
{
    public TabMeasureRendererException() {}
    public TabMeasureRendererException(string message) : base(message) {}
    public TabMeasureRendererException(string message, Exception innerException) : base(message, innerException) {}
}