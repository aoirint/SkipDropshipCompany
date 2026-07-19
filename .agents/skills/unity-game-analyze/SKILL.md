---
name: unity-game-analyze
description: >-
  Trace version-specific Unity game behavior through decompiled managed code,
  scenes, prefabs, prefab variants, ScriptableObjects, bundles, serialized
  references, and runtime mutation paths. Use when locating an implementation,
  reconciling code defaults with asset overrides, proving callback or object
  reachability, comparing Unity builds, or preparing code-and-asset evidence
  for a game-specific analysis or mod integration. Pair with a domain analysis
  skill for gameplay interpretation and with a mod quality skill for hook and
  implementation decisions.
---

# Unity Game Analysis

## Goals

- Reconstruct a behavior from the user-visible surface to its code, serialized data, load path,
  runtime mutations, and observation window.
- Preserve exact build identity and source provenance so evidence from different versions cannot
  be mixed accidentally.
- Distinguish code presence, effective serialized state, runtime reachability, and observed behavior.
- Produce a bounded evidence handoff that another analysis or implementation Skill can consume.

## Responsibility boundaries

Use this Skill to answer where a Unity behavior comes from and what the inspected build proves.
It owns decompiled-code tracing, Unity serialization identity and precedence, asset/load-path
resolution, lifecycle and callback reachability, cross-build evidence comparison, and static/runtime
evidence boundaries.

Use a game-specific Skill such as `lethal-company-analyze` to define gameplay endpoints, reproduce
game-specific RNG, enumerate seeds, estimate probabilities, and interpret results for players. Use
`bepinex-mono-mod-quality-check` to choose and verify patches, Core/Interop boundaries, transactions,
dependencies, packaging, runtime compatibility, and releases. The consuming Skill must not silently
reinterpret an unresolved Unity trace as a gameplay fact or a safe hook.

For a compound request, complete the Unity trace first, hand its established facts to the
game-specific analysis second, and make the mod integration decision last. If no suitable
game-specific Skill is available, preserve the evidence handoff and leave the gameplay conclusion
unresolved instead of absorbing domain interpretation into this Skill.

This Skill does not obtain or redistribute proprietary game files. Work only with evidence roots
the user has authorized. Do not paste substantial decompiled source into a report; record narrow
locators, signatures, identities, and hashes sufficient to repeat the trace.

## Workflow

1. Freeze the target build.
   - Record the product, platform, game/build/manifest identifier, scripting backend, managed and
     asset evidence roots, Unity and managed-runtime/library identity when relevant,
     extraction/decompiler tool and version when known, and evidence date.
   - Keep each build in a separate inventory. Run `scripts/inventory_build.py` when stable file
     hashes are needed. Never use a same-named file from another build to fill a gap.
   - Record missing or transformed evidence. An exported YAML tree is not automatically a
     byte-faithful representation of its source bundle.
2. Define the behavior boundary.
   - State the user-visible event or value, execution role, object population, lifecycle phase, and
     observation window to explain.
   - List the questions the trace must settle: origin, caller, effective value, object identity,
     load/reachability path, mutation, synchronization, or version change.
3. Trace decompiled code.
   - Read [code-tracing.md](references/code-tracing.md). Start from the closest verified surface and
     follow callers and callees through the statement that computes, mutates, sends, or displays the
     value. Include wrappers and failure/early-return branches.
   - Resolve virtual/interface dispatch, delegates/events, coroutines and generated state machines,
     reflection, RPC wrappers, Unity lifecycle messages, and serialized UnityEvents when applicable.
     Do not treat a method name or decompiler cross-reference list as proof of execution.
4. Resolve serialized assets.
   - Read [serialized-assets.md](references/serialized-assets.md). Join scripts and objects by GUID,
     fileID, type, and owner context rather than names alone.
   - Compose base prefab, nested prefab, variant, scene-instance, added/removed component, and
     property modifications in precedence order. Then trace runtime instantiation and assignments.
   - Keep code initializer, effective serialized value, and later runtime value as separate facts.
5. Prove applicability.
   - Trace scene/build-list, Resources, Addressables, bundle, factory, network-spawn, or other load
     routes to the concrete object. Check object/component existence, active/enabled state where
     relevant, role/authority gates, timing, and destruction/replacement.
   - Distinguish `present`, `reachable`, `executed`, and `runtime-observed`. A disabled object may
     still contribute serialized data; an effective value may still be unreachable in the endpoint.
6. Reconcile and compare evidence.
   - Read [evidence-handoff.md](references/evidence-handoff.md). Preserve conflicts instead of
     selecting whichever source matches the hypothesis.
   - For a build comparison, complete the trace independently in each build, then compare stable
     semantic roles and verified identities. Do not align assets or members by filename, local
     fileID, source line, or decompiler-generated name alone.
7. Verify the closure.
   - Rewalk the trace from the visible endpoint to every cited code and asset locator. Search for
     competing writers, overrides, variants, event bindings, load paths, and role-specific branches.
   - Use runtime observation when static evidence cannot establish dynamic ordering, object
     enumeration, stateful Unity APIs, network arrival, reflection success, or generated content.
     Keep the unresolved result explicitly unknown until then.
8. Hand off the result.
   - Report the target build, behavior boundary, ordered trace, effective-value composition,
     reachability result, conflicts, unknowns, and exact evidence locators/hashes.
   - State what the consuming game-analysis or mod-quality Skill may rely on and what it must still
     establish. A candidate integration surface is only an evidence record containing its exact
     signature, timing, ownership, compatibility scope, and evidence state. Do not turn it or a
     static code path into an implementation recommendation inside this Skill.

## When evidence is unavailable

Do not speculate from a method name, field initializer, remembered game version, or neighboring
repository. State that the trace is unresolved and request the minimum authorized evidence package:
exact build/platform/backend and managed-runtime identity, managed assemblies, the closest verified
behavior surface, relevant asset sources and metadata, extraction-tool identity, and any runtime
observation required for dynamic state. Return a bounded next-step and handoff contract; leave hook
selection, gameplay interpretation, and probability uncalculated until their required evidence and
consumer Skills are available.

## Completion checklist

- Build and evidence roots are identified and not mixed across versions.
- The trace reaches the actual compute/mutation/use statement through all relevant wrappers.
- Script/object identity uses GUID/fileID/type/owner evidence rather than a name guess.
- Serialized precedence and runtime mutation are represented separately.
- Load path, lifecycle, role, and observation window are explicit.
- Static presence, reachability, execution, and runtime observation are not conflated.
- Conflicts and negative findings state the searched scope.
- The handoff says which downstream conclusions are supported and which remain unknown.
