#nullable enable

using SkipDropshipCompany.Core.Ports;
using SkipDropshipCompany.Core.State;
using SkipDropshipCompany.Core.Validation;

namespace SkipDropshipCompany.Core.UseCases;

internal sealed class ClearLandingHistoryUseCase
{
    private readonly IGameInterop gameInterop;
    private readonly LandingHistoryStore landingHistoryStore;
    private readonly IPluginLogger logger;
    private readonly IValidationLogger validationLogger;

    public ClearLandingHistoryUseCase(
        IGameInterop gameInterop,
        LandingHistoryStore landingHistoryStore,
        IPluginLogger logger,
        IValidationLogger validationLogger
    )
    {
        this.gameInterop = gameInterop;
        this.landingHistoryStore = landingHistoryStore;
        this.logger = logger;
        this.validationLogger = validationLogger;
    }

    public void Execute()
    {
        if (!gameInterop.IsServer())
        {
            logger.LogDebug("Not the server. Skipping landing history clear.");
            validationLogger.Record(
                ValidationLogRecord.LandingHistoryCleared(
                    role: ValidationLogRole.Client,
                    cleared: false
                )
            );
            return;
        }

        logger.LogDebug("Clearing landing history.");
        landingHistoryStore.ClearLandingHistory();
        logger.LogDebug("Cleared landing history.");
        validationLogger.Record(
            ValidationLogRecord.LandingHistoryCleared(
                role: ValidationLogRole.Server,
                cleared: true
            )
        );
    }
}
