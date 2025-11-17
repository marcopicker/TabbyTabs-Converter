using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Text.Json;

namespace GW2MidiConverter.App.Services;

internal class GitHubHostedApplicationVersionChecker : IApplicationVersionChecker
{
    private const string LATEST_RELEASE_URI = "https://api.github.com/repos/Pocram93/GW2-Midi-Converter/releases/latest";
    private static readonly VersionCheckInfo NoNewVersionInfo = new VersionCheckInfo(false, null, null, null);

    private readonly HttpClient _client = new HttpClient();
    private readonly Version? _currentVersion;

    public GitHubHostedApplicationVersionChecker()
    {
        _client.DefaultRequestHeaders.Add("User-Agent", "GW2MidiConverter");
        _client.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");
        _client.Timeout = TimeSpan.FromSeconds(30);
        
        string? token = Environment.GetEnvironmentVariable("GITHUB_TOKEN");
        if (!string.IsNullOrWhiteSpace(token))
        {
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }
        
        _currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
    }

    public async Task<VersionCheckInfo> IsNewVersionAvailableAsync()
    {
        try
        {
            string response = await _client.GetStringAsync(LATEST_RELEASE_URI);

            JsonElement json = JsonSerializer.Deserialize<JsonElement>(response);

            string? latestVersion = json.GetProperty("tag_name").GetString();
            if (string.IsNullOrWhiteSpace(latestVersion)) return NoNewVersionInfo;
            
            string versionString = latestVersion.TrimStart('v');
            Version version = new Version(versionString);
            if (version <= _currentVersion) return NoNewVersionInfo;

            string? releaseUrl = json.GetProperty("html_url").GetString();
            if (string.IsNullOrWhiteSpace(releaseUrl)) return NoNewVersionInfo;

            string? notes = json.GetProperty("body").GetString();
            if (string.IsNullOrWhiteSpace(notes)) return NoNewVersionInfo;

            return new VersionCheckInfo(true, version, notes, releaseUrl);
        }
        catch (HttpRequestException e)
        {
            return NoNewVersionInfo;
        }
        catch (Exception e)
        {
            return NoNewVersionInfo;
        }
    }
}