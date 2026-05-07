#nullable enable

using SkipDropshipCompany.Core.Ports;
using SkipDropshipCompany.Core.State;
using SkipDropshipCompany.Core.Validation;

namespace SkipDropshipCompany.Core.UseCases;

internal sealed class RecordLandingUseCase
{
    private readonly IGameInterop gameInterop;
    private readonly LandingHistoryStore landingHistoryStore;
    private readonly IPluginLogger logger;
    private readonly IValidationLogger validationLogger;

    public RecordLandingUseCase(
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

    public void Execute(string? sceneName)
    {
        if (!gameInterop.IsServer())
        {
            logger.LogDebug("Not the server. Skipping landing history addition.");
            validationLogger.Record(
                ValidationLogRecord.LandingHistoryUpdated(
                    role: ValidationLogRole.Client,
                    result: ValidationLogLandingHistoryResult.NoServer,
                    scene: ValidationLogRecord.ToValidationScene(sceneName)
                )
            );
            return;
        }

        if (sceneName == null)
        {
            logger.LogError("StartOfRound.currentLevel.sceneName is null.");
            validationLogger.Record(
                ValidationLogRecord.LandingHistoryUpdated(
                    role: ValidationLogRole.Server,
                    result: ValidationLogLandingHistoryResult.NullScene,
                    scene: ValidationLogScene.Unknown
                )
            );
            return;
        }

        logger.LogDebug($"Adding landing history. sceneName={sceneName}");
        if (!landingHistoryStore.AddLandingHistory(sceneName: sceneName))
        {
            logger.LogError($"Failed to add landing history. sceneName={sceneName}");
            validationLogger.Record(
                ValidationLogRecord.LandingHistoryUpdated(
                    role: ValidationLogRole.Server,
                    result: ValidationLogLandingHistoryResult.EmptyScene,
                    scene: ValidationLogRecord.ToValidationScene(sceneName)
                )
            );
            return;
        }

        logger.LogDebug($"Added landing history. sceneName={sceneName}");
        validationLogger.Record(
            ValidationLogRecord.LandingHistoryUpdated(
                role: ValidationLogRole.Server,
                result: ValidationLogLandingHistoryResult.Success,
                scene: ValidationLogRecord.ToValidationScene(sceneName)
            )
        );
    }
}
