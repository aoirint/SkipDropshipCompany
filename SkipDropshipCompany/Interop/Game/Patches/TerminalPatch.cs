// SPDX-License-Identifier: MIT
#nullable enable

using HarmonyLib;

namespace SkipDropshipCompany.Interop.Game.Patches;

[HarmonyPatch(typeof(Terminal))]
internal static class TerminalPatch
{
    // SyncGroupCreditsClientRpc applies the synchronized group credits and the
    // dropship item count during the base RPC body. The Prefix can still adjust
    // only the count argument before the base game observes it.
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

    // The base RPC does not consume orderedItemsFromTerminal as the delivery
    // source. Later dropship flow reads that order, so the Postfix restores the
    // retained dropship order after the credit/count synchronization returns.
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
