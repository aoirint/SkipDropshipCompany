#nullable enable

using System.Collections.Generic;
using SkipDropshipCompany.Core.State;

namespace SkipDropshipCompany.Core.Ports;

/// <summary>
/// Names the game operations the instant-purchase rules need without exposing Unity objects.
/// </summary>
internal interface IGameInterop
{
    bool IsServer();

    RoundState GetRoundState();

    string? GetCurrentLevelSceneName();

    List<int>? GetTerminalOrderedItemIndexes();

    bool SetTerminalOrderedItemIndexes(List<int> boughtItemIndexes);

    bool SpawnBuyableItemInShip(int buyableItemIndex);
}
