// SPDX-License-Identifier: MIT
#nullable enable

using System;
using HarmonyLib;

namespace SkipDropshipCompany.Interop.Game.Patches;

[HarmonyPatch(typeof(Terminal))]
internal static class TerminalPatch
{
    private const string PrefixCallback = "terminal_sync_group_credits_prefix";
    private const string PostfixCallback = "terminal_sync_group_credits_postfix";

    [HarmonyPatch(nameof(Terminal.SyncGroupCreditsClientRpc))]
    [HarmonyPrefix]
    public static void SyncGroupCreditsClientRpcPrefix(
        Terminal __instance,
        int newGroupCredits,
        ref int numItemsInShip
    )
    {
        try
        {
            var result = SkipDropshipCompany.Controller.PrepareTerminalSyncGroupCreditsInstantPurchase();
            if (result == null)
            {
                return;
            }

            // Harmony requires this ref mutation at the patch boundary so the base
            // RPC sees the dropship-only item count.
            numItemsInShip = result.DropShipBoughtItemIndexes.Count;
        }
        catch (Exception exception)
        {
            RecordCallbackException(PrefixCallback, exception);
        }
    }

    [HarmonyPatch(nameof(Terminal.SyncGroupCreditsClientRpc))]
    [HarmonyPostfix]
    public static void SyncGroupCreditsClientRpcPostfix()
    {
        try
        {
            SkipDropshipCompany.Controller.SpawnTerminalSyncGroupCreditsPreparedItems();
        }
        catch (Exception exception)
        {
            RecordCallbackException(PostfixCallback, exception);
        }
    }

    private static void RecordCallbackException(string callback, Exception exception)
    {
        try
        {
            SkipDropshipCompany.CallbackDiagnostics.RecordException(callback, exception);
        }
        catch
        {
            // Diagnostics must not turn a fail-open callback path into a base-game failure.
        }
    }
}
