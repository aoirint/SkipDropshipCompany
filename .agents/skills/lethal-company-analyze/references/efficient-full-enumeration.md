# Efficient exhaustive enumeration

Optimize the number of semantic full-domain passes, not the seed domain. A projected or fused scan over every certified row remains exhaustive; a prefix, sample, or unproved cache does not.

## Freeze a dependency DAG

1. Represent evidence extraction, seeded primitive state, endpoint projections, exact reductions, statistical models, figures, and reports as nodes with machine-readable parent hashes.
2. Give every endpoint a field-level dependency list. Decide reuse by walking those dependencies, not by comparing filenames or broad script hashes.
3. Find the maximal shared seeded primitive state across the requested endpoints. Traverse its RNG path once per semantic scenario, then fan out separately contracted endpoint projections. Keep branch-specific RNG suffixes separate.
4. Choose a materialization frontier before production:
   - stream exact mergeable reducers only when the questions are fixed;
   - store a narrow seed-state spine when related endpoints will likely be added;
   - store wide row/item state only when later exact derivation actually needs it.
5. Store exact reusable summaries alongside each shard when useful: integer histograms, categorical contingencies, supports, seed moments, and smallest-`k` distinct witness candidates. Do not substitute a lossy summary for a field required by a later endpoint.

When a new analysis is exactly derivable from a certified spine, scan the complete parent with column projection and compute all new reductions in one pass. When a required primitive is absent, enumerate only an independently derivable extension if its RNG and population semantics permit it; otherwise make a new semantic seal and full run.

## Separate semantic and orchestration identity

Keep the seeded kernel and endpoint projections outside the CLI, scheduler, progress logger, path guard, and report builder. Seal two identities:

- `semantic_fingerprint`: transitive computational closure, runtime/compiler and lock, raw/source inputs, effective serialized values, domain and strata, contracts, scenarios, complete schema, canonical encoding, and reduction rules;
- `orchestration_fingerprint`: CLI/wrapper, worker and chunk schedule, logging, checkpoint policy, and path/no-clobber guards.

Key every numeric shard by the semantic fingerprint and its canonical range. An orchestration-only change may reuse a shard only after proving unchanged kernel arguments and closure, retaining the true producer identity, and revalidating the shard from bytes. Record a compatibility certificate rather than pretending the new wrapper produced old bytes. Any uncertainty fails closed.

Invalidate only descendants in the DAG. Styling or narrative changes rebuild the report; reducer changes rebuild reductions and descendants; endpoint or kernel changes rebuild their numeric descendants; build inputs, RNG semantics, or population changes require a new semantic root.

## Pilot, partition, and resume

Run pilots only in explicit non-production namespaces. Measure multiple chunk widths and worker counts for seeds/second, peak RSS, compressed bytes/seed-stratum, reducer time, checkpoint/hash overhead, and restart waste. Size the run from measured throughput and disk rather than a guessed shard count.

Freeze canonical half-open ranges after the pilot. Choose a target shard duration that balances failure loss against file/hash/open overhead; retain the measured decision record. Stream bounded row groups, cap workers by memory and sustained storage bandwidth, publish data plus sidecar through temp paths and atomic rename, and merge strictly by canonical ordinal.

On resume, reconstruct—not merely read—the semantic fingerprint. Rehash the part, require the complete schema and exact key/range arrays, verify non-null/type rules, per-stratum contributions, semantic invariants, and sidecar identity, then rebuild the ordered root. Quarantine mismatches by rename.

Before the expensive pass, require small fixtures for contract/schema parsing, scalar JSON types, CLI and canonical-path guards, partial-output rejection, partition closure, atomic publication, rare RNG fallback, threshold inclusivity, float precision, draw order, and every planned mutation. Freeze their passing code closures before production.

## Fuse exact reductions

Project only required columns from the certified spine. Compute shared row derivations once and update all requested histograms, prediction tables, support panels, and mapping universes together. Use exact mergeable accumulators so worker scheduling cannot change semantics:

- integer counts/sums and categorical contingencies merge by addition;
- minima/maxima merge by their exact operators;
- smallest-`k` distinct witnesses merge as `sorted(unique(left ∪ right))[:k]`;
- ordered semantic roots merge only in frozen chunk order.

Keep a reducer registry listing every report-consumed estimand. A new report row must either be derived from a certified accumulator or trigger the appropriate complete-parent reduction before publication.

## Avoid redundant witness passes

During both production enumeration and the independent complete-domain expectation pass, accumulate per mapping key/output: contribution support, distinct population-unit/seed support, and chunk-local smallest-`k` distinct seeds. Merge those accumulators exactly and verify bidirectional singleton closure. Because every seed was independently recomputed, the merged seeds are globally minimal and shortage support is complete without a third domain scan.

Use a separate scalar or differently vectorized witness rescan only when the complete independent pass did not emit seed-resolved mapping identity/applicability, when its accumulator cannot be audited independently of production logic, or when a higher-assurance contract explicitly requires a third implementation. A performance prefix never proves minimality or shortage.

## Split independent verification phases

Keep the independent expectation engine/entry point and the production comparator/parser as separately hashed closures.

1. Phase 1 reads only frozen semantic/source inputs and declarative contracts, recomputes the complete domain and every registered estimand, then publishes a content-addressed seal.
2. Phase 2 binds that phase-1 seal, opens production artifacts, and performs exact comparison. It may recompute expected shards with the unchanged sealed expectation engine or consume persisted expected shards, whichever frozen CPU/disk plan is cheaper.

A phase-2 parser, formatting, or production-reader fix must not force phase 1 to rerun when the sealed phase-1 code closure, inputs, domain, encoding, estimands, and output bytes remain identical. Preserve the failed phase-2 attempt and issue a new comparator child. Rerun phase 1 whenever any of those identities changed or no valid pre-read seal exists.

Construct each phase's isolated artifact root from a fixed allowlist and the authenticated manifest's exact ordered part paths. Start from an empty root, reject unexpected files, and never use a directory glob as the authority for the certified universe. Publish the construction command and resulting inventory so another receiver can reproduce the same phase boundary.

## Seal the handoff

Publish immutable semantic, numeric, verification, analysis, report-artifact, and receiver-handoff stages. Document clean/full, extension-only, analysis-only, and report-only modes as executable command sequences with complete no-clobber archive lists. Receiver verification must rehash direct and indirect bytes, ordered shard universes, sidecars, phase seals, compatibility certificates, portable report dependencies, and recursively referenced prior handoffs from a relocated workspace root.
