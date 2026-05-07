#nullable enable

namespace SkipDropshipCompany.Core.Ports;

/// <summary>
/// Logger port used by Core use cases and game interop adapters.
/// </summary>
internal interface IPluginLogger
{
    void LogDebug(string message);

    void LogInfo(string message);

    void LogError(string message);
}
