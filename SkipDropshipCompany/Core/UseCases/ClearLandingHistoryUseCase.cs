// SPDX-License-Identifier: MIT
#nullable enable

using SkipDropshipCompany.Core.Ports;
using SkipDropshipCompany.Core.State;

namespace SkipDropshipCompany.Core.UseCases;

internal sealed class ClearLandingHistoryUseCase
{
    private readonly IGameInterop gameInterop;
    private readonly LandingHistoryStore landingHistoryStore;
    private readonly IPluginLogger logger;

    public ClearLandingHistoryUseCase(
        IGameInterop gameInterop,
        LandingHistoryStore landingHistoryStore,
        IPluginLogger logger
    )
    {
        this.gameInterop = gameInterop;
        this.landingHistoryStore = landingHistoryStore;
        this.logger = logger;
    }

    public void Execute()
    {
        if (!gameInterop.IsServer())
        {
            logger.LogDebug("Not the server. Skipping landing history clear.");
            return;
        }

        logger.LogDebug("Clearing landing history.");
        landingHistoryStore.ClearLandingHistory();
        logger.LogDebug("Cleared landing history.");
    }
}
