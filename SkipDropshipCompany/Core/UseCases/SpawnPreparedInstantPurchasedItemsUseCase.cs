#nullable enable

using SkipDropshipCompany.Core.Ports;
using SkipDropshipCompany.Core.State;
using SkipDropshipCompany.Core.Validation;

namespace SkipDropshipCompany.Core.UseCases;

/// <summary>
/// Spawns the instant-delivery portion prepared by the terminal RPC Prefix.
/// </summary>
/// <remarks>
/// Returns the dropship-retained order so the handler can put
/// Terminal.orderedItemsFromTerminal back into the expected delivery state.
/// </remarks>
internal sealed class SpawnPreparedInstantPurchasedItemsUseCase
{
    private readonly IGameInterop gameInterop;
    private readonly PreparedInstantPurchaseStore preparedInstantPurchaseStore;
    private readonly IPluginLogger logger;
    private readonly IValidationLogger validationLogger;

    public SpawnPreparedInstantPurchasedItemsUseCase(
        IGameInterop gameInterop,
        PreparedInstantPurchaseStore preparedInstantPurchaseStore,
        IPluginLogger logger,
        IValidationLogger validationLogger
    )
    {
        this.gameInterop = gameInterop;
        this.preparedInstantPurchaseStore = preparedInstantPurchaseStore;
        this.logger = logger;
        this.validationLogger = validationLogger;
    }

    public SpawnPreparedInstantPurchasedItemsResult? Execute()
    {
        if (!gameInterop.IsServer())
        {
            logger.LogDebug("Not the server. Skipping instant purchase logic.");
            validationLogger.Record(
                ValidationLogRecord.SpawnInstantPurchaseResult(
                    role: ValidationLogRole.Client,
                    result: ValidationLogSpawnResult.NoServer,
                    preparedInstantItemCount: 0,
                    preparedDropShipItemCount: 0,
                    spawnedItemCount: 0
                )
            );
            return null;
        }

        logger.LogDebug("Spawning prepared instant purchased items.");

        var preparedInstantPurchaseResult =
            preparedInstantPurchaseStore.GetPreparedInstantPurchaseResult();
        if (preparedInstantPurchaseResult == null)
        {
            logger.LogDebug("No prepared instant purchase to spawn.");
            validationLogger.Record(
                ValidationLogRecord.SpawnInstantPurchaseResult(
                    role: ValidationLogRole.Server,
                    result: ValidationLogSpawnResult.NoPreparedPurchase,
                    preparedInstantItemCount: 0,
                    preparedDropShipItemCount: 0,
                    spawnedItemCount: 0
                )
            );
            return null;
        }

        var spawnedItemCount = 0;
        foreach (var buyableItemIndex in preparedInstantPurchaseResult.InstantBoughtItemIndexes)
        {
            if (!gameInterop.SpawnBuyableItemInShip(buyableItemIndex))
            {
                logger.LogError(
                    $"Failed to spawn instant purchased item. buyableItemIndex={buyableItemIndex}"
                );
                validationLogger.Record(
                    ValidationLogRecord.SpawnInstantPurchaseResult(
                        role: ValidationLogRole.Server,
                        result: ValidationLogSpawnResult.SpawnFailed,
                        preparedInstantItemCount:
                            preparedInstantPurchaseResult.InstantBoughtItemIndexes.Count,
                        preparedDropShipItemCount:
                            preparedInstantPurchaseResult.DropShipBoughtItemIndexes.Count,
                        spawnedItemCount: spawnedItemCount
                    )
                );
                return null;
            }

            spawnedItemCount++;
        }

        var result = new SpawnPreparedInstantPurchasedItemsResult(
            dropShipBoughtItemIndexes: preparedInstantPurchaseResult.DropShipBoughtItemIndexes,
            instantBoughtItemIndexes: preparedInstantPurchaseResult.InstantBoughtItemIndexes
        );

        // Clear before reporting success so a later Postfix cannot replay the
        // same prepared order if terminal restoration or logging is retried.
        preparedInstantPurchaseStore.ClearPreparedInstantPurchaseResult();
        validationLogger.Record(
            ValidationLogRecord.SpawnInstantPurchaseResult(
                role: ValidationLogRole.Server,
                result: ValidationLogSpawnResult.Success,
                preparedInstantItemCount: preparedInstantPurchaseResult.InstantBoughtItemIndexes.Count,
                preparedDropShipItemCount: preparedInstantPurchaseResult.DropShipBoughtItemIndexes.Count,
                spawnedItemCount: spawnedItemCount
            )
        );
        return result;
    }
}
