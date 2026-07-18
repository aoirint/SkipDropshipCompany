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
| Credit synchronization | `public void SyncGroupCreditsClientRpc(int newGroupCredits, int numItemsInShip)` | RPC send/receive boundary carrying credits and dropship count. |

### `Item` and `GrabbableObject`

| Member | Declaration | Role |
| --- | --- | --- |
| Item identity | `public int itemId` | Identifies the item definition independently of a spawned object instance. |
| Item prefab | `public GameObject spawnPrefab` | Prefab instantiated for an immediate item delivery. |
| Falling state | `public float fallTime` | Controls the base `GrabbableObject` falling update. |
| Ship flags | `public bool isInElevator`, `public bool isInShipRoom` | Ordinary fields describing current ship/elevator membership. |
| Ground state | `public bool hasHitGround` | Ordinary field describing whether the object has reached ground. |

### `StartOfRound`

| Member | Declaration | Role |
| --- | --- | --- |
| Ship parent | `public Transform elevatorTransform` | Parent the direct-delivery item to the ship elevator. |
| Spawn positions | `public Transform[] playerSpawnPositions` | Index `1` is used by the saved ship-item recovery path when a saved position is outside ship bounds. |
| Round start | `public void StartGame()` | Postfix point for a selected-destination snapshot before physical landing. |
| Ship reset | `public void ResetShip()` | Base reset lifecycle boundary. |

### `Unity.Netcode.NetworkObject`

| Member | Declaration | Role |
| --- | --- | --- |
| Network spawn | `public void Spawn(bool destroyWithScene = false)` | Spawns the item's network object. |

## Implementation choices

### Split a pending order

#### Patch `Terminal.SyncGroupCreditsClientRpc(int, int)` with prefix and postfix — recommended

At this boundary the pending index list and outgoing dropship count are
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

Resolve the ordered index through `buyableItemsList`, instantiate its prefab
near `playerSpawnPositions[1]`, then call `NetworkObject.Spawn(false)`. The
The saved ship-item recovery path sets `fallTime`, `isInElevator`, and
`isInShipRoom` as local object state; it does not establish their network
replication. A mod must supply separate evidence or synchronization for these
ordinary fields.

#### Call a dropship-delivery path or spawn the prefab without a `NetworkObject`

The dropship path retains the base game's delayed delivery behaviour.
A local-only prefab is not replicated to clients as a purchased game object.

#### Choose a different local field-setting order

The base recovery path offers one local ordering example, but the appropriate
order and any client-visible effect depend on the mod's own synchronization.

## Order and landing lifecycle

`Terminal.orderedItemsFromTerminal` is the pending purchase list. Its values
index `Terminal.buyableItemsList`; they are not spawned item instances. A
split-delivery implementation must preserve the retained indexes and their
order, then assign the retained list back after the terminal RPC.

`Terminal.SyncGroupCreditsClientRpc(int, int)` is an RPC send/receive boundary.
On client execution, the game updates credits, conditionally updates
`numberOfItemsInDropship` when the argument is not `-1`, and clears the credit
cooldown. Server/host invocation serializes and sends the arguments instead.
Use a prefix to snapshot and partition `orderedItemsFromTerminal`; use a
postfix to spawn the immediate portion and restore the retained dropship
portion. A credit-sync callback alone does not establish that a physical
delivery has landed.

`StartOfRound.StartGame()` is an early selected-destination snapshot, not the
physical landing boundary. `StartOfRound.ResetShip()` is a base-game reset
lifecycle boundary suitable for a postfix. Destination-sensitive behaviour
must query the current round destination when deciding whether a purchase is
eligible; do not reuse a previous-level decision.

## Change checklist

1. Patch the `SyncGroupCreditsClientRpc` name with both prefix and postfix, as
   in the current Harmony attribute target; add signature-specific targeting if
   a same-named overload is introduced.
2. Preserve `orderedItemsFromTerminal` values and ordering.
3. Keep preparation, instant spawning, and retained-list restoration as
   separate stages around the base RPC.
4. Use a `ResetShip()` postfix as the lifecycle boundary for any mod-owned
   reset policy.
5. Resolve item indexes through `buyableItemsList`, and set ship/falling flags
   before spawning the item's `NetworkObject`.
