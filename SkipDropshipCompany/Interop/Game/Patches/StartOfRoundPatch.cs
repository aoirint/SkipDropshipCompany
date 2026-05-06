// SPDX-License-Identifier: MIT
#nullable enable

using HarmonyLib;

namespace SkipDropshipCompany.Interop.Game.Patches;

[HarmonyPatch(typeof(StartOfRound))]
internal static class StartOfRoundPatch
{
    private const string StartGameCallback = "start_game_postfix";
    private const string ResetShipCallback = "reset_ship_postfix";

    [HarmonyPatch(nameof(StartOfRound.StartGame))]
    [HarmonyPostfix]
    public static void StartGamePostfix()
    {
        HarmonyCallbackGuard.TryNotifyHarmonyCallback(
            StartGameCallback,
            SkipDropshipCompany.Controller.HandleStartGame
        );
    }

    [HarmonyPatch(nameof(StartOfRound.ResetShip))]
    [HarmonyPostfix]
    public static void ResetShipPostfix()
    {
        HarmonyCallbackGuard.TryNotifyHarmonyCallback(
            ResetShipCallback,
            SkipDropshipCompany.Controller.HandleResetShip
        );
    }
}
