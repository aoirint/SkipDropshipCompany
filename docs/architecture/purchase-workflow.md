# Purchase Workflow

## Model

`PreparedInstantPurchaseStore` holds one prepared split of the pending order.
It separates item indexes to spawn immediately from indexes retained for the
dropship. `LandingHistoryStore` records whether the current landing lifecycle
has already been observed. `RoundState` supplies the current round context.

These are mod models. The meaning of the terminal's pending-order field and
round callbacks is documented in
[../domain/dropship-orders.md](../domain/dropship-orders.md).

## Terminal flow

The terminal prefix delegates to `TerminalSyncGroupCreditsHandler`, which
reads the pending indexes and executes `PrepareInstantPurchaseUseCase`.
Eligibility is decided by `InstantPurchaseEligibilityUseCase`; the prepared
split is stored until the matching postfix.

The postfix executes `SpawnPreparedInstantPurchasedItemsUseCase`, spawns the
immediate portion, and restores the retained dropship indexes through game
interop. Preparation and consumption are deliberately separate so that the
same partition is used on both sides of the base terminal callback.

## Round flow

The `StartGame` postfix records landing history through
`RecordLandingUseCase`. The `ResetShip` postfix clears it through
`ClearLandingHistoryUseCase`. Eligibility checks consume this state instead
of inferring a landing from terminal credit synchronization.

## Failure policy

A missing terminal order is an unavailable game read and stops the purchase
flow. A failed preparation is not retried by the postfix. This avoids treating
a partial order as empty or spawning an instant item without a matching
retained-order decision.

