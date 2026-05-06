// SPDX-License-Identifier: MIT
#nullable enable

using BepInEx.Configuration;
using SkipDropshipCompany.Core.Ports;

namespace SkipDropshipCompany.Interop;

internal sealed class BepInExPluginConfig : IPluginConfig
{
    private readonly ConfigEntry<bool> enabledConfig;
    private readonly ConfigEntry<bool> requireReroutingOnFirstDayConfig;

    private BepInExPluginConfig(
        ConfigEntry<bool> enabledConfig,
        ConfigEntry<bool> requireReroutingOnFirstDayConfig
    )
    {
        this.enabledConfig = enabledConfig;
        this.requireReroutingOnFirstDayConfig = requireReroutingOnFirstDayConfig;
    }

    public bool Enabled => enabledConfig.Value;

    public bool RequireReroutingOnFirstDay => requireReroutingOnFirstDayConfig.Value;

    public static BepInExPluginConfig Bind(ConfigFile config)
    {
        var enabledConfig = config.Bind(
            "General",
            "Enabled",
            true,
            "Set to false to disable this mod."
        );

        var requireReroutingOnFirstDayConfig = config.Bind(
            "General",
            "RequireReroutingOnFirstDay",
            false,
            "If true, rerouting to the company will be required to skip the dropship on the first day."
        );

        return new BepInExPluginConfig(
            enabledConfig: enabledConfig,
            requireReroutingOnFirstDayConfig: requireReroutingOnFirstDayConfig
        );
    }
}
