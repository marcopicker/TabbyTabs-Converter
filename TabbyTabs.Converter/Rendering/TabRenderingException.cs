using System;
using TabbyTabs.Converter.Rendering.Settings;

namespace TabbyTabs.Converter.Rendering;

public class TabRenderingException : Exception
{
    public TabRenderingSettings? Settings { get; init; }
    
    public TabRenderingException()
    {
    }
    
    public TabRenderingException(string message) : base(message)
    {
    }
    
    public TabRenderingException(string message, Exception innerException) : base(message, innerException)
    {
    }
}