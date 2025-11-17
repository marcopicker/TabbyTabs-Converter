using System;

namespace GW2MidiConverter.App.Services;

internal record VersionCheckInfo(
    bool IsNewVersionAvailable,
    Version LatestVersion,
    string ReleaseNotes,
    string DownloadUrl
);