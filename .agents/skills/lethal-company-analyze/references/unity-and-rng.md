# Unity and RNG analysis

## Serialization

Search scene YAML, prefabs, prefab variants, nested prefabs, ScriptableObjects, addressable/bundle extracts, and build scene lists. Join a `MonoBehaviour` to its class through the script `.meta` GUID and join other assets by GUID/fileID rather than filename guesses. Traverse the complete prefab dependency graph in Unity precedence order and keep record-specific identities: property modifications use owner instance/asset + target correspondence + property path; added records use owner + owner-local added ID + attachment-target correspondence; removed records use owner + removed-source correspondence; stripped proxies use owning prefab instance + stripped local ID + source correspondence. Do not match nested objects by name alone. Confirm which GameObjects/components survive composition before resolving their properties. Apply serialized scene prefab-instance modifications next, then separately trace runtime instantiation changes and later code assignments. The resolved serialized value supersedes the code initializer even on a disabled or inactive object. Separately decide whether the object/value can affect the declared endpoint window by checking structural existence, enabled/component and GameObject-active state where applicable, plus scene/asset load-path reachability. Record whether a value is a code default, serialized override at each layer, serialized scene modification, or runtime mutation.

Classify individual serialized source records as `direct_static`. Classify an effective serialized state composed across verified prefab/scene precedence as `exact_derived`; classify whether it affected the runtime endpoint separately under the evidence policy's runtime-promotion boundary.

## RNG streams

Inventory every RNG constructor and seed offset. Record draw order, conditional draws, `Next` exclusive maxima, float32 versus float64 arithmetic, casts, clamps, retries, and fallback branches. Treat two streams with related seeds as separate but not automatically statistically independent.

For Mono legacy `System.Random`, verify against a C# oracle or frozen vectors. In weighted selection, reproduce the game's accumulator precision and comparison direction exactly.

## Runtime consumption hazards

Static item-list generation does not imply exact realized values when the same RNG later drives spawn-point selection, NavMesh sampling, retry loops, or failures before value draws. Model such paths only after reproducing the generated interior and candidate objects, or label a fixed-consumption implementation counterfactual.

For serialized spawn-entry lists, preserve asset order and distinguish three phases: the curve/requested-attempt draw, per-attempt placement draws, and successful instantiation. Record skips that occur before the curve draw, zero targets, and compatibility checks that suppress later placement draws. A later entry can therefore depend on whether an earlier compatible spawner existed even when both entries use one seeded stream. If spawner presence in the generated interior is unavailable, enumerate named global compatibility scenarios for sensitivity; do not call them observations, monotone bounds, or actual placement counts.

When a draw-count claim depends on a helper call, freeze the helper body through the actual RNG-consuming statement, not only its call sites or signature. The excerpt locator and hash must let a receiver reconstruct the complete call-to-draw alignment without access to the producer's external decompilation tree.

Treat a reproduced `AnimationCurve` integer output as exact only after validating the target build's effective serialized keys, weighted tangents, infinity behavior, float32 input, and truncation at reachable boundaries against a suitable Unity oracle. Agreement between two custom evaluators strengthens a static model but does not by itself promote it to Unity-runtime exactness.

`FindObjectsOfType` order is runtime state. Noneligible objects can change a global array index without consuming endpoint-specific RNG. A fixed offset is a scenario, not a bound, unless monotonicity is proved.

## Networking and observation windows

Trace who computes the value, who owns the authoritative state, when RPC synchronization occurs, and whether the UI queries the server or reads local objects. Define a snapshot protocol such as post-sync, pre-movement, pre-dynamic-spawn. Pair predictors and targets on the same object set and frame.

For before/after UI differences, verify whether the UI recomputes every existing object or only adds the new object. A full rescan means reordering can change old contributions, so the displayed delta is not automatically the new object's value. Define a stable-window protocol with the same client, synchronized values, count/object-set deltas, and exclusion of unrelated state changes. Model append positions only as named scenarios unless runtime captures prove enumeration stability.

For a delayed event that creates multiple objects, freeze object identity across every ordinal system: value-generation draw, any branch-mask bit, endpoint raw draw, append order, and final global index. Do not assume that equal object types make within-event permutations irrelevant when clamp bounds or endpoint draws depend on index. Either state one exact correspondence such as one-based ordinal `e` mapping to append index `first+e-1`, or enumerate named permutation scenarios.

When a value path mixes seeded `System.Random` with stateful `UnityEngine.Random`, keep the seeded portion exact and the Unity-state-dependent branch unknown. A frozen hash allocation can be useful for sensitivity analysis, but label it counterfactual and never present its per-seed branch as a recovered runtime outcome.

For synthetic populations crossed with imposed profiles or append scenarios, freeze whether scenarios are fit separately or pooled, the complete training-cell key, how moons/levels/builds enter the pool, every unit weight (including whether serialized rarity is ignored), and the exact evaluation denominator. A complete seed range does not remove ambiguity in these estimands.
