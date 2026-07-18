# Dropship Orders

## Evidence scope

This document records the order and landing concepts used for Lethal Company
v81. Recheck target-version evidence before changing behavior that depends on
Terminal order state or ship-reset timing.

The claims are `direct_static` observations from the v81 decompilation's
`Terminal.cs` and `StartOfRound.cs`. The matching v81 asset root was inspected;
this document does not infer serialized item or scene configuration. Callback
ordering and successful item delivery remain runtime-dependent.

## Order state

The Terminal keeps ordered item indexes as the pending purchase state. Treat
that list as authoritative until the game processes the order. Preserve item
indexes and their order when transferring or restoring pending purchases; a
missing list is an unavailable state, not an empty order.

## Landing lifecycle

Ship-reset and Terminal credit synchronization are distinct callbacks. A
purchase flow that changes delivery timing must preserve the relationship
between pending order state, a recorded landing, and the next round reset.

Do not assume that a credit synchronization by itself proves an item has
landed. Keep order preparation separate from item spawning so a failed or
late game callback cannot duplicate an order.

## Company destination

The company scene has different delivery implications from a moon. Any
eligibility rule that depends on a first-day reroute or company destination
must use the current round destination, not a cached prior-level assumption.

## Change checklist

1. Preserve pending item indexes without reordering.
2. Distinguish unavailable Terminal state from an empty order.
3. Tie item spawning to one confirmed lifecycle point.
4. Re-evaluate destination-specific behavior after each round transition.
