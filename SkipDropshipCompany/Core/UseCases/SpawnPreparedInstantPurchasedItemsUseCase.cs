// SPDX-License-Identifier: MIT
#nullable enable

using SkipDropshipCompany.Core.Ports;
using SkipDropshipCompany.Core.State;

namespace SkipDropshipCompany.Core.UseCases;

internal sealed class SpawnPreparedInstantPurchasedItemsUseCase
{
    private readonly IGameInterop gameInterop;
    private readonly PreparedInstantPurchaseStore preparedInstantPurchaseStore;
    private readonly IPluginLogger logger;

    public SpawnPreparedInstantPurchasedItemsUseCase(
        IGameInterop gameInterop,
        PreparedInstantPurchaseStore preparedInstantPurchaseStore,
        IPluginLogger logger
    )
    {
        this.gameInterop = gameInterop;
        this.preparedInstantPurchaseStore = preparedInstantPurchaseStore;
        this.logger = logger;
    }

    public SpawnPreparedInstantPurchasedItemsResult? Execute()
    {
        if (!gameInterop.IsServer())
        {
            logger.LogDebug("Not the server. Skipping instant purchase logic.");
            return null;
        }

        logger.LogDebug("Spawning prepared instant purchased items.");

        var preparedInstantPurchaseResult =
            preparedInstantPurchaseStore.GetPreparedInstantPurchaseResult();
        if (preparedInstantPurchaseResult == null)
        {
            logger.LogDebug("No prepared instant purchase to spawn.");
            return null;
        }

        foreach (var buyableItemIndex in preparedInstantPurchaseResult.InstantBoughtItemIndexes)
        {
            if (!gameInterop.SpawnBuyableItemInShip(buyableItemIndex))
            {
                logger.LogError(
                    $"Failed to spawn instant purchased item. buyableItemIndex={buyableItemIndex}"
                );
                return null;
            }
        }

        var result = new SpawnPreparedInstantPurchasedItemsResult(
            dropShipBoughtItemIndexes: preparedInstantPurchaseResult.DropShipBoughtItemIndexes,
            instantBoughtItemIndexes: preparedInstantPurchaseResult.InstantBoughtItemIndexes
        );

        preparedInstantPurchaseStore.ClearPreparedInstantPurchaseResult();
        return result;
    }
}
