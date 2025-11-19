using System.Threading.Tasks;

namespace GW2MidiConverter.App.Services;

internal interface IApplicationVersionChecker
{
    Task<VersionCheckInfo> IsNewVersionAvailableAsync();
}