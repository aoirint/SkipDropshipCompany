// SPDX-License-Identifier: MIT
#nullable enable

using HarmonyLib;

namespace SkipDropshipCompany.Interop.Game.Patches;

[HarmonyPatch(typeof(StartOfRound))]
internal static class StartOfRoundPatch
{
    // StartGame is a base-game round lifecycle entry point. SDC observes after
    // it completes so landing-history state can be updated without changing the
    // base lifecycle call.
    [HarmonyPatch(nameof(StartOfRound.StartGame))]
    [HarmonyPostfix]
    public static void StartGamePostfix()
    {
        HarmonyCallbackGuard.TryNotifyHarmonyCallback(
            callback: HarmonyCallbackTokens.StartOfRoundStartGamePostfix,
            notify: static () => SkipDropshipCompany.Controller.HandleStartGame()
        );
    }

    // ResetShip is the base-game ship reset lifecycle path. SDC observes after
    // the reset so stale dropship/landing-history state can be cleared without
    // making the reset depend on SDC callback success.
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
