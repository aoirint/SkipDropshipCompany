// SPDX-License-Identifier: MIT
#nullable enable

using SkipDropshipCompany.Core.Ports;
using SkipDropshipCompany.Core.UseCases;
using SkipDropshipCompany.Core.Validation;

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
    private readonly IValidationLogger validationLogger;

    public TerminalSyncGroupCreditsHandler(
        IGameInterop gameInterop,
        PrepareInstantPurchaseUseCase prepareInstantPurchaseUseCase,
        SpawnPreparedInstantPurchasedItemsUseCase spawnPreparedInstantPurchasedItemsUseCase,
        IPluginLogger logger,
        IValidationLogger validationLogger
    )
    {
        this.gameInterop = gameInterop;
        this.prepareInstantPurchaseUseCase = prepareInstantPurchaseUseCase;
        this.spawnPreparedInstantPurchasedItemsUseCase = spawnPreparedInstantPurchasedItemsUseCase;
        this.logger = logger;
        this.validationLogger = validationLogger;
    }

    public PrepareInstantPurchaseResult? HandlePrefix()
    {
        var boughtItemIndexes = gameInterop.GetTerminalOrderedItemIndexes();
        if (boughtItemIndexes == null)
        {
            logger.LogError("Terminal.orderedItemsFromTerminal is null.");
            validationLogger.Record(
                ValidationLogRecord.TerminalOrderReadResult(
                    role: GetRole(),
                    result: ValidationLogTerminalOrderReadResult.NullOrderedItems
                )
            );
            return null;
        }

        // Prefix preparation partitions the pending order before the base-game
        // RPC applies its dropship count. The Postfix later consumes the
        // prepared instant portion and restores the retained dropship order.
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

    public void HandlePostfix()
    {
        // Postfix work is intentionally driven only by prepared state from the
        // Prefix. If preparation failed or was not allowed, the base-game order
        // remains untouched.
        var result = spawnPreparedInstantPurchasedItemsUseCase.Execute();
        if (result == null)
        {
            logger.LogDebug("Spawning prepared instant purchased items failed or none to spawn.");
            return;
        }

        if (!gameInterop.SetTerminalOrderedItemIndexes(result.DropShipBoughtItemIndexes))
        {
            logger.LogError("Failed to restore Terminal.orderedItemsFromTerminal.");
            validationLogger.Record(
                ValidationLogRecord.TerminalOrderRestoreResult(
                    role: GetRole(),
                    result: ValidationLogTerminalOrderRestoreResult.Failed,
                    dropshipItemCount: result.DropShipBoughtItemIndexes.Count
                )
            );
            return;
        }

        validationLogger.Record(
            ValidationLogRecord.TerminalOrderRestoreResult(
                role: GetRole(),
                result: ValidationLogTerminalOrderRestoreResult.Success,
                dropshipItemCount: result.DropShipBoughtItemIndexes.Count
            )
        );
        logger.LogDebug("Spawned all prepared instant purchased items.");
    }

    private ValidationLogRole GetRole()
    {
        return gameInterop.IsServer() ? ValidationLogRole.Server : ValidationLogRole.Client;
    }
}
