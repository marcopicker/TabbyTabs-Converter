using System;
using System.Reflection;

namespace BardicTabConverter.App.Services;

internal static class VersioningHelper
{
    public static Version? CurrentVersion => Assembly.GetExecutingAssembly().GetName().Version;
    public static string? CurrentVersionText => CurrentVersion?.ToString(3);
}