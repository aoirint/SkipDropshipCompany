// SPDX-License-Identifier: MIT
#nullable enable

using HarmonyLib;

namespace SkipDropshipCompany.Interop.Game.Patches;

[HarmonyPatch(typeof(Terminal))]
internal static class TerminalPatch
{
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

        // Harmony requires this ref mutation at the patch boundary so the base
        // RPC sees the dropship-only item count.
        numItemsInShip = result.DropShipBoughtItemIndexes.Count;
    }

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
