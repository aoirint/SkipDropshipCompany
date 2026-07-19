# Exhaustive seeds and lineage

## Full-population contract

Freeze the inclusive seed domain, every declared population/comparison stratum, endpoint identities, scenario identities, canonical record encoding, and reduction order before production. For a claimed full enumeration:

- partition the domain into canonical disjoint half-open chunks;
- prove their union equals the declared domain with no gaps, overlaps, duplicates, or out-of-range seeds;
- store per-chunk and per declared population/comparison stratum (for example moon, level, profile, or build) contribution counts and ordered record digests, plus type-appropriate exact invariants: integer minima/maxima/sums/squared sums/XOR where meaningful, categorical contingencies, or canonical structured-record checks;
- compute a parent digest from the ordered chunk identities and digests;
- require reducer totals to equal worker totals and the expected population contribution count.

Use a second implementation that imports no production computational or canonical-encoding modules; share only frozen semantic/source inputs plus declarative contracts and schemas. Do not read production-derived intermediates, canonical records, contingencies, reducer outputs, or digests until the complete-domain independent phase is sealed. A chunk-interleaved loop that computes one expectation and immediately opens its production counterpart is not this two-phase protocol. Regenerate complete-domain results, type-appropriate invariants, and canonical digests independently; then use a second pass (recomputing chunks if necessary) for production comparison. Independently reproduce every declared reducer estimand and every reported metric/model row, not only raw-record equality or headline MAE. Make it reject targeted mutations such as seed offsets, serialized entry order, branch timing, draw counts, float precision, truncation, curve behavior, applicability rules, record encoding, or reduction order. A sample or prefix can validate performance but cannot certify coverage.

For huge finite-population reductions, distinguish semantic disagreement from floating-point reduction-order noise. Require exact equality for integer counts, supports, keys, canonical semantic digests, histograms, witnesses, and any exactly representable estimand. For a floating sum that can legitimately change with chunk or parallel reduction order, diagnose every affected reducer row before accepting a tolerance: preserve the rejected complete-domain phase seal, show that integer and non-target fields still agree, measure maximum absolute and relative error, then apply the smallest field-specific finite relative bound that covers the observed reduction error. Never use one blanket tolerance for the whole certificate, and never relax semantics, coverage, or integer fields to accommodate a floating discrepancy. Record the policy and diagnostic artifact in the final certificate and exercise it with rejection fixtures outside the bound.

For weighted selection, freeze the entire edge path, not just the ordinary cumulative-probability loop: binary precision of the random target, division and accumulator precision, comparison inclusivity, treatment of nonpositive weights, and the decompiled fallback after cumulative rounding. If the game consumes another RNG draw when no cumulative bin reaches the target, the independent verifier must do the same. Include a fixture that actually reaches this fallback; otherwise a verifier can agree on common rows while shifting every later draw in the rare boundary row.

## Deterministic mappings and witnesses

Freeze the mapping universe in the endpoint contract, including every declared dimension that belongs to the estimand (for example moon, build, level, endpoint, type, interior, profile, or counterfactual scenario). Freeze the target predicate byte-for-byte too—especially inclusive versus strict thresholds—and make every producer, verifier, reporter, and sealer compare it with the parent endpoint definition rather than merely trusting the overlay hash. Do not silently narrow “all mappings” to a headline scenario; if the report shows a headline subset, link the complete-universe table explicitly.

Derive a 100% mapping only from a complete-domain proof, such as an exhaustive finite-population contingency or an independently checked algebraic/formal derivation, never from cross-fit predictions. Check bidirectional closure over the declared mapping universe:

1. supported singleton cells minus emitted mappings is empty;
2. emitted mappings minus supported singleton cells is empty.

For every emitted mapping, use the complete-domain proof to verify contribution/cell support and distinct population-unit support; also verify distinct-seed support when seed is a separate clustering or witness unit. Report differing values or explicitly certify their equality. When seed witnesses are requested, attach the numerically smallest `min(requested count, distinct-seed support)` distinct seeds; use a requested count of three when the user asks for examples but gives no count. Disclose mappings whose distinct-seed support cannot supply the requested count instead of omitting them or duplicating seeds.

Prefer collecting exact support and chunk-local smallest-`k` candidates inside both the production pass and the implementation-isolated complete-domain expectation pass, then merge candidates as sorted distinct top-`k`. When that independent pass recomputes seed-resolved mapping identity, output, and applicability for the entire domain, exact merge plus complete support proves witness minimality and shortage without a third full scan. Follow [efficient-full-enumeration.md](efficient-full-enumeration.md) for the accumulator and phase split.

Use a separate scalar or differently vectorized witness rescan when the complete independent pass lacks seed-resolved inputs, its witness path is not independently auditable, or the contract explicitly requires a third implementation. In that case, scan from the domain minimum through the last fulfilled witness and through the domain maximum for shortages. Prefer physical isolation in a dedicated module whose imports and namespace contain no production/chunk simulator or canonical helpers; seal the module and lock hashes and audit its complete import/function closure before execution. Execute the exact audited source bytes with fixed compile parameters in a fresh namespace, or otherwise cryptographically bind the executed code object—ordinary import machinery may select an unsealed timestamp-compatible bytecode cache. If shared-module policy isolation is unavoidable, resolve module-level and function-local callable value flow (including attribute/subscript aliases, callable factories, and higher-order callbacks), fail closed on unknown callable provenance, and include direct, alias, callback, factory, and malicious-bytecode mutation fixtures. A fixed `isolated: true` field is not an audit. Include actual-runtime mappings only when runtime correspondence, applicability, execution path, and every runtime-dependent input are established over the mapping's complete declared domain under the evidence policy's promotion boundary.

## Immutable stages

Before implementation, freeze a machine-readable stage/handoff contract: schema version, canonical encoding, self-hash algorithm, permitted stage identities and parent relations, workspace-relative path syntax and root policy, terminal-stage rules, and receiver acceptance checks. Define the external trust anchor too: publish the terminal root digest through a separately trusted channel, or use an authenticated signature with signer identity and verification policy. Content addressing proves internal integrity after a root is trusted; it does not authenticate a wholly replaced but internally consistent snapshot. Separate semantic inputs, production certificates, processed analysis, report/media, and handoff into content-addressed stages. Bind every stage to its parent hash and artifact byte hashes. Revalidate parent artifacts before sealing a child. Keep failed or superseded attempts by rename; never overwrite a sealed run. Keep `report_artifact` distinct from `report_visual_handoff`: verify and seal report bytes first, perform any warranted fresh review, then seal the QA inventory, inspection attestation, portable dependencies, and receiver instructions as the handoff child.

A handoff seal also needs a receiver-side, read-only verification mode. Verify the terminal seal against the external trust anchor, recompute the seal self-hash, stage hashes and parent links, then rehash every directly and indirectly enumerated artifact from the bytes being received (including partition sidecars and resumable bundles). Recursively resolve any `previous_handoff` by workspace-relative path, require its declared hash and terminal-stage binding, verify its complete artifact ancestry, and reject cycles or paths outside the supplied workspace. Freeze a strict portable-path grammar and collision policy; reject absolute/device/extended paths, alternate data streams, ambiguous separators, case-fold or normalization collisions, non-regular files, and symlinks, junctions, reparse points, archive entries, or hardlink policies that can escape the receiver root or alias undeclared bytes. Resolve components relative to an opened root handle where supported, hash through validated handles, and detect mutation by revalidating identity/metadata after hashing. Extract archives into a new root with traversal and link rejection before verification. Internal JSON self-consistency alone does not prove that the referenced artifact bytes still exist or are unchanged, and is insufficient evidence for report-only ancestry.

Test that chain through the production receiver entry point: construct a real descendant seal, relocate the complete snapshot under a different receiver root, require PASS there, corrupt an ancestor seal or artifact, and require rejection. A synthetic dictionary selector or a unit test of the raw path resolver alone does not exercise recursive handoff verification.

Reserve canonical enumeration directories, phase-seal paths, and final-certificate paths for complete-domain production PASS runs (while allowing an explicitly defined resumable full-domain part limit). A prefix, mutation check, or independent-only diagnostic must require explicit non-production output and manifest paths and must be rejected before expensive scanning if it targets any canonical production path.

For every resumable part or bundle, independently reconstruct its generation fingerprint from the declared parent bytes, contracts, producer and lock, raw inputs, complete schema, scenario/profile definitions, and support rules; do not accept a self-sealed fingerprint merely because it is well-formed. When an additive certificate claims that a parent was independently certified, bind the exact parent certificate, its phase seal/content hash, and its complete part universe/root into the child phase seal and final certificate. Require per-part identity/range/row and independent semantic-record digest equality between the parent's expected and comparison passes, plus an ordered semantic root on both sides, then enforce that cross-stage identity in the handoff seal. Normalize against the real producer schema—derive deterministic fields from frozen dimensions when they are not stored rather than letting synthetic fixtures invent extra keys.

Document three reproduction modes when applicable:

- a clean full run from a new semantic seal;
- an extension-only run against an unchanged sealed parent;
- a report-only rebuild against unchanged numeric parents.

When a report-only rebuild compares ancestry, name the exact numeric parent stages to preserve. Do not infer them with positional slices such as “all stages except the last,” because adding a separate handoff stage silently changes that set. Store prior-seal references as workspace-relative paths and reject producer-absolute or out-of-workspace references. If reproduction documentation, contracts, or other semantic inputs changed, create a new semantic chain or an explicit sealed overlay instead of claiming unchanged report-only ancestry.

## Additive work after a sealed run

Do not make changed live files appear to have belonged to the old run. Choose one of two paths:

- require a new semantic seal and full rerun when population definitions, core endpoint meaning, frozen game inputs, RNG mechanics, enumeration code, or core analysis logic changed;
- create a separately sealed additive extension when the old numeric artifacts remain byte-identical and the new endpoint can be enumerated independently.

For an additive extension, create a machine-readable overlay that binds the old semantic/certificate/analysis hashes and lists every superseded sealed path with its old hash, accepted new hash, role, and reason. Also hash every new contract, evidence file, script, lock, completion certificate, independent verifier, processed output, and review artifact. Fail if:

- a changed base path is undeclared;
- an overlay old hash differs from the base seal;
- a current or archived new hash differs from the overlay;
- an undeclared core computational input changed;
- a downstream stage omits the overlay or any required extension parent.

Avoid both extremes of recursive live-path verification: rejecting a legitimate declared extension because a report document changed, or blanket-skipping changed paths and allowing later drift. Compare unchanged base paths to the base seal, declared changed paths to exact overlay old/new pairs, and new paths to the extension allowlist. Seal the overlay into the final report and handoff chain.
