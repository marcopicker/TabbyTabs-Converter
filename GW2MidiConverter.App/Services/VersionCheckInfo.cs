using System;

namespace GW2MidiConverter.App.Services;

internal record VersionCheckInfo(
    bool IsNewVersionAvailable,
    Version LatestVersion,
    string ReleaseNotes,
    string DownloadUrl
)
{
    public static readonly VersionCheckInfo NoNewVersionInfo = new VersionCheckInfo(false, new Version(), string.Empty, string.Empty);
};