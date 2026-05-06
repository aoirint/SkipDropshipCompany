// SPDX-License-Identifier: MIT
#nullable enable

using HarmonyLib;

namespace SkipDropshipCompany.Interop.Game.Patches;

[HarmonyPatch(typeof(StartOfRound))]
internal static class StartOfRoundPatch
{
    // For the base game, StartGame enters the round-start lifecycle. The
    // Postfix runs after that lifecycle step without changing the base call.
    [HarmonyPatch(nameof(StartOfRound.StartGame))]
    [HarmonyPostfix]
    public static void StartGamePostfix()
    {
        HarmonyCallbackGuard.TryNotifyHarmonyCallback(
            callback: HarmonyCallbackTokens.StartOfRoundStartGamePostfix,
            notify: static () => SkipDropshipCompany.Controller.HandleStartGame()
        );
    }

    // For the base game, ResetShip clears ship and round state for the next
    // lifecycle phase. The Postfix runs after the reset without making the base
    // reset depend on SDC callback success.
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
