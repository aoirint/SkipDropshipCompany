// SPDX-License-Identifier: MIT
#nullable enable

using SkipDropshipCompany.Core.Ports;
using SkipDropshipCompany.Core.State;

namespace SkipDropshipCompany.Interop.Game.Adapters;

internal sealed class RoundAdapter
{
    private readonly IPluginLogger logger;

    public RoundAdapter(IPluginLogger logger)
    {
        this.logger = logger;
    }

    public RoundState GetRoundState()
    {
        var startOfRound = StartOfRound.Instance;
        if (startOfRound == null)
        {
            logger.LogError("StartOfRound.Instance is null.");
            return new RoundState(isInOrbit: false, isFirstDay: false, isRoutingToCompany: false);
        }

        var isInOrbit = startOfRound.inShipPhase;
        var isFirstDay = IsFirstDay(startOfRound);
        var isRoutingToCompany = IsRoutingToCompany(startOfRound);

        return new RoundState(
            isInOrbit: isInOrbit,
            isFirstDay: isFirstDay,
            isRoutingToCompany: isRoutingToCompany
        );
    }

    private bool IsFirstDay(StartOfRound startOfRound)
    {
        var gameStats = startOfRound.gameStats;
        if (gameStats == null)
        {
            logger.LogError("StartOfRound.Instance.gameStats is null.");
            return false;
        }

        var daysSpent = gameStats.daysSpent;
        logger.LogDebug($"daysSpent={daysSpent}");

        return daysSpent == 0;
    }

    private bool IsRoutingToCompany(StartOfRound startOfRound)
    {
        var currentLevel = startOfRound.currentLevel;
        if (currentLevel == null)
        {
            logger.LogError("StartOfRound.Instance.currentLevel is null.");
            return false;
        }

        var sceneName = currentLevel.sceneName;
        logger.LogDebug($"IsSceneNameCompany? sceneName={sceneName}");
        return CompanyScene.IsCompanyScene(sceneName);
    }
}
