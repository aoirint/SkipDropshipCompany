# BepInEx Project and Plugin Checks

## Project and module structure

1. Check C# project and module structure.
   - Start from the repository's existing layout. Do not introduce projects, layers, or folder taxonomy solely to
     resemble another mod.
   - Keep the loader entry point and composition root small. It may create the controller, adapters, state, and use
     cases, but it must not become the location for gameplay policy or framework callbacks.
   - Put gameplay policy, state transitions, result values, and use cases in framework-free Core modules when the mod
     has enough behavior to benefit from a boundary. Core may depend on ports it defines, but not on BepInEx, Harmony,
     Unity, or game-specific types.
   - Put BepInEx configuration/logging, Harmony callbacks, Unity/game-object access, networking, and concrete port
     adapters in Interop modules. Keep callbacks thin: validate or guard the framework boundary, translate data, and
     delegate to Core-facing controller methods.
   - Introduce a Core port and an Interop implementation when a use case needs game or framework access. Do not hide
     that access behind static singletons in Core.
   - When one callback or update supplies multiple outputs, capture live game/framework state once in Interop as a
     framework-free observation, derive conditions once in Core, and pass one immutable frame or result to every
     presenter. Do not let HUD, world rendering, logging, or other presentation paths independently resample the same
     live state.
   - Keep direct observations, derived conditions, and intentionally incomplete diagnostic proxies distinct in names and
     types. Represent values that become available together as one optional aggregate or result variant instead of
     correlated nullable properties, and preserve `unknown` or `unreadable` separately from observed `false` or absence.
   - Do not add persistent Core state merely because a value type belongs in Core. Keep an update frame transient unless
     history, smoothing, change detection, save/restore, or another explicit cross-callback rule requires a store.
   - Keep a module cohesive around one responsibility. Split a module when it mixes policy with framework access or when
     its lifecycle/state ownership cannot be stated clearly; do not split it merely by file type or pre-emptive reuse.
   - Keep one plugin project when it enforces the needed boundary. Add separate C# projects only when a concrete test,
     reuse, build, or dependency-isolation need outweighs the added target-framework and packaging complexity.
   - Keep repeated build properties in project files until at least two projects actually share them and the repository
     family uses a common props file. Do not add `Directory.Build.props` merely to centralize a small solution.
     `global.json` has a separate SDK-selection role and remains required when the family build uses it.

## Plugin boundary

1. Check the plugin boundary.
   - Ensure the assembly name, plugin GUID, plugin name, and loader-facing version have one documented source of truth
     or a verified synchronization path.
   - Confirm the plugin entry point uses BepInEx metadata and that generated plugin-info values, if used, agree with the
     project configuration. For a game-specific plugin with a stable tested executable name, declare `BepInProcess` and
     verify it in the built assembly.
   - Keep Harmony or equivalent patch registration deliberate: use a stable identifier, a bounded assembly/patch scope,
     and lifecycle behavior that does not duplicate registration.
   - Make every patch/callback boundary fail-safe according to its declared behavior. Guard the primary callback and its
     diagnostic/error-reporting path independently so a logging, serialization, or reporting failure cannot escape into
     the game callback.
   - Gate callback-driven state changes, scans, UI work, and network sends on the mod's declared execution role
     (`client`, `host`, or server) using an explicit role check at the Interop boundary. Treat unavailable network state
     as not authorized for that role; do not rely on incidental RPC stage or prior state to suppress work.
   - For practice, debug, cheat-like, or other client-visible features that can substantially change game balance,
     default non-host use to denied. Require an authoritative host consent path before enabling the feature for a
     guest. A local guest setting never grants consent. Use the host's installation of the same mod as the consent
     signal only when the documented feature cannot affect host security or another protected trust boundary;
     otherwise require an explicit host-controlled allow/deny policy. Document the consent model, denial behavior,
     and any deliberately different policy. Read [guest-feature-authorization.md](guest-feature-authorization.md)
     before selecting the authorization transport, settings, lifetime, or verification matrix.
   - Bind the global `Enabled` setting at the Interop/configuration boundary and gate the mod's declared work before
     expensive observation, mutation, presentation, or network sends. Keep the plugin loaded so lifecycle cleanup and
     safe callbacks remain valid. If a global gate is impractical, document the concrete scale or lifecycle constraint
     and verify that the narrower controls cover the intended operational need.
   - When an observation must be all-or-nothing, enumerate every prerequisite needed to classify each entry. A
     missing/destroyed entry, required metadata object, or required classification field fails the whole observation;
     never return a partial count merely because one nullable layer was skipped.
   - For transactional changes to base-game state, separate prepare, commit, and rollback. Start the restoration scope
     before the first destructive mutation; keep every fallible synchronization, spawn, RPC, and commit operation inside
     it; and restore the exact original values plus remove prepared/spawned residue on failure. A swallowed outer
     exception is not a substitute for rollback.
   - Treat resource acquisition as a transaction too. If allocation or instantiation succeeds and later validation in
     the same prepare step fails, return an owned cleanup handle or clean up internally before throwing. Fault tests
     must cover failure after each acquired resource, not only before a stage begins.
   - Before patching a networked method, record its boundary ledger for every relevant path: execution role,
     send/server/receive stage, authoritative arguments, synchronously mutated fields, and deferred RPC effects. Select
     the hook and state source from that ledger; never read a mirrored field in a postfix unless the call graph proves
     it is updated before the hook. Cover host-local and remote-client paths when both exist.
   - For a shared RPC/event boundary, enumerate every baseline caller and relevant mod-extensible caller class. Preserve
     an explicit transaction-origin signal or paired hook so a state synchronization callback is not treated as proof
     that the intended domain action occurred. Negative provenance paths must leave base-game state unchanged.
   - Validate the real Harmony boundary, not only extracted Core policy. Invoke the actual prefix/postfix entry points
     in the target version's call order for every host and remote path. Use deliberately different method-argument,
     pre-call field, post-call field, and deferred-receive values so an incorrect authority source or provenance window
     cannot pass through accidental equality.
   - Make the boundary harness reproduce the base method's actual mutations and omissions. Do not manually update a fake
     field at a point where the inspected game method leaves it stale. Assert each intermediate sentinel before invoking
     the patch so the test proves timing rather than merely reaching the expected final value.
   - Preserve protocol indexes, catalog/list indexes, network IDs, and stable domain IDs as separately named or typed
     values through preparation and application. Hash, persist, log, or serialize only the identity named by the product
     contract; never relabel a collection index as a domain ID.
   - Choose prefix or postfix from lifecycle semantics. Observe completion-dependent state in a postfix unless pre-call
     mutation is required; if the base method is skipped or throws, do not report or persist completion that did not
     occur.
   - For every lifecycle predicate, enumerate the named positive states and immediately adjacent negative transitions
     such as loading, departing, travelling, resetting, and unavailable state. Build and verify a truth table rather
     than using a broad proxy such as `not in orbit` when the product names a narrower state.
   - Treat game API calls, patches, serialized names, and timing assumptions as compatibility-sensitive. Preserve the
     exact tested game baseline and request runtime evidence when a code-only review cannot establish it.
