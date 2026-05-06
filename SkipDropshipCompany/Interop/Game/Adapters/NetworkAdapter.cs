// SPDX-License-Identifier: MIT
#nullable enable

using SkipDropshipCompany.Core.Ports;
using Unity.Netcode;

namespace SkipDropshipCompany.Interop.Game.Adapters;

internal sealed class NetworkAdapter
{
    private readonly IPluginLogger logger;

    public NetworkAdapter(IPluginLogger logger)
    {
        this.logger = logger;
    }

    public bool IsServer()
    {
        var networkManager = NetworkManager.Singleton;
        if (networkManager == null)
        {
            logger.LogError("NetworkManager.Singleton is null.");
            return false;
        }

        return networkManager.IsServer;
    }
}
