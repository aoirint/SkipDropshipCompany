#nullable enable

using HarmonyLib;

namespace SkipDropshipCompany.Interop.Game.Patches;

[HarmonyPatch(typeof(Terminal))]
internal static class TerminalPatch
{
    /// <summary>
    /// Prepares instant delivery before the base-game terminal RPC applies its count.
    /// </summary>
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

    /// <summary>
    /// Restores retained dropship order after the base-game terminal RPC finishes.
    /// </summary>
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
