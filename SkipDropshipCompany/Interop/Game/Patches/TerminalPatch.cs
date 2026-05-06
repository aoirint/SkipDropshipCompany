// SPDX-License-Identifier: MIT
#nullable enable

using HarmonyLib;

namespace SkipDropshipCompany.Interop.Game.Patches;

[HarmonyPatch(typeof(Terminal))]
internal static class TerminalPatch
{
    // For the base game, SyncGroupCreditsClientRpc is the client-side RPC that
    // applies synchronized group credits and the dropship item count. The Prefix
    // can only change the count argument before that base-game apply step.
    [HarmonyPatch(nameof(Terminal.SyncGroupCreditsClientRpc))]
    [HarmonyPrefix]
    public static void SyncGroupCreditsClientRpcPrefix(
        Terminal __instance,
        int newGroupCredits,
        ref int numItemsInShip
    )
    {
        var completed = HarmonyCallbackGuard.TryNotifyHarmonyCallback(
            callback: HarmonyCallbackTokens.TerminalSyncGroupCreditsClientRpcPrefix,
            notify: static () =>
                SkipDropshipCompany.Controller.HandleTerminalSyncGroupCreditsClientRpcPrefix(),
            result: out var result
        );
        if (!completed)
        {
            return;
        }

        if (result == null)
        {
            return;
        }

        // Keep Harmony ref mutation outside the guard. A failed notification
        // must leave the original base-game RPC argument untouched.
        numItemsInShip = result.DropShipBoughtItemIndexes.Count;
    }

    // For the base game, the later dropship delivery flow, not this RPC, reads
    // orderedItemsFromTerminal as the delivery source. The Postfix restores the
    // retained order after the RPC finishes its credit/count synchronization.
    [HarmonyPatch(nameof(Terminal.SyncGroupCreditsClientRpc))]
    [HarmonyPostfix]
    public static void SyncGroupCreditsClientRpcPostfix()
    {
        HarmonyCallbackGuard.TryNotifyHarmonyCallback(
            callback: HarmonyCallbackTokens.TerminalSyncGroupCreditsClientRpcPostfix,
            notify: static () =>
                SkipDropshipCompany.Controller.HandleTerminalSyncGroupCreditsClientRpcPostfix()
        );
    }
}
