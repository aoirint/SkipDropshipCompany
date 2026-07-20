---
name: lethal-company-analyze
description: Investigate version-specific Lethal Company mechanics using verified Unity-build evidence, seeded RNG, exhaustive or sampled computation, and runtime observation; produce reproducible, bounded gameplay reports. Use for mechanics questions, full-seed enumeration, sealed-result reuse, Terminal or HUD endpoint interpretation, version comparisons, statistical prediction, or playable-strategy evaluation. Use unity-game-analyze for the underlying decompiled-code and serialized-asset trace.
---

# Lethal Company Analysis

## Establish scope

1. Record the exact game version, Steam manifest/build identifier, platform, mod state, evidence handoff, and evidence date.
2. Use `unity-game-analyze` when the question requires tracing decompiled code, serialized assets, prefab/scene overrides, load paths, or callback reachability. Consume its target-build inventory and bounded evidence handoff instead of recreating Unity tracing rules here.
3. Keep evidence from different builds separate. Complete the Unity trace and mechanics analysis independently in each build before comparing them.
4. For a game-version upgrade, define a target-build identity tuple before
   drawing a compatibility conclusion: displayed game version, Steam
   manifest/build identifier, managed-code inventory/hash, and exported-asset
   inventory/hash. Accept decompiled managed code and serialized assets only
   when they identify the same target build; otherwise mark the affected
   mechanic and compatibility claim `blocked`, rather than combining evidence
   across builds.

## Build the evidence model

1. Define the player-visible behavior, decision, population, and observation window before interpreting the Unity evidence.
2. Verify that the `unity-game-analyze` handoff covers every code path, effective serialized value, object identity, load/reachability condition, role, and runtime unknown needed by the mechanic. Route missing trace evidence back to that Skill; do not fill it with a gameplay model.
3. Read [rng-and-runtime-modeling.md](references/rng-and-runtime-modeling.md) when the mechanic uses `System.Random`, `UnityEngine.Random`, object enumeration, spawning, NavMesh, networking, synchronization, or before/after UI observations.
4. Read [evidence-policy.md](references/evidence-policy.md) before designing the mechanics evidence ledger, classifying claims, or resolving conflicting sources.
5. Record claim-level locators, source handoff hashes, and derivation hashes. Use the policy's exact classes: `direct_static`, `exact_derived`, `counterfactual_model`, `sample_estimate`, `runtime_observed`, and `unknown_runtime`.

## Define endpoints before computation

1. Write a machine-readable endpoint contract before production.
2. Separate actual runtime endpoints from exact static/RNG endpoints and from analytic or counterfactual proxies. Never fill an unknown runtime endpoint with a proxy.
3. Define the population key, observation window, inclusion/exclusion predicates, units, rounding, thresholds, and applicability for every endpoint.
4. Name proxies by what they compute. Put `proxy`, `counterfactual`, or `static` in tables, captions, and artifact schemas.
5. Freeze analysis thresholds, binning, tie rules, sparse-cell fallbacks, folds, metrics, and uncertainty interpretation before inspecting production outcomes.

## Compute reproducibly

1. Prefer Python scripts with PEP 723 metadata. Set `[tool.uv] exclude-newer = "P7D"`, pin dependencies, and generate an adjacent uv lockfile.
2. Preserve source artifacts and prior runs. Use no-clobber outputs and archive by rename instead of overwriting.
3. Read [efficient-full-enumeration.md](references/efficient-full-enumeration.md) before designing a large or repeated exhaustive run. Freeze the dependency DAG, reusable materialization frontier, layered fingerprints, preflight gates, and verifier phase boundary before production.
4. Read [exhaustive-seeds-and-lineage.md](references/exhaustive-seeds-and-lineage.md) for full seed enumeration, deterministic mappings or witnesses, immutable stage seals, or an additive analysis requested after a prior run was sealed.
5. For full seed enumeration, prove domain endpoints, gaps, overlaps, duplicates, contribution counts for every declared population stratum, type-appropriate exact invariants, and parent hashes. Do not call a prefix or convenience sample "all seeds."
6. Traverse each seed-state primitive once per semantic scenario when possible, then fan out separately contracted endpoints and exact mergeable reducers. Reuse a sealed full-domain parent only when the new endpoint is exactly derivable from its certified fields and provenance; scanning that complete parent is still exhaustive, while sampling it is not.
7. Bind every resumable part to the semantic kernel closure, adjacent lock, endpoint contract, raw inputs, complete schema, scenario definitions, and domain. Record orchestration identity separately. Publish parts and sidecars atomically; validate reused parts semantically before including them in a new ordered root manifest.
8. Freeze and mutation-test parsers, schemas, path guards, rare RNG branches, and phase barriers on isolated fixtures before starting an expensive complete-domain pass.
9. When full item-level enumeration is infeasible, apply one frozen domain-separated selection rule consistently to every declared comparison stratum. Share sampled population units across strata only when the comparison design supports matched sampling, and disclose the assumption. Seal a hashed population-frame and sample-membership manifest with canonical frame schema/encoding. Have the isolated verifier reconstruct the frame from frozen semantic/source inputs, derive membership with the exact selection-rule encoding, namespace, tie, and replacement semantics, and read neither production frame nor membership manifest until comparison. Then require frame-hash equality and bidirectional membership set/multiset equality. Verify expected selected-unit and contribution counts per stratum and recompute inclusion probabilities/weights for unequal-probability designs. Never downgrade a runtime-unknown endpoint into a sampled "actual" endpoint.
10. Keep independent verification implementation-isolated: import no production computational or canonical-encoding modules, and share only frozen semantic/source inputs plus declarative contracts and schemas. Do not consume production-derived intermediates, canonical records, contingencies, reducer outputs, or digests until comparison after independent recomputation.
11. Reimplement fragile RNG logic independently. Test exclusive maxima, float32 accumulation, integer truncation, offsets, event branches, serialized multipliers, and RNG consumption order with mutation fixtures.
12. Keep training/test partitions at the population-unit level across all declared comparison strata (for example moons, builds, levels, profiles, or variants). Fit bins, probabilities, modes, medians, intervals, sparse support, and fallbacks on training data only.

## Report and validate

1. Read [reporting-and-review.md](references/reporting-and-review.md) before building a formal HTML/PDF report.
2. Lead with the player-visible finding, realistic decision horizon, and limitations; put exhaustive-computation detail after the gameplay interpretation. State what can and cannot be inferred near every gameplay-facing result.
3. Prefer conditional heatmaps, support panels, and 80%/95%/100% certainty views over generic box plots when the question is state identification.
4. Generate HTML and A4 portrait PDF from one narrative/claim source. Number figures in reader-visible display order, close captions and in-text references mechanically, run separate report-closure checks for claims and figure hashes, then render and inspect every PDF page. Bind the inspection set with a hashed visual-QA inventory and a separate attestation.
5. Promote a reset, reroll, SCAN, or HUD result into an actionable gameplay strategy only after runtime correspondence, the concrete repeat workflow, fresh-state behavior, independence assumptions, save-state side effects, and the observation window are established. Otherwise present a bounded candidate strategy or conditional frequency calculation.
6. Calibrate review cost to risk. Use a fresh blank-context reviewer for new mechanics, endpoint meanings, computations, evidence-class changes, gameplay recommendations, or broad narrative/visual rewrites. Do not spend a subagent pass on a localized mechanically verifiable edit such as a heading level, label, link, or known table placement when deterministic closure checks plus targeted rendering can decide it. After a material correction, use a new reviewer and iterate until no useful correction remains.
7. Preserve scripts, locks, evidence, raw/intermediate/processed data, review records, and executable top-to-bottom reproduction commands with the deliverable. Make each rebuild mode's no-clobber archive list complete, construct isolated verifier roots explicitly from sealed manifests, and finish with receiver-side read-only verification of the complete recursive handoff chain.
