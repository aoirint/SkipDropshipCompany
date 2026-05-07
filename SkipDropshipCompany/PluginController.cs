#nullable enable

using SkipDropshipCompany.Core.Handlers;
using SkipDropshipCompany.Core.Ports;
using SkipDropshipCompany.Core.State;
using SkipDropshipCompany.Core.UseCases;
using SkipDropshipCompany.Core.Validation;
using SkipDropshipCompany.Interop.Game;

namespace SkipDropshipCompany;

/// <summary>
/// Plugin-facing facade for game callbacks reported by Harmony patches.
/// </summary>
/// <remarks>
/// Core use cases own the purchase and landing-history policy behind this boundary.
/// </remarks>
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

    /// <summary>
    /// Builds the plugin controller and manually wires concrete integrations to
    /// Core ports.
    /// </summary>
    public static PluginController Create(
        IPluginConfig config,
        IPluginLogger logger,
        IValidationLogger validationLogger
    )
    {
        IGameInterop gameInterop = new GameInterop(logger);

        // Manual wiring is grouped by game-state lifetime: landing history,
        // prepared terminal purchase state, then the handlers that consume both.
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
                logger: logger,
                validationLogger: validationLogger
            )
        );
    }

    /// <summary>
    /// Handles the base-game round-start callback.
    /// </summary>
    public void HandleStartGame()
    {
        roundCallbackHandler.HandleStartGame();
    }

    /// <summary>
    /// Handles the base-game ship-reset callback.
    /// </summary>
    public void HandleResetShip()
    {
        roundCallbackHandler.HandleResetShip();
    }

    /// <summary>
    /// Handles the Prefix side of Terminal.SyncGroupCreditsClientRpc.
    /// </summary>
    public PrepareInstantPurchaseResult? HandleTerminalSyncGroupCreditsClientRpcPrefix()
    {
        return terminalSyncGroupCreditsHandler.HandlePrefix();
    }

    /// <summary>
    /// Handles the Postfix side of Terminal.SyncGroupCreditsClientRpc.
    /// </summary>
    public void HandleTerminalSyncGroupCreditsClientRpcPostfix()
    {
        terminalSyncGroupCreditsHandler.HandlePostfix();
    }
}
