using System;
using BardicTabConverter.Rendering.Settings;

namespace BardicTabConverter.Rendering;

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