#nullable enable

using System.Collections.Generic;
using SkipDropshipCompany.Core.Ports;
using SkipDropshipCompany.Core.State;
using SkipDropshipCompany.Interop.Game.Adapters;

namespace SkipDropshipCompany.Interop.Game;

/// <summary>
/// Game-facing implementation of the mod operations requested by Core.
/// </summary>
/// <remarks>
/// Presents one SDC-oriented surface while focused adapters handle networking,
/// round state, terminal order state, and item spawning.
/// </remarks>
internal sealed class GameInterop : IGameInterop
{
    private readonly NetworkAdapter networkAdapter;
    private readonly RoundAdapter roundAdapter;
    private readonly TerminalAdapter terminalAdapter;
    private readonly ItemSpawnAdapter itemSpawnAdapter;

    public GameInterop(IPluginLogger logger)
    {
        networkAdapter = new NetworkAdapter(logger);
        roundAdapter = new RoundAdapter(logger);
        terminalAdapter = new TerminalAdapter(logger);
        itemSpawnAdapter = new ItemSpawnAdapter(logger);
    }

    public bool IsServer()
    {
        return networkAdapter.IsServer();
    }

    public RoundState GetRoundState()
    {
        return roundAdapter.GetRoundState();
    }

    public string? GetCurrentLevelSceneName()
    {
        return roundAdapter.GetCurrentLevelSceneName();
    }

    public List<int>? GetTerminalOrderedItemIndexes()
    {
        return terminalAdapter.GetOrderedItemIndexes();
    }

    public bool SetTerminalOrderedItemIndexes(List<int> boughtItemIndexes)
    {
        return terminalAdapter.SetOrderedItemIndexes(boughtItemIndexes);
    }

    /// <summary>
    /// Spawns the buyable item represented by a terminal item index inside the ship.
    /// </summary>
    public bool SpawnBuyableItemInShip(int buyableItemIndex)
    {
        // Core works with terminal item indexes because that is what the
        // base-game order list stores; Interop resolves the index to an Item at
        // the final Unity boundary.
        var item = terminalAdapter.GetBuyableItemByIndex(buyableItemIndex);
        if (item == null)
        {
            return false;
        }

        return itemSpawnAdapter.SpawnItemInShip(item);
    }
}
