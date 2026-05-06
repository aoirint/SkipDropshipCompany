// SPDX-License-Identifier: MIT
#nullable enable

using System.Collections.Generic;

namespace SkipDropshipCompany.Core.UseCases;

internal sealed class PrepareInstantPurchaseResult
{
    public PrepareInstantPurchaseResult(
        List<int> dropShipBoughtItemIndexes,
        List<int> instantBoughtItemIndexes
    )
    {
        DropShipBoughtItemIndexes = dropShipBoughtItemIndexes;
        InstantBoughtItemIndexes = instantBoughtItemIndexes;
    }

    public List<int> DropShipBoughtItemIndexes { get; }

    public List<int> InstantBoughtItemIndexes { get; }
}

internal sealed class SpawnPreparedInstantPurchasedItemsResult
{
    public SpawnPreparedInstantPurchasedItemsResult(
        List<int> dropShipBoughtItemIndexes,
        List<int> instantBoughtItemIndexes
    )
    {
        DropShipBoughtItemIndexes = dropShipBoughtItemIndexes;
        InstantBoughtItemIndexes = instantBoughtItemIndexes;
    }

    public List<int> DropShipBoughtItemIndexes { get; }

    public List<int> InstantBoughtItemIndexes { get; }
}
