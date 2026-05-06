// SPDX-License-Identifier: MIT
#nullable enable

using System.Collections.Generic;
using SkipDropshipCompany.Core.State;

namespace SkipDropshipCompany.Core.Ports;

internal interface IGameInterop
{
    bool IsServer();

    RoundState GetRoundState();

    string? GetCurrentLevelSceneName();

    List<int>? GetOrderedItemsFromTerminal();

    bool SpawnBuyableItemInShip(int buyableItemIndex);
}
