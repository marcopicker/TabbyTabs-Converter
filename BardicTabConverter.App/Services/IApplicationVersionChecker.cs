using System.Threading.Tasks;

namespace BardicTabConverter.App.Services;

internal interface IApplicationVersionChecker
{
    Task<VersionCheckInfo> IsNewVersionAvailableAsync();
}