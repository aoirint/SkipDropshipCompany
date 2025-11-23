using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using SkipDropshipCompany.Generated;
using SkipDropshipCompany.Managers;

namespace SkipDropshipCompany;

[BepInPlugin(ModInfo.GUID, ModInfo.NAME, ModInfo.VERSION)]
[BepInProcess("Lethal Company.exe")]
public class SkipDropshipCompany : BaseUnityPlugin
{

    internal static new ManualLogSource Logger { get; private set; }

    internal static Harmony harmony = new(ModInfo.GUID);

    internal static InstantPurchaseManager instantPurchaseManager = new();

    internal static LandingHistoryManager landingHistoryManager = new();

    private void Awake()
    {
        Logger = base.Logger;

        harmony.PatchAll();

        Logger.LogInfo($"Plugin {ModInfo.NAME} v{ModInfo.VERSION} is loaded!");
    }
}
