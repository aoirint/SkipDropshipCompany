# Evidence policy

## Source order

Prefer sources in this order while preserving conflicts:

1. Runtime observation from a frozen, reproducible build and observation protocol.
2. Build-local decompiled code plus the serialized assets it references.
3. Build-local code or assets alone.
4. Official version notes.
5. Prior local analyses with source hashes.
6. Community material as a lead, not final proof.

Do not let a higher-ranked source erase a conflict. Explain why the sources describe different phases, clients,
versions, or object sets.

## Claim classes

- `direct_static`: explicit in target-build code or serialized data.
- `exact_derived`: deterministic result exact over the claim's complete declared domain, including a single declared
  case, with its implementation and inputs verified. Use it for an actual-runtime endpoint only after runtime
  correspondence, applicability, execution path, and every runtime-dependent input are established by claim-appropriate
  evidence.
- `counterfactual_model`: deterministic under stated assumptions not established as runtime, including known-different
  assumptions and named sensitivity scenarios. Keep the corresponding actual endpoint `unknown_runtime` until runtime
  correspondence and every runtime-dependent input are established by claim-appropriate observation and/or exact
  derivation.
- `sample_estimate`: estimate from a frozen sample; record design and uncertainty meaning.
- `runtime_observed`: measured under a named profile/window.
- `unknown_runtime`: required runtime state is absent.

Track computational completion separately from claim class. `complete_exact` may describe exhaustive completion of a
declared static or counterfactual model; it does not promote that model to an actual runtime fact. Store fields such as
enumeration status, evidence class, runtime status, and applicability independently rather than overloading one status
label.

## Evidence ledger fields

Record claim ID, statement, build, class, source path, locator, source hash, derivation artifact, population key,
endpoint status, and caveat. Use absolute paths only in source manifests; use workspace-relative frozen paths in
portable reports.

## Conflict checks

- Compare C# field defaults with scene, prefab, and ScriptableObject serialization.
- Distinguish code presence from reachability and execution.
- Distinguish host state, client state, pre-sync state, and post-sync state.
- Distinguish planned objects from successfully spawned and active objects.
- Keep HUD, Terminal, server totals, quota values, and item `scrapValue` as separate endpoints unless code proves
  equality.
- For a gameplay loop or save-state claim, freeze the complete call chain from the user action through wrappers to the
  state mutation. A callee excerpt without the caller/wrapper does not establish that the mutation occurs on the claimed
  path.
- Treat repeated-play strategy assumptions as evidence-bearing claims: whether a reload produces a new seed, whether
  draws are independent, whether counters are per-level or global, and whether solo and multiplayer follow the same save
  branch.
