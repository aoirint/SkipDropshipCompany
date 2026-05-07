#nullable enable

using System.Collections.Generic;
using System.Linq;
using SkipDropshipCompany.Core.Ports;
using UnityEngine;

namespace SkipDropshipCompany.Interop.Game.Adapters;

/// <summary>
/// Owns the mutable Terminal order-list boundary.
/// </summary>
/// <remarks>
/// Caching avoids repeated scene searches during the paired Prefix/Postfix callbacks.
/// </remarks>
internal sealed class TerminalAdapter
{
    private readonly IPluginLogger logger;
    private Terminal? cachedTerminal;

    public TerminalAdapter(IPluginLogger logger)
    {
        this.logger = logger;
    }

    public Item? GetBuyableItemByIndex(int index)
    {
        var terminal = GetTerminal();
        if (terminal == null)
        {
            logger.LogError("Terminal is null.");
            return null;
        }

        var buyableItemsList = terminal.buyableItemsList;
        if (buyableItemsList == null)
        {
            logger.LogError("Terminal.buyableItemsList is null.");
            return null;
        }

        return buyableItemsList.ElementAtOrDefault(index);
    }

    public List<int>? GetOrderedItemIndexes()
    {
        var terminal = GetTerminal();
        if (terminal == null)
        {
            return null;
        }

        return terminal.orderedItemsFromTerminal;
    }

    public bool SetOrderedItemIndexes(List<int> boughtItemIndexes)
    {
        var terminal = GetTerminal();
        if (terminal == null)
        {
            return false;
        }

        // Replacing the whole list mirrors the base-game field shape and keeps
        // the retained dropship order explicit after instant delivery spawns.
        terminal.orderedItemsFromTerminal = boughtItemIndexes;
        return true;
    }

    private Terminal? GetTerminal()
    {
        if (cachedTerminal != null)
        {
            return cachedTerminal;
        }

        Terminal terminal = Object.FindObjectOfType<Terminal>();
        if (terminal == null)
        {
            logger.LogError("Failed to find Terminal instance in the scene.");
            return null;
        }

        cachedTerminal = terminal;
        return terminal;
    }
}
