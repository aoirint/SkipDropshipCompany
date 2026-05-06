// SPDX-License-Identifier: MIT
#nullable enable

using SkipDropshipCompany.Core.Ports;
using SkipDropshipCompany.Core.State;

namespace SkipDropshipCompany.Core.UseCases;

internal sealed class RecordLandingUseCase
{
    private readonly IGameInterop gameInterop;
    private readonly LandingHistoryStore landingHistoryStore;
    private readonly IPluginLogger logger;

    public RecordLandingUseCase(
        IGameInterop gameInterop,
        LandingHistoryStore landingHistoryStore,
        IPluginLogger logger
    )
    {
        this.gameInterop = gameInterop;
        this.landingHistoryStore = landingHistoryStore;
        this.logger = logger;
    }

    public void Execute(string? sceneName)
    {
        if (!gameInterop.IsServer())
        {
            logger.LogDebug("Not the server. Skipping landing history addition.");
            return;
        }

        if (sceneName == null)
        {
            logger.LogError("StartOfRound.currentLevel.sceneName is null.");
            return;
        }

        logger.LogDebug($"Adding landing history. sceneName={sceneName}");
        if (!landingHistoryStore.AddLandingHistory(sceneName: sceneName))
        {
            logger.LogError($"Failed to add landing history. sceneName={sceneName}");
            return;
        }

        logger.LogDebug($"Added landing history. sceneName={sceneName}");
    }
}
