using System.Threading.Tasks;

namespace TabbyTabs.Converter.App.Services;

internal interface IApplicationVersionChecker
{
    Task<VersionCheckInfo> IsNewVersionAvailableAsync();
}