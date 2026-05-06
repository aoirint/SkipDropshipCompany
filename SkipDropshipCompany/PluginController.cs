// SPDX-License-Identifier: MIT
#nullable enable

using SkipDropshipCompany.Core.Handlers;
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
    private readonly RoundCallbackHandler roundCallbackHandler;
    private readonly TerminalSyncGroupCreditsHandler terminalSyncGroupCreditsHandler;

    private PluginController(
        RoundCallbackHandler roundCallbackHandler,
        TerminalSyncGroupCreditsHandler terminalSyncGroupCreditsHandler
    )
    {
        this.roundCallbackHandler = roundCallbackHandler;
        this.terminalSyncGroupCreditsHandler = terminalSyncGroupCreditsHandler;
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

        var recordLandingUseCase = new RecordLandingUseCase(
            gameInterop: gameInterop,
            landingHistoryStore: landingHistoryStore,
            logger: logger,
            validationLogger: validationLogger
        );
        var clearLandingHistoryUseCase = new ClearLandingHistoryUseCase(
            gameInterop: gameInterop,
            landingHistoryStore: landingHistoryStore,
            logger: logger,
            validationLogger: validationLogger
        );

        return new PluginController(
            roundCallbackHandler: new RoundCallbackHandler(
                gameInterop: gameInterop,
                recordLandingUseCase: recordLandingUseCase,
                clearLandingHistoryUseCase: clearLandingHistoryUseCase
            ),
            terminalSyncGroupCreditsHandler: new TerminalSyncGroupCreditsHandler(
                gameInterop: gameInterop,
                prepareInstantPurchaseUseCase: prepareInstantPurchaseUseCase,
                spawnPreparedInstantPurchasedItemsUseCase: spawnPreparedInstantPurchasedItemsUseCase,
                logger: logger
            )
        );
    }

    public void HandleStartGame()
    {
        roundCallbackHandler.HandleStartGame();
    }

    public void HandleResetShip()
    {
        roundCallbackHandler.HandleResetShip();
    }

    public PrepareInstantPurchaseResult? HandleTerminalSyncGroupCreditsClientRpcPrefix()
    {
        return terminalSyncGroupCreditsHandler.HandlePrefix();
    }

    public SpawnPreparedInstantPurchasedItemsResult? HandleTerminalSyncGroupCreditsClientRpcPostfix()
    {
        return terminalSyncGroupCreditsHandler.HandlePostfix();
    }
}
