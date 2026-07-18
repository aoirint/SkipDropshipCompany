# Purchase Workflow

## Model

`PreparedInstantPurchaseStore` holds one prepared immediate-delivery order.
The current policy places every eligible pending item in that order and retains
no dropship indexes. `LandingHistoryStore` retains one selected-destination
snapshot. `RoundState` supplies the current round context.

These are mod models. The meaning of the terminal's pending-order field and
round callbacks is documented in
[../domain/dropship-orders.md](../domain/dropship-orders.md).

## Terminal flow

The terminal prefix delegates to `TerminalSyncGroupCreditsHandler`, which
reads the pending indexes and executes `PrepareInstantPurchaseUseCase`.
Eligibility is decided by `InstantPurchaseEligibilityUseCase`; the prepared
immediate order is stored until the matching postfix.

## Eligibility policy

`Enabled` gates every case. When enabled, instant purchase is allowed for a
Company destination while landed; first-day orbit when
`RequireReroutingOnFirstDay` is false; first-day orbit already routing to
Company; or orbit after the one-entry `LandingHistoryStore` recorded Company
while routing to Company. The history is a selected-destination snapshot from
`StartGame`, intentionally earlier than physical landing.

The postfix executes `SpawnPreparedInstantPurchasedItemsUseCase`, spawns the
complete prepared order, and restores an empty pending-order list through game
interop. Preparation and consumption are deliberately separate so the same
prepared item sequence is used on both sides of the base terminal callback.

Immediate items are placed near `playerSpawnPositions[1]` with a deterministic
per-item X offset and a `z + 1f` offset. This intentionally differs from the
base game's randomized out-of-bounds delivery spread. The adapter also sets
`fallTime = 0f`, `isInElevator = true`, `isInShipRoom = true`, and
`hasHitGround = true` before spawning the `NetworkObject`. The last setting is
the mod's immediate-placement policy; these ordinary fields are not themselves
`NetworkVariable` values, so the mod must not claim that `Spawn` alone
synchronizes them.

## Round flow

The `StartGame` postfix records the selected-destination snapshot through
`RecordLandingUseCase`. The `ResetShip` postfix clears it through
`ClearLandingHistoryUseCase`. Eligibility checks consume this state instead of
inferring a destination from terminal credit synchronization.

## Failure policy

A missing terminal order is an unavailable game read and stops the purchase
flow. When preparation itself returns no result, that postfix does not create a
new delivery. A previously prepared store can nevertheless remain after a
spawn failure and be consumed by a later postfix if that later prefix does not
replace or clear it.

If item spawning fails after earlier items succeeded, the use case stops at the
first failure, retains the prepared store, and leaves the terminal order
unchanged because the handler receives no result to restore. The outcome can
therefore contain already spawned items alongside the original pending order.
