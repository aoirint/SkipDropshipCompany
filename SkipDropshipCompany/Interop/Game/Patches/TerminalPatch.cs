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
        var result = SkipDropshipCompany.Controller.PrepareTerminalSyncGroupCreditsInstantPurchase();
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
        SkipDropshipCompany.Controller.SpawnTerminalSyncGroupCreditsPreparedItems();
    }
}
