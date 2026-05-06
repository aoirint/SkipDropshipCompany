#nullable enable

using BepInEx.Configuration;
using SkipDropshipCompany.Managers;

namespace SkipDropshipCompany;

// The controller is the plugin-facing composition root. It owns long-lived
// plugin state while the BepInEx entrypoint stays focused on startup wiring.
internal sealed class PluginController
{
    private PluginController(
        InstantPurchaseManager instantPurchaseManager,
        LandingHistoryManager landingHistoryManager,
        ConfigEntry<bool> enabledConfig,
        ConfigEntry<bool> requireReroutingOnFirstDayConfig
    )
    {
        InstantPurchaseManager = instantPurchaseManager;
        LandingHistoryManager = landingHistoryManager;
        EnabledConfig = enabledConfig;
        RequireReroutingOnFirstDayConfig = requireReroutingOnFirstDayConfig;
    }

    public InstantPurchaseManager InstantPurchaseManager { get; }

    public LandingHistoryManager LandingHistoryManager { get; }

    public ConfigEntry<bool> EnabledConfig { get; }

    public ConfigEntry<bool> RequireReroutingOnFirstDayConfig { get; }

    public static PluginController Create(ConfigFile config)
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

        return new PluginController(
            instantPurchaseManager: new InstantPurchaseManager(),
            landingHistoryManager: new LandingHistoryManager(),
            enabledConfig: enabledConfig,
            requireReroutingOnFirstDayConfig: requireReroutingOnFirstDayConfig
        );
    }
}
