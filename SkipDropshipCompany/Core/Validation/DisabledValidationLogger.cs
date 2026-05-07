#nullable enable

using SkipDropshipCompany.Core.Ports;

namespace SkipDropshipCompany.Core.Validation;

internal sealed class DisabledValidationLogger : IValidationLogger
{
    public static DisabledValidationLogger Instance { get; } = new();

    private DisabledValidationLogger()
    {
    }

    public void Record(ValidationLogRecord record)
    {
    }
}
