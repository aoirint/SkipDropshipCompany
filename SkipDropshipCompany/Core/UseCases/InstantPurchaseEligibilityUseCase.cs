// SPDX-License-Identifier: MIT
#nullable enable

using SkipDropshipCompany.Core.Ports;
using SkipDropshipCompany.Core.State;
using SkipDropshipCompany.Core.Validation;

namespace SkipDropshipCompany.Core.UseCases;

// Eligibility is a pure policy layer around current round state, config, and
// one-entry landing history. It records both the boolean decision and the reason
// token used for validation.
internal sealed class InstantPurchaseEligibilityUseCase
{
    private readonly IPluginConfig config;
    private readonly IGameInterop gameInterop;
    private readonly LandingHistoryStore landingHistoryStore;
    private readonly IPluginLogger logger;
    private readonly IValidationLogger validationLogger;

    public InstantPurchaseEligibilityUseCase(
        IPluginConfig config,
        IGameInterop gameInterop,
        LandingHistoryStore landingHistoryStore,
        IPluginLogger logger,
        IValidationLogger validationLogger
    )
    {
        this.config = config;
        this.gameInterop = gameInterop;
        this.landingHistoryStore = landingHistoryStore;
        this.logger = logger;
        this.validationLogger = validationLogger;
    }

    public bool IsInstantPurchaseAllowed()
    {
        logger.LogDebug("Checking if instant purchase is allowed.");

        if (!config.Enabled)
        {
            logger.LogDebug("Not enabled.");
            validationLogger.Record(ValidationLogRecord.InstantPurchaseEligibilityConfigDisabled());
            return false;
        }

        var isFirstDayRerouteRequired = config.RequireReroutingOnFirstDay;
        logger.LogDebug($"Configs: isFirstDayRerouteRequired={isFirstDayRerouteRequired}");

        var roundState = gameInterop.GetRoundState();
        var isLandedOnCompany = IsLandedOnCompany(roundState);
        var isInFirstDayOrbit = IsInFirstDayOrbit(roundState);
        var isInFirstDayOrbitAndRoutingToCompany = IsInFirstDayOrbitAndRoutingToCompany(roundState);
        var lastLandedOnCompany = roundState.IsInOrbit && landingHistoryStore.IsLastLandedOnCompany();
        var isInOrbitAndLastLandedOnCompanyAndRoutingToCompany =
            IsInOrbitAndLastLandedOnCompanyAndRoutingToCompany(
                roundState,
                lastLandedOnCompany
            );

        logger.LogDebug(
            "Flags:" +
            $" IsLandedOnCompany={isLandedOnCompany}" +
            $" IsInFirstDayOrbit={isInFirstDayOrbit}" +
            $" IsInFirstDayOrbitAndRoutingToCompany={isInFirstDayOrbitAndRoutingToCompany}" +
            $" isInOrbitAndLastLandedOnCompanyAndRoutingToCompany={isInOrbitAndLastLandedOnCompanyAndRoutingToCompany}"
        );

        var allowed = (
            isLandedOnCompany ||
            (!isFirstDayRerouteRequired && isInFirstDayOrbit) ||
            isInFirstDayOrbitAndRoutingToCompany ||
            isInOrbitAndLastLandedOnCompanyAndRoutingToCompany
        );

        // Compute the reason from the same booleans used for the decision so
        // validation logs explain the selected policy path without re-reading
        // mutable game state.
        validationLogger.Record(
            ValidationLogRecord.InstantPurchaseEligibilityDecision(
                roundState: roundState,
                enabled: true,
                requireReroutingOnFirstDay: isFirstDayRerouteRequired,
                lastLandedOnCompany: lastLandedOnCompany,
                allowed: allowed,
                reason: GetEligibilityReason(
                    isLandedOnCompany: isLandedOnCompany,
                    isFirstDayRerouteRequired: isFirstDayRerouteRequired,
                    isInFirstDayOrbit: isInFirstDayOrbit,
                    isInFirstDayOrbitAndRoutingToCompany: isInFirstDayOrbitAndRoutingToCompany,
                    isInOrbitAndLastLandedOnCompanyAndRoutingToCompany:
                        isInOrbitAndLastLandedOnCompanyAndRoutingToCompany
                )
            )
        );

        return allowed;
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

    private bool IsInOrbitAndLastLandedOnCompanyAndRoutingToCompany(
        RoundState roundState,
        bool lastLandedOnCompany
    )
    {
        if (!roundState.IsInOrbit)
        {
            logger.LogDebug("Not in orbit.");
            return false;
        }

        if (!lastLandedOnCompany)
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

    private static string GetEligibilityReason(
        bool isLandedOnCompany,
        bool isFirstDayRerouteRequired,
        bool isInFirstDayOrbit,
        bool isInFirstDayOrbitAndRoutingToCompany,
        bool isInOrbitAndLastLandedOnCompanyAndRoutingToCompany
    )
    {
        if (isLandedOnCompany)
        {
            return "landed_on_company";
        }

        if (!isFirstDayRerouteRequired && isInFirstDayOrbit)
        {
            return "first_day_orbit";
        }

        if (isInFirstDayOrbitAndRoutingToCompany)
        {
            return "first_day_orbit_routing_to_company";
        }

        if (isInOrbitAndLastLandedOnCompanyAndRoutingToCompany)
        {
            return "orbit_after_company_landing";
        }

        return "conditions_not_met";
    }
}
