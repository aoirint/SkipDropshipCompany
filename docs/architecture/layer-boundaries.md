# Layer Boundaries

## Core

`Core` owns the purchase and round models, use cases, handlers, result values,
and ports. It decides eligibility, partitions pending indexes, stores prepared
work, records landing history, and defines failure outcomes.

Core depends on interfaces such as `IGameInterop`, `IPluginConfig`,
`IPluginLogger`, and `IValidationLogger`, not Unity, Harmony, BepInEx, or
Lethal Company types.

## Interop

`Interop` owns BepInEx configuration and logging, Harmony patches, terminal
and round adapters, item spawning, networking, and exception guards. Patch
methods delegate to `PluginController`; they do not contain purchase policy.

## Composition

`PluginController.Create()` wires stores, use cases, handlers, and adapters.
New game access belongs behind an `IGameInterop` operation implemented by an
Interop adapter. New purchase policy belongs in a Core use case rather than in
a Harmony callback.

