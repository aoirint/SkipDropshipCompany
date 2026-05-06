// SPDX-License-Identifier: MIT
#nullable enable

using System;
using SkipDropshipCompany.Core.Ports;
using SkipDropshipCompany.Core.Validation;

namespace SkipDropshipCompany.Interop.Game.Patches;

internal sealed class HarmonyCallbackDiagnosticReporter
{
    private readonly IPluginLogger logger;
    private readonly IValidationLogger validationLogger;

    public HarmonyCallbackDiagnosticReporter(
        IPluginLogger logger,
        IValidationLogger validationLogger
    )
    {
        this.logger = logger;
        this.validationLogger = validationLogger;
    }

    public void RecordCallbackException(string callback, Exception exception)
    {
        var exceptionType = exception.GetType().FullName ?? exception.GetType().Name;
        logger.LogError(
            $"Harmony callback exception: callback={callback}, exception_type={exceptionType}"
        );
        validationLogger.Record(
            ValidationLogRecord.CallbackException(
                callback: callback,
                exceptionType: exceptionType
            )
        );
    }
}
