using BepInEx.Logging;
using HarmonyLib;
using Unity.Netcode;

namespace QuickPurchaseCompany.Patches;

[HarmonyPatch(typeof(Terminal))]
internal class TerminalPatch
{
    internal static ManualLogSource Logger => QuickPurchaseCompany.Logger;

    [HarmonyPatch("SyncGroupCreditsClientRpc")]
    [HarmonyPrefix]
    public static void SyncGroupCreditsClientRpcPrefix(Terminal __instance, int newGroupCredits, ref int numItemsInShip)
    {
        var networkManager = NetworkManager.Singleton;
        if (networkManager == null)
        {
            Logger.LogError("NetworkManager.Singleton is null.");
            return;
        }
        if (!networkManager.IsServer)
        {
            Logger.LogDebug("Not the server. Skipping instant purchase logic.");
            return;
        }

        var instantPurchaseManager = QuickPurchaseCompany.instantPurchaseManager;
        if (instantPurchaseManager == null)
        {
            Logger.LogError("InstantPurchaseManager is null.");
            return;
        }

        var orderedItemsFromTerminal = __instance.orderedItemsFromTerminal;
        if (orderedItemsFromTerminal == null)
        {
            Logger.LogError("Terminal.orderedItemsFromTerminal is null.");
            return;
        }

        Logger.LogDebug(
            "Preparing instant purchase." +
            $" IsHost={networkManager.IsHost} IsServer={networkManager.IsServer} IsClient={networkManager.IsClient}"
        );

        var result = instantPurchaseManager.PrepareInstantPurchase(
            boughtItemIndexes: orderedItemsFromTerminal
        );
        if (!result.Succeeded)
        {
            Logger.LogDebug("Prepare instant purchase failed or not allowed. Skipping instant purchase logic.");
            return;
        }

        // Adjust the number of items in the dropship
        // NOTE:  `numItemsInShip` will be set to `Terminal.numberOfItemsInDropship`.
        // This is an incorrect argument name in the original game code.
        // It means `numItemsInDropship`.
        numItemsInShip = result.DropShipBoughtItemIndexes.Count;

        Logger.LogDebug(
            "Prepared instant purchase." +
            $" originalDropShipItemCount={orderedItemsFromTerminal.Count}" +
            $" preparedDropShipItemCount={numItemsInShip}" +
            $" preparedInstantPurchaseItemCount={result.InstantBoughtItemIndexes.Count}"
        );
    }

    [HarmonyPatch("SyncGroupCreditsClientRpc")]
    [HarmonyPostfix]
    public static void SyncGroupCreditsClientRpcPostfix(Terminal __instance, int newGroupCredits, ref int numItemsInShip)
    {
        var networkManager = NetworkManager.Singleton;
        if (networkManager == null)
        {
            Logger.LogError("NetworkManager.Singleton is null.");
            return;
        }
        if (!networkManager.IsServer)
        {
            Logger.LogDebug("Not the server. Skipping instant purchase logic.");
            return;
        }

        var instantPurchaseManager = QuickPurchaseCompany.instantPurchaseManager;
        if (instantPurchaseManager == null)
        {
            Logger.LogError("InstantPurchaseManager is null.");
            return;
        }

        Logger.LogDebug(
            "Spawning prepared instant purchased items." +
            $" IsHost={networkManager.IsHost} IsServer={networkManager.IsServer} IsClient={networkManager.IsClient}"
        );

        var result = instantPurchaseManager.SpawnPreparedInstantPurchasedItems();
        if (!result.Succeeded)
        {
            Logger.LogDebug("Spawning prepared instant purchased items failed or none to spawn.");
            return;
        }

        __instance.orderedItemsFromTerminal = result.DropShipBoughtItemIndexes;

        Logger.LogDebug("Spawned all prepared instant purchased items.");
    }
}
