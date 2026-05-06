// SPDX-License-Identifier: MIT
#nullable enable

using System;
using SkipDropshipCompany.Core.Ports;
using SkipDropshipCompany.Core.Validation;

namespace SkipDropshipCompany.Interop;

internal sealed class CallbackDiagnosticReporter
{
    private readonly IPluginLogger logger;
    private readonly IValidationLogger validationLogger;

    public CallbackDiagnosticReporter(IPluginLogger logger, IValidationLogger validationLogger)
    {
        this.logger = logger;
        this.validationLogger = validationLogger;
    }

    public void RecordException(string callback, Exception exception)
    {
        var exceptionType = exception.GetType().FullName ?? exception.GetType().Name;
        logger.LogError(
            "SkipDropshipCompany callback exception." +
            $" callback={callback}" +
            $" exception_type={exceptionType}"
        );
        validationLogger.Record(
            ValidationLogRecord.CallbackException(
                callback: callback,
                exceptionType: exceptionType
            )
        );
    }
}
