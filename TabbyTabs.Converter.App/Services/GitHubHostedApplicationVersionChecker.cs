using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace TabbyTabs.Converter.App.Services;

internal class GitHubHostedApplicationVersionChecker : IApplicationVersionChecker
{
    private const string LATEST_RELEASE_URI = "https://api.github.com/repos/marcopicker/tabbytabs-converter/releases/latest";

    private readonly ILogger<GitHubHostedApplicationVersionChecker> _logger;
    private readonly HttpClient _client = new HttpClient();
    private readonly Version? _currentVersion;

    public GitHubHostedApplicationVersionChecker(ILogger<GitHubHostedApplicationVersionChecker> logger)
    {
        _logger = logger;
        _client.DefaultRequestHeaders.Add("User-Agent", "TabbyTabs.Converter");
        _client.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");
        _client.Timeout = TimeSpan.FromSeconds(30);
        
        _currentVersion = VersioningHelper.CurrentVersion;
    }

    public async Task<VersionCheckInfo> IsNewVersionAvailableAsync()
    {
        using IDisposable? _ = _logger.BeginScope(new { Operation = "Checking for new version" });
        try
        {
            string response = await _client.GetStringAsync(LATEST_RELEASE_URI);
            JsonElement json = JsonSerializer.Deserialize<JsonElement>(response);

            string? latestVersion = json.GetProperty("tag_name").GetString();
            if (string.IsNullOrWhiteSpace(latestVersion))
            {
                _logger.LogError("Failed to get latest version from GitHub: Property 'tag_name' missing.");
                return VersionCheckInfo.NoNewVersionInfo;
            }

            string versionString = latestVersion.TrimStart('v');
            Version version = new Version(versionString);
            if (version <= _currentVersion)
            {
                _logger.LogInformation("Latest version ({Version}) is the same or lower than the current version ({CurrentVersion}).", latestVersion, _currentVersion?.ToString() ?? "unknown");
                return VersionCheckInfo.NoNewVersionInfo;
            }

            string? releaseUrl = json.GetProperty("html_url").GetString();
            if (string.IsNullOrWhiteSpace(releaseUrl))
            {
                _logger.LogError("Failed to get latest version from GitHub: Property 'html_url' missing.");
                return VersionCheckInfo.NoNewVersionInfo;
            }

            string? notes = json.GetProperty("body").GetString();
            if (string.IsNullOrWhiteSpace(notes))
            {
                _logger.LogError("Failed to get latest version from GitHub: Property 'body' missing.");
                return VersionCheckInfo.NoNewVersionInfo;
            }

            _logger.LogInformation("Found new version: {Version}", latestVersion);
            return new VersionCheckInfo(true, version, notes, releaseUrl);
        }
        catch (HttpRequestException e)
        {
            _logger.LogError(e, "Failed to check for new version.");
            return VersionCheckInfo.NoNewVersionInfo;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to check for new version.");
            return VersionCheckInfo.NoNewVersionInfo;
        }
    }
}