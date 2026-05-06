// SPDX-License-Identifier: MIT
#nullable enable

using BepInEx.Logging;
using SkipDropshipCompany.Core.Ports;

namespace SkipDropshipCompany.Interop;

/// <summary>
/// Routes plugin log messages through BepInEx at the edge of the plugin.
/// </summary>
internal sealed class BepInExPluginLogger : IPluginLogger
{
    private readonly ManualLogSource logger;

    public BepInExPluginLogger(ManualLogSource logger)
    {
        this.logger = logger;
    }

    public void LogDebug(string message)
    {
        logger.LogDebug(message);
    }

    public void LogInfo(string message)
    {
        logger.LogInfo(message);
    }

    public void LogError(string message)
    {
        logger.LogError(message);
    }
}
