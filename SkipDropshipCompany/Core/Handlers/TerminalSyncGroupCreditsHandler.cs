// SPDX-License-Identifier: MIT
#nullable enable

using SkipDropshipCompany.Core.Ports;
using SkipDropshipCompany.Core.UseCases;

namespace SkipDropshipCompany.Core.Handlers;

// Terminal SyncGroupCreditsClientRpc handling is callback coordination. The
// Harmony patch owns method-shape adaptation; this handler owns game-state reads
// and use-case dispatch.
internal sealed class TerminalSyncGroupCreditsHandler
{
    private readonly IGameInterop gameInterop;
    private readonly PrepareInstantPurchaseUseCase prepareInstantPurchaseUseCase;
    private readonly SpawnPreparedInstantPurchasedItemsUseCase spawnPreparedInstantPurchasedItemsUseCase;
    private readonly IPluginLogger logger;

    public TerminalSyncGroupCreditsHandler(
        IGameInterop gameInterop,
        PrepareInstantPurchaseUseCase prepareInstantPurchaseUseCase,
        SpawnPreparedInstantPurchasedItemsUseCase spawnPreparedInstantPurchasedItemsUseCase,
        IPluginLogger logger
    )
    {
        this.gameInterop = gameInterop;
        this.prepareInstantPurchaseUseCase = prepareInstantPurchaseUseCase;
        this.spawnPreparedInstantPurchasedItemsUseCase = spawnPreparedInstantPurchasedItemsUseCase;
        this.logger = logger;
    }

    public PrepareInstantPurchaseResult? PrepareInstantPurchase()
    {
        var boughtItemIndexes = gameInterop.GetTerminalOrderedItemIndexes();
        if (boughtItemIndexes == null)
        {
            logger.LogError("Terminal.orderedItemsFromTerminal is null.");
            return null;
        }

        var result = prepareInstantPurchaseUseCase.Execute(boughtItemIndexes: boughtItemIndexes);
        if (result == null)
        {
            logger.LogDebug("Prepare instant purchase failed or not allowed. Skipping instant purchase logic.");
            return null;
        }

        logger.LogDebug(
            "Prepared instant purchase." +
            $" originalDropShipItemCount={boughtItemIndexes.Count}" +
            $" preparedDropShipItemCount={result.DropShipBoughtItemIndexes.Count}" +
            $" preparedInstantPurchaseItemCount={result.InstantBoughtItemIndexes.Count}"
        );

        return result;
    }

    public SpawnPreparedInstantPurchasedItemsResult? SpawnPreparedItemsAndRestoreDropshipOrder()
    {
        var result = spawnPreparedInstantPurchasedItemsUseCase.Execute();
        if (result == null)
        {
            logger.LogDebug("Spawning prepared instant purchased items failed or none to spawn.");
            return null;
        }

        if (!gameInterop.SetTerminalOrderedItemIndexes(result.DropShipBoughtItemIndexes))
        {
            logger.LogError("Failed to restore Terminal.orderedItemsFromTerminal.");
            return null;
        }

        logger.LogDebug("Spawned all prepared instant purchased items.");
        return result;
    }
}
