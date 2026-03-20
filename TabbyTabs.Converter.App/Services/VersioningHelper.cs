using System;
using System.Reflection;

namespace TabbyTabs.Converter.App.Services;

internal static class VersioningHelper
{
    public static Version? CurrentVersion => Assembly.GetExecutingAssembly().GetName().Version;
    public static string? CurrentVersionText => CurrentVersion?.ToString(3);
}