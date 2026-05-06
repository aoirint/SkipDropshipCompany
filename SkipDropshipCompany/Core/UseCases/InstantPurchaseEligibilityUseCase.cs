// SPDX-License-Identifier: MIT
#nullable enable

using SkipDropshipCompany.Core.Ports;
using SkipDropshipCompany.Core.State;

namespace SkipDropshipCompany.Core.UseCases;

internal sealed class InstantPurchaseEligibilityUseCase
{
    private readonly IPluginConfig config;
    private readonly IGameInterop gameInterop;
    private readonly LandingHistoryStore landingHistoryStore;
    private readonly IPluginLogger logger;

    public InstantPurchaseEligibilityUseCase(
        IPluginConfig config,
        IGameInterop gameInterop,
        LandingHistoryStore landingHistoryStore,
        IPluginLogger logger
    )
    {
        this.config = config;
        this.gameInterop = gameInterop;
        this.landingHistoryStore = landingHistoryStore;
        this.logger = logger;
    }

    public bool IsInstantPurchaseAllowed()
    {
        logger.LogDebug("Checking if instant purchase is allowed.");

        if (!config.Enabled)
        {
            logger.LogDebug("Not enabled.");
            return false;
        }

        var isFirstDayRerouteRequired = config.RequireReroutingOnFirstDay;
        logger.LogDebug($"Configs: isFirstDayRerouteRequired={isFirstDayRerouteRequired}");

        var roundState = gameInterop.GetRoundState();
        var isLandedOnCompany = IsLandedOnCompany(roundState);
        var isInFirstDayOrbit = IsInFirstDayOrbit(roundState);
        var isInFirstDayOrbitAndRoutingToCompany = IsInFirstDayOrbitAndRoutingToCompany(roundState);
        var isInOrbitAndLastLandedOnCompanyAndRoutingToCompany =
            IsInOrbitAndLastLandedOnCompanyAndRoutingToCompany(roundState);

        logger.LogDebug(
            "Flags:" +
            $" IsLandedOnCompany={isLandedOnCompany}" +
            $" IsInFirstDayOrbit={isInFirstDayOrbit}" +
            $" IsInFirstDayOrbitAndRoutingToCompany={isInFirstDayOrbitAndRoutingToCompany}" +
            $" isInOrbitAndLastLandedOnCompanyAndRoutingToCompany={isInOrbitAndLastLandedOnCompanyAndRoutingToCompany}"
        );

        return (
            isLandedOnCompany ||
            (!isFirstDayRerouteRequired && isInFirstDayOrbit) ||
            isInFirstDayOrbitAndRoutingToCompany ||
            isInOrbitAndLastLandedOnCompanyAndRoutingToCompany
        );
    }

    private bool IsInFirstDayOrbit(RoundState roundState)
    {
        if (!roundState.IsInOrbit)
        {
            logger.LogDebug("Not in orbit.");
            return false;
        }

        if (!roundState.IsFirstDay)
        {
            logger.LogDebug("Not first day.");
            return false;
        }

        return true;
    }

    private bool IsInFirstDayOrbitAndRoutingToCompany(RoundState roundState)
    {
        if (!IsInFirstDayOrbit(roundState))
        {
            logger.LogDebug("Not in first day orbit.");
            return false;
        }

        if (!roundState.IsRoutingToCompany)
        {
            logger.LogDebug("Not routing to company.");
            return false;
        }

        return true;
    }

    private bool IsLandedOnCompany(RoundState roundState)
    {
        if (roundState.IsInOrbit)
        {
            logger.LogDebug("In orbit.");
            return false;
        }

        if (!roundState.IsRoutingToCompany)
        {
            logger.LogDebug("Not routing to company.");
            return false;
        }

        return true;
    }

    private bool IsInOrbitAndLastLandedOnCompanyAndRoutingToCompany(RoundState roundState)
    {
        if (!roundState.IsInOrbit)
        {
            logger.LogDebug("Not in orbit.");
            return false;
        }

        if (!landingHistoryStore.IsLastLandedOnCompany())
        {
            logger.LogDebug("Last landed level is not company.");
            return false;
        }

        if (!roundState.IsRoutingToCompany)
        {
            logger.LogDebug("Not routing to company.");
            return false;
        }

        return true;
    }
}
