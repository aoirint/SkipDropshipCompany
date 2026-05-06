// SPDX-License-Identifier: MIT
#nullable enable

using System;
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
        try
        {
            SkipDropshipCompany.Controller.HandleStartGame();
        }
        catch (Exception exception)
        {
            RecordCallbackException(StartGameCallback, exception);
        }
    }

    [HarmonyPatch(nameof(StartOfRound.ResetShip))]
    [HarmonyPostfix]
    public static void ResetShipPostfix()
    {
        try
        {
            SkipDropshipCompany.Controller.HandleResetShip();
        }
        catch (Exception exception)
        {
            RecordCallbackException(ResetShipCallback, exception);
        }
    }

    private static void RecordCallbackException(string callback, Exception exception)
    {
        try
        {
            SkipDropshipCompany.Controller.RecordCallbackException(callback, exception);
        }
        catch
        {
            // Diagnostics must not turn a swallowed lifecycle callback into a base-game failure.
        }
    }
}
