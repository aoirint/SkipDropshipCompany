// SPDX-License-Identifier: MIT
#nullable enable

namespace SkipDropshipCompany.Core.Ports;

internal interface IPluginLogger
{
    void LogDebug(string message);

    void LogInfo(string message);

    void LogError(string message);
}
