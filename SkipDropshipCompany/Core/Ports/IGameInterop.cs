// SPDX-License-Identifier: MIT
#nullable enable

using SkipDropshipCompany.Core.State;

namespace SkipDropshipCompany.Core.Ports;

internal interface IGameInterop
{
    bool IsServer();

    RoundState GetRoundState();

    bool SpawnBuyableItemInShip(int buyableItemIndex);
}
