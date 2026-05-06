// SPDX-License-Identifier: MIT
#nullable enable

using System.Collections.Generic;
using SkipDropshipCompany.Core.Ports;
using SkipDropshipCompany.Core.State;
using SkipDropshipCompany.Core.UseCases;
using SkipDropshipCompany.Core.Validation;
using SkipDropshipCompany.Interop.Game;

namespace SkipDropshipCompany;

// The controller is the plugin-facing facade. Harmony patches report game
// callbacks here, while Core use cases own the purchase and landing-history
// policy behind this boundary.
internal sealed class PluginController
{
    private readonly RecordLandingUseCase recordLandingUseCase;
    private readonly ClearLandingHistoryUseCase clearLandingHistoryUseCase;
    private readonly PrepareInstantPurchaseUseCase prepareInstantPurchaseUseCase;
    private readonly SpawnPreparedInstantPurchasedItemsUseCase spawnPreparedInstantPurchasedItemsUseCase;
    private readonly IPluginLogger logger;

    private PluginController(
        RecordLandingUseCase recordLandingUseCase,
        ClearLandingHistoryUseCase clearLandingHistoryUseCase,
        PrepareInstantPurchaseUseCase prepareInstantPurchaseUseCase,
        SpawnPreparedInstantPurchasedItemsUseCase spawnPreparedInstantPurchasedItemsUseCase,
        IPluginLogger logger
    )
    {
        this.recordLandingUseCase = recordLandingUseCase;
        this.clearLandingHistoryUseCase = clearLandingHistoryUseCase;
        this.prepareInstantPurchaseUseCase = prepareInstantPurchaseUseCase;
        this.spawnPreparedInstantPurchasedItemsUseCase = spawnPreparedInstantPurchasedItemsUseCase;
        this.logger = logger;
    }

    public static PluginController Create(
        IPluginConfig config,
        IPluginLogger logger,
        IValidationLogger validationLogger
    )
    {
        IGameInterop gameInterop = new GameInterop(logger);

        var landingHistoryStore = new LandingHistoryStore(logger);
        var preparedInstantPurchaseStore = new PreparedInstantPurchaseStore();
        var instantPurchaseEligibilityUseCase = new InstantPurchaseEligibilityUseCase(
            config: config,
            gameInterop: gameInterop,
            landingHistoryStore: landingHistoryStore,
            logger: logger,
            validationLogger: validationLogger
        );
        var prepareInstantPurchaseUseCase = new PrepareInstantPurchaseUseCase(
            gameInterop: gameInterop,
            eligibilityUseCase: instantPurchaseEligibilityUseCase,
            preparedInstantPurchaseStore: preparedInstantPurchaseStore,
            logger: logger,
            validationLogger: validationLogger
        );
        var spawnPreparedInstantPurchasedItemsUseCase =
            new SpawnPreparedInstantPurchasedItemsUseCase(
                gameInterop: gameInterop,
                preparedInstantPurchaseStore: preparedInstantPurchaseStore,
                logger: logger,
                validationLogger: validationLogger
            );

        validationLogger.Record(ValidationLogRecord.ControllerCreated());

        return new PluginController(
            recordLandingUseCase: new RecordLandingUseCase(
                gameInterop: gameInterop,
                landingHistoryStore: landingHistoryStore,
                logger: logger,
                validationLogger: validationLogger
            ),
            clearLandingHistoryUseCase: new ClearLandingHistoryUseCase(
                gameInterop: gameInterop,
                landingHistoryStore: landingHistoryStore,
                logger: logger,
                validationLogger: validationLogger
            ),
            prepareInstantPurchaseUseCase: prepareInstantPurchaseUseCase,
            spawnPreparedInstantPurchasedItemsUseCase: spawnPreparedInstantPurchasedItemsUseCase,
            logger: logger
        );
    }

    public void HandleStartGame(string? sceneName)
    {
        recordLandingUseCase.Execute(sceneName);
    }

    public void HandleResetShip()
    {
        clearLandingHistoryUseCase.Execute();
    }

    public PrepareInstantPurchaseResult? PrepareInstantPurchase(List<int>? boughtItemIndexes)
    {
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

    public SpawnPreparedInstantPurchasedItemsResult? SpawnPreparedInstantPurchasedItems()
    {
        var result = spawnPreparedInstantPurchasedItemsUseCase.Execute();
        if (result == null)
        {
            logger.LogDebug("Spawning prepared instant purchased items failed or none to spawn.");
            return null;
        }

        logger.LogDebug("Spawned all prepared instant purchased items.");
        return result;
    }
}
