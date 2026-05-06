// SPDX-License-Identifier: MIT
#nullable enable

using HarmonyLib;

namespace SkipDropshipCompany.Interop.Game.Patches;

[HarmonyPatch(typeof(StartOfRound))]
internal static class StartOfRoundPatch
{
    [HarmonyPatch(nameof(StartOfRound.StartGame))]
    [HarmonyPostfix]
    public static void StartGamePostfix()
    {
        SkipDropshipCompany.Controller.HandleStartGame();
    }

    [HarmonyPatch(nameof(StartOfRound.ResetShip))]
    [HarmonyPostfix]
    public static void ResetShipPostfix()
    {
        SkipDropshipCompany.Controller.HandleResetShip();
    }
}
