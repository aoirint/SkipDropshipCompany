# Dropship Orders

## Target

- Game: Lethal Company v81
- Steam manifest ID: `6423525044216269478`

## Patch and access targets

### `Terminal`

| Member | Declaration | Role |
| --- | --- | --- |
| Purchasable items | `public Item[] buyableItemsList` | Resolves a pending item index to the item definition used for direct spawning. |
| Pending order | `public List<int> orderedItemsFromTerminal` | Ordered item indexes awaiting dropship processing. Preserve list order when retaining a portion of an order. |
| Dropship count | `public int numberOfItemsInDropship` | The count supplied by credit synchronization. |
| Credit synchronization | `public void SyncGroupCreditsClientRpc(int newGroupCredits, int numItemsInShip)` | Prefix before the base game updates terminal state; postfix after it has done so. |

### `Item` and `GrabbableObject`

| Member | Declaration | Role |
| --- | --- | --- |
| Item identity | `public int itemId` | Identifies the item definition independently of a spawned object instance. |
| Item prefab | `public GameObject spawnPrefab` | Prefab instantiated for an immediate item delivery. |
| Falling state | `public float fallTime` | Set to `0f` before spawn so the new item begins in the expected placement state. |
| Ship flags | `public bool isInElevator`, `public bool isInShipRoom` | Set before network spawn so clients receive a ship-contained item. |
| Ground state | `public bool hasHitGround` | Set for an immediately placed item rather than a falling delivery. |

### `StartOfRound`

| Member | Declaration | Role |
| --- | --- | --- |
| Ship parent | `public Transform elevatorTransform` | Parent the direct-delivery item to the ship elevator. |
| Spawn positions | `public Transform[] playerSpawnPositions` | Index `1` is the base position used by the v81 out-of-bounds delivery path. |
| Round start | `public void StartGame()` | Postfix point for recording that a landing lifecycle began. |
| Ship reset | `public void ResetShip()` | Postfix point for clearing landing state after the base reset. |

## Implementation choices

### Split a pending order

#### Patch `Terminal.SyncGroupCreditsClientRpc(int, int)` with prefix and postfix — recommended

At this boundary the pending index list and synchronized dropship count are
available in one terminal lifecycle step. Use the prefix for preparation and
the postfix for spawning and retained-order restoration.

#### Patch purchase entry

This occurs before the synchronization boundary, so it does not observe the
terminal state after the base RPC applies its update.

#### Patch a later item-spawn path

The later path has already lost the clean preparation boundary for the pending
order and its synchronized count.

### Retain dropship items

#### Replace `orderedItemsFromTerminal` with the retained ordered list — recommended

The list field is the base representation of pending state. Assigning the
complete retained sequence makes that state explicit after the base RPC.

#### Mutate the existing list in place

This can yield the same elements, but leaves the retained post-RPC state less
explicit and makes ownership of the sequence harder to reason about.

### Track landing lifecycle

#### Record in `StartGame()` and clear in `ResetShip()` postfices — recommended

The two round methods supply explicit lifecycle start and reset boundaries.

#### Infer landing from credit synchronization

Credit synchronization represents terminal state, not a completed landing.

### Choose a destination for eligibility

#### Query the current round destination at decision time — recommended

Company and moon delivery behaviour differs, and this uses the destination
that actually applies to the current round.

#### Reuse a previous destination

A cached value can describe a prior round rather than the round being handled.

### Spawn an immediate item

#### Instantiate `Item.spawnPrefab` below `elevatorTransform`, then spawn its `NetworkObject` — recommended

Resolve the ordered index through `buyableItemsList`, instantiate its prefab at
the v81 ship-side spawn position, and set `fallTime`, `isInElevator`,
`isInShipRoom`, and `hasHitGround` before `NetworkObject.Spawn(false)`. This
matches the base game's ship-contained object state at replication time.

#### Call a dropship-delivery path or spawn the prefab without a `NetworkObject`

The dropship path retains the delayed delivery behaviour this mod is changing.
A local-only prefab is not replicated to clients as a purchased game object.

#### Spawn before setting the ship and falling flags

Clients can receive the object before the direct-delivery state is applied,
leaving a falling or non-ship-contained item visible during synchronization.

## Order and landing lifecycle

`Terminal.orderedItemsFromTerminal` is the pending purchase list. Its values
index `Terminal.buyableItemsList`; they are not spawned item instances. A
split-delivery implementation must preserve the retained indexes and their
order, then assign the retained list back after the terminal RPC.

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
5. Resolve item indexes through `buyableItemsList`, and set ship/falling flags
   before spawning the item's `NetworkObject`.
