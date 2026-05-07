#nullable enable

namespace SkipDropshipCompany.Core.Ports;

internal interface IPluginConfig
{
    bool Enabled { get; }

    bool RequireReroutingOnFirstDay { get; }

    bool ValidationLogging { get; }
}
