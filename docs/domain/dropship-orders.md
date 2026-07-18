# Dropship Orders

## Target

- Game: Lethal Company v81
- Steam manifest ID: `6423525044216269478`

## Patch and access targets

### `Terminal`

| Member | Declaration | Role |
| --- | --- | --- |
| Pending order | `public List<int> orderedItemsFromTerminal` | Ordered item indexes awaiting dropship processing. Preserve list order when retaining a portion of an order. |
| Dropship count | `public int numberOfItemsInDropship` | The count supplied by credit synchronization. |
| Credit synchronization | `public void SyncGroupCreditsClientRpc(int newGroupCredits, int numItemsInShip)` | Prefix before the base game updates terminal state; postfix after it has done so. |

### `StartOfRound`

| Member | Declaration | Role |
| --- | --- | --- |
| Round start | `public void StartGame()` | Postfix point for recording that a landing lifecycle began. |
| Ship reset | `public void ResetShip()` | Postfix point for clearing landing state after the base reset. |

## Implementation choices

| Decision | Options | Recommended approach | Why |
| --- | --- | --- | --- |
| Split a pending order | Patch purchase entry; patch `SyncGroupCreditsClientRpc(int, int)`; patch a later item-spawn path | Use prefix and postfix around `SyncGroupCreditsClientRpc(int, int)`. | At this boundary the pending index list and synchronized dropship count are available in one terminal lifecycle step; a later spawn path has already lost the clean preparation boundary. |
| Retain dropship items | Mutate the existing list in place; replace `orderedItemsFromTerminal` with the retained ordered list | Replace the field with the retained list in the postfix. | The base state is represented by the list field itself; assigning the complete retained sequence makes the post-RPC pending state explicit. |
| Track landing lifecycle | Infer it from credit synchronization; record in `StartGame()` and clear in `ResetShip()` | Use postfices on `StartGame()` and `ResetShip()`. | Credit synchronization represents terminal state, not a completed landing; the round methods give explicit start and reset boundaries. |
| Choose a destination for eligibility | Reuse a previous destination; query the current round destination at decision time | Query the current round destination. | Company and moon delivery behaviour differs, and a cached prior destination can describe the wrong round. |

## Order and landing lifecycle

`Terminal.orderedItemsFromTerminal` is the pending purchase list. Its values
are indexes into the terminal's purchasable-item data, not spawned item
instances. A split-delivery implementation must preserve the retained indexes
and their order, then assign the retained list back after the terminal RPC.

`Terminal.SyncGroupCreditsClientRpc(int, int)` is the boundary where the game
applies synchronized credits and dropship count. Use a prefix to snapshot and
partition `orderedItemsFromTerminal`; use a postfix to spawn the immediate
portion and restore the retained dropship portion. A credit-sync callback alone
does not establish that a physical delivery has landed.

`StartOfRound.StartGame()` and `StartOfRound.ResetShip()` bracket the local
landing history: record after the former completes, and clear after the latter
completes. Destination-sensitive behaviour must query the current round
destination when deciding whether a purchase is eligible; do not reuse a
previous-level decision.

## Change checklist

1. Patch `SyncGroupCreditsClientRpc(int, int)` with both prefix and postfix;
   do not target a same-named overload by name alone.
2. Preserve `orderedItemsFromTerminal` values and ordering.
3. Keep preparation, instant spawning, and retained-list restoration as
   separate stages around the base RPC.
4. Clear landing history in a `ResetShip()` postfix, not before base reset.
