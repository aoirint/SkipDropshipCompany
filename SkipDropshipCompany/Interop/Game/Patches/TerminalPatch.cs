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
        var result = SkipDropshipCompany.Controller.PrepareInstantPurchase(
            __instance.orderedItemsFromTerminal
        );
        if (result == null)
        {
            return;
        }

        numItemsInShip = result.DropShipBoughtItemIndexes.Count;
    }

    [HarmonyPatch(nameof(Terminal.SyncGroupCreditsClientRpc))]
    [HarmonyPostfix]
    public static void SyncGroupCreditsClientRpcPostfix(
        Terminal __instance,
        int newGroupCredits,
        ref int numItemsInShip
    )
    {
        var result = SkipDropshipCompany.Controller.SpawnPreparedInstantPurchasedItems();
        if (result == null)
        {
            return;
        }

        __instance.orderedItemsFromTerminal = result.DropShipBoughtItemIndexes;
    }
}
