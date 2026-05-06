// SPDX-License-Identifier: MIT
#nullable enable

using System.Collections.Generic;
using SkipDropshipCompany.Core.Ports;
using SkipDropshipCompany.Core.State;

namespace SkipDropshipCompany.Core.UseCases;

internal sealed class PrepareInstantPurchaseUseCase
{
    private readonly IGameInterop gameInterop;
    private readonly InstantPurchaseEligibilityUseCase eligibilityUseCase;
    private readonly PreparedInstantPurchaseStore preparedInstantPurchaseStore;
    private readonly IPluginLogger logger;

    public PrepareInstantPurchaseUseCase(
        IGameInterop gameInterop,
        InstantPurchaseEligibilityUseCase eligibilityUseCase,
        PreparedInstantPurchaseStore preparedInstantPurchaseStore,
        IPluginLogger logger
    )
    {
        this.gameInterop = gameInterop;
        this.eligibilityUseCase = eligibilityUseCase;
        this.preparedInstantPurchaseStore = preparedInstantPurchaseStore;
        this.logger = logger;
    }

    public PrepareInstantPurchaseResult? Execute(List<int> boughtItemIndexes)
    {
        if (!gameInterop.IsServer())
        {
            logger.LogDebug("Not the server. Skipping instant purchase logic.");
            return null;
        }

        logger.LogDebug("Preparing instant purchase.");
        if (!eligibilityUseCase.IsInstantPurchaseAllowed())
        {
            logger.LogDebug("Instant purchase is not allowed in the current game state.");
            return null;
        }

        var result = new PrepareInstantPurchaseResult(
            dropShipBoughtItemIndexes: [],
            instantBoughtItemIndexes: boughtItemIndexes
        );

        preparedInstantPurchaseStore.SetPreparedInstantPurchaseResult(result);
        return result;
    }
}
