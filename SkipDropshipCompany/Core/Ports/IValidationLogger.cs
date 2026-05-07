#nullable enable

using SkipDropshipCompany.Core.Validation;

namespace SkipDropshipCompany.Core.Ports;

internal interface IValidationLogger
{
    void Record(ValidationLogRecord record);
}
