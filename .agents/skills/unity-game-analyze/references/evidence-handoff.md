# Evidence and Handoff

## Evidence ladder

Keep these states separate for every claim:

1. `present`: code or a serialized record exists in the target-build evidence.
2. `effective_static`: verified composition produces the stated member/object/value.
3. `reachable`: a verified load, binding, lifecycle, role, and branch path can reach it.
4. `executed`: runtime evidence, or a complete static proof for a declared deterministic window,
   establishes that the path ran. Record which proof form applies.
5. `runtime_observed`: a named runtime protocol directly observed the stated value or behavior.
6. `unknown_runtime`: dynamic state prevents promotion beyond static evidence.

An exact static composition or statically proved execution is not automatically a runtime
observation. Runtime observation also does not erase a conflicting static result; it may describe
another build, role, phase, object, or configuration.

## Trace ledger

Record one row per material claim or edge with:

- claim/edge ID and statement;
- build/platform/backend;
- code assembly, type, signature, locator, and hash;
- asset source, GUID/fileID or path ID, property path, owner context, and hash;
- composition layer and effective value;
- load/binding/lifecycle/role evidence;
- evidence state from the ladder;
- conflicts, extraction gaps, runtime requirements, and downstream applicability.

Use absolute local paths only in a private source manifest. Use portable workspace-relative paths
in the handoff. Keep narrow excerpts within applicable legal and repository policy; prefer locators
and hashes over copied decompiled bodies.

## Resolve conflicts without flattening them

Compare code initializers with serialized values, base prefabs with variants and scene instances,
send-stage state with receive-stage state, intended paths with reachable paths, and extracted assets
with runtime observations. Preserve both sides until a verified phase/version/identity distinction
explains the conflict.

For negative findings, attach the complete bounded search scope and tool limitations. For ambiguous
decompilation or extraction, record what representation would resolve it.

## Compare builds safely

Inventory and analyze each build independently. After both closures are complete, align by semantic
role plus verified type/member/asset identity. Record additions, removals, signature changes,
serialized changes, load-path changes, and behavior changes separately.

Do not align solely by decompiler line number, generated state-machine name, local fileID, export
filename, object name, or directory position. A stable GUID can still point to changed bytes; a
changed GUID can still represent a migrated semantic role. Report both identity and content evidence.

## Downstream handoff

Give the consumer:

- target-build inventory and extraction limitations;
- behavior boundary and ordered code/asset/runtime trace;
- effective serialized state with full composition chain;
- reachability and evidence state for every conclusion;
- conflicts, unknowns, and exact runtime observations still required;
- candidate integration surfaces as evidence, not as approved hook recommendations.

For a game-mechanics analysis, state which values, orderings, RNG calls, object populations, and
runtime inputs are established. For a mod-quality review, state which callback timing, ownership,
identity, serialization, and compatibility facts are established. The consumer remains responsible
for its own endpoint or implementation decision.
