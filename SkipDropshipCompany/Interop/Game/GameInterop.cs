// SPDX-License-Identifier: MIT
#nullable enable

using SkipDropshipCompany.Core.Ports;
using SkipDropshipCompany.Core.State;
using SkipDropshipCompany.Interop.Game.Adapters;

namespace SkipDropshipCompany.Interop.Game;

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

    public bool SpawnBuyableItemInShip(int buyableItemIndex)
    {
        var item = terminalAdapter.GetBuyableItemByIndex(buyableItemIndex);
        if (item == null)
        {
            return false;
        }

        return itemSpawnAdapter.SpawnItemInShip(item);
    }
}
