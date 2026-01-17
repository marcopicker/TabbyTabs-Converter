using System;
using System.IO;

namespace BardicTabConverter.App.Services;

internal static class Logging
{
    public static readonly string LogFolder = Path.Combine(Environment.CurrentDirectory, "Logs");
}