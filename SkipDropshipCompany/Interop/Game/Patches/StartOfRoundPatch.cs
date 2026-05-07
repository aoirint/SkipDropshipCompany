#nullable enable

using HarmonyLib;

namespace SkipDropshipCompany.Interop.Game.Patches;

[HarmonyPatch(typeof(StartOfRound))]
internal static class StartOfRoundPatch
{
    /// <summary>
    /// Records landing history after the base-game round-start lifecycle enters.
    /// </summary>
    [HarmonyPatch(nameof(StartOfRound.StartGame))]
    [HarmonyPostfix]
    public static void StartGamePostfix()
    {
        HarmonyCallbackGuard.TryNotifyHarmonyCallback(
            callback: HarmonyCallbackTokens.StartOfRoundStartGamePostfix,
            notify: static () => SkipDropshipCompany.Controller.HandleStartGame()
        );
    }

    /// <summary>
    /// Clears landing history after the base-game ship reset completes.
    /// </summary>
    [HarmonyPatch(nameof(StartOfRound.ResetShip))]
    [HarmonyPostfix]
    public static void ResetShipPostfix()
    {
        HarmonyCallbackGuard.TryNotifyHarmonyCallback(
            callback: HarmonyCallbackTokens.StartOfRoundResetShipPostfix,
            notify: static () => SkipDropshipCompany.Controller.HandleResetShip()
        );
    }
}
