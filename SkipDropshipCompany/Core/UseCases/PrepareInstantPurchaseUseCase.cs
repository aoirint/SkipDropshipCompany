// SPDX-License-Identifier: MIT
#nullable enable

using System.Collections.Generic;
using SkipDropshipCompany.Core.Ports;
using SkipDropshipCompany.Core.State;
using SkipDropshipCompany.Core.Validation;

namespace SkipDropshipCompany.Core.UseCases;

internal sealed class PrepareInstantPurchaseUseCase
{
    private readonly IGameInterop gameInterop;
    private readonly InstantPurchaseEligibilityUseCase eligibilityUseCase;
    private readonly PreparedInstantPurchaseStore preparedInstantPurchaseStore;
    private readonly IPluginLogger logger;
    private readonly IValidationLogger validationLogger;

    public PrepareInstantPurchaseUseCase(
        IGameInterop gameInterop,
        InstantPurchaseEligibilityUseCase eligibilityUseCase,
        PreparedInstantPurchaseStore preparedInstantPurchaseStore,
        IPluginLogger logger,
        IValidationLogger validationLogger
    )
    {
        this.gameInterop = gameInterop;
        this.eligibilityUseCase = eligibilityUseCase;
        this.preparedInstantPurchaseStore = preparedInstantPurchaseStore;
        this.logger = logger;
        this.validationLogger = validationLogger;
    }

    public PrepareInstantPurchaseResult? Execute(List<int> boughtItemIndexes)
    {
        if (!gameInterop.IsServer())
        {
            logger.LogDebug("Not the server. Skipping instant purchase logic.");
            validationLogger.Record(
                ValidationLogRecord.PrepareInstantPurchaseResult(
                    role: ValidationLogRole.Client,
                    result: ValidationLogPrepareResult.NoServer,
                    originalItemCount: boughtItemIndexes.Count,
                    preparedResult: null
                )
            );

            return null;
        }

        logger.LogDebug("Preparing instant purchase.");

        if (!eligibilityUseCase.IsInstantPurchaseAllowed())
        {
            logger.LogDebug("Instant purchase is not allowed in the current game state.");
            validationLogger.Record(
                ValidationLogRecord.PrepareInstantPurchaseResult(
                    role: ValidationLogRole.Server,
                    result: ValidationLogPrepareResult.NotAllowed,
                    originalItemCount: boughtItemIndexes.Count,
                    preparedResult: null
                )
            );

            return null;
        }

        var result = new PrepareInstantPurchaseResult(
            dropShipBoughtItemIndexes: [],
            instantBoughtItemIndexes: boughtItemIndexes
        );

        validationLogger.Record(
            ValidationLogRecord.PrepareInstantPurchaseResult(
                role: ValidationLogRole.Server,
                result: ValidationLogPrepareResult.Success,
                originalItemCount: boughtItemIndexes.Count,
                preparedResult: result
            )
        );

        // Publish only after the success record is emitted. If validation
        // logging throws, the Harmony guard treats the Prefix as fail-open and
        // the Postfix must not see prepared work from that failed notification.
        preparedInstantPurchaseStore.SetPreparedInstantPurchaseResult(result);

        return result;
    }
}
