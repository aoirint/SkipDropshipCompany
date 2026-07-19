# Reporting and review

## Artifact layout

Use `evidence/`, `data/raw/`, `data/intermediate/`, `data/processed/`, `docs/`, `report/`, and `script/`. Keep a source manifest and data dictionary. Store stage certificates where large computations must be sealed.

Distinguish a clean full rerun, an additive extension, and a report-only rebuild. Before rebuilding, publish a classification manifest for every changed figure/table/media artifact: analysis-owned when its data, estimand, filtering, transform, axes semantics, or statistical encoding changes; report-owned only when it is an exact presentation derivative of unchanged sealed analysis data. For each report-owned derivative, freeze a sealed transformation contract and either regenerate it independently for byte/semantic comparison or restrict it to a narrow mechanically verified display transform. A label alone does not prove the classification. For a report-only change, keep certified numeric parents byte-identical, archive the prior report and downstream seals by rename, restore analysis-owned media byte-for-byte, and rebuild only the report/handoff descendants. Do not rerun a full seed population merely because prose, headings, tables, or layout changed.

## Gameplay-facing interpretation

- Lead with what a player can observe, decide, and gain within a realistic number of attempts. Follow with the probability model and evidence boundary, then the exhaustive-computation details.
- For reset/reroll cases, define the actual population unit: landing, reload, new seed, day, moon, save, host, player count, weather, and observation window. Distinguish the number of landings from the number of retries after the first landing.
- Show practical horizons such as expected landings and the chance within 5 or 10 landings when the model supports them. State the independence/uniformity assumption next to the result.
- Before calling a conditional result a playable SCAN/HUD strategy, establish both mappings: internal predictor to actual displayed value, and the concrete quit/reload sequence to a genuinely new eligible state. Audit save counters, per-level state, mold or other persistence, multiplayer/solo branches, and any world changes caused by repetition.
- If either mapping is unverified, call the result a candidate strategy, conditional frequency, or promotion target for runtime observation. Avoid action verbs such as “use,” “target,” or “avoid” unless explicitly conditioned on a verified runtime profile.
- Keep planned/requested counts, instantiated objects, active objects, Terminal SCAN, HUD values, and recovered value totals separate. Name the exact one used in every heading, table, and claim.
- Use natural locale-aware terminology. Avoid ambiguous machine-translated counters (for example, a Japanese phrase that can mean calendar months when it means moon count), and define specialized count corrections where first introduced.

## Statistical presentation

- Separate finite-population exact descriptions from sample estimates and cross-fit prediction.
- Full enumeration removes population-unit sampling uncertainty only. Keep `counterfactual_model` as the evidence class, record enumeration status separately, and preserve runtime/model uncertainty.
- For weighted discrete outcomes, freeze an exact integer nearest-rank rule such as `ceil(q*N)` and report prediction-range coverage. Do not call a fully known finite-population range a confidence interval.
- Show support alongside conditional probabilities.
- Keep zero probability distinct from unsupported cells.
- For integer state identification, show modal accuracy and empirical 80%/95% intervals; list every 100% mapping in a machine-readable appendix.
- Keep full-sample 100% descriptions separate from deployable cross-fit rules. Audit support in every training fold and disclose fallback behavior.
- Preserve the full numeric bin grid in heatmaps, including globally unsupported bins as blank cells; never compress observed categories into equal-width numeric-looking columns. Choose adaptive major ticks from numeric bin values with a bounded label count for A4 readability, and state both the cell resolution and tick spacing.
- Build plotted lines from consecutive supported integer segments. Never connect two supported points across an unsupported integer gap; preserve supported zero as different from unsupported.
- If dense integer panels make exact annotations unreadable at A4 size, split the numeric range into ordered strips or move the values to a table. Do not solve density by shrinking labels below legibility.
- For conditional ribbons or lines over a discrete predictor, split each series into maximal contiguous supported segments before plotting. Never interpolate a median or prediction range across unsupported cells. Render a one-cell supported segment with an explicit marker and, when applicable, a vertical interval bar; a markerless line or `fill_between` call makes it visually indistinguishable from missing support. Bind the plotted segment endpoints/counts to the source table and make the report verifier reconstruct them independently.
- When deterministic mapping witnesses are included, present or explicitly link the complete declared-universe mapping table; never show a headline subset as though it were complete. Apply the witness-selection and minimality rules from the exhaustive-seed reference routed by `SKILL.md`.
- Separate a small representative-witness table from the complete mapping table. Make examples visibly subordinate to the complete-domain proof: examples support reproduction and visual checking, not the 100% inference. When both belong to one topic, use a parent section with explicit subsections rather than concatenating titles or presenting them as unrelated peers.
- Distinguish the number of unique predictor conditions from the number of predictor-by-outcome state rows. Do not alternate between those counts without labels.
- Never round a non-certain probability to `100%` or `100.0%`. Choose enough decimals to keep it visibly below 100, or use an explicit upper-bound notation.
- Keep tied modes as ties. Do not color or label a tied conditional distribution as one uniquely determined state.
- Fix probability axes to 0–1 unless a clearly marked zoom is necessary; disable scientific/offset formatting that can make probabilities appear negative or otherwise change their meaning.
- Report a baseline, paired improvement, and an uncertainty or stability measure whose interpretation matches the design.

## Verification depth

- Independently recompute report claims from canonical data rather than trusting production summaries. For seeded witnesses, keep the report verifier implementation-isolated from production RNG/model helpers and mutation-test the shared-implementation boundary.
- Verify every row and every declared column, not only group-first metadata. Close schema, column order, rule IDs, ranks, support, probabilities, population counts, corrections, scope/caveat strings, applicability-specific nulls, and smallest-seed ordering. Add negative fixtures for fields whose corruption would otherwise leave a PASS.
- Bind semantic presentation structure when it matters: section order, parent/subsection levels, complete-table row count, headers, figure numbering, in-text figure references, captions, and evidence links.
- Treat “unsupported” as different from supported probability zero, and reject line segments that cross unsupported integer gaps. Verify these properties from source tables, not by visual judgment alone.
- Freeze complete call paths for gameplay claims, including wrappers that connect an observed entrypoint to a saved state or RNG-consuming helper. A nearby method body is not evidence of reachability.

## Closure checks

Generate user-facing text, tables, claims, and captions from one structured source. Recompute headline claims separately from sealed canonical data as a report-closure check. Do not present this as independent endpoint/computation verification; apply the implementation-isolation rule in `SKILL.md` for that stronger claim. Verify HTML visible-text closure, PDF text inclusion, forbidden runtime assertions, figure set/hash closure, A4 portrait geometry, and evidence links.

Make the evidence ledger and report links close bidirectionally: every cited path must exist and match its recorded hash, and every headline claim must lead to an indexed canonical source. Separate analysis-source figures from report-only split or enlarged display figures and record the relationship explicitly.

For no-clobber pipelines, document full clean reruns separately from report-only and analysis-only rebuilds. Name every generated path that must be archived, keep prior runs by move rather than deletion, and link the reproduction guide and data dictionary from the report artifact table.

Treat reproduction prose as an executable interface. For every mode, give a single top-to-bottom command sequence containing every required producer, verifier, sealer, renderer, and receiver check; a command mentioned later in explanatory prose does not complete an earlier sequence. Audit the archive list against every path the invoked builders own, including registries, isolated roots, QA inventories, rendered pages, contact sheets, handoff dependencies, and downstream seals. Exercise the documented sequence from an empty output namespace when practical.

When independent verification requires a restricted filesystem view, document how to construct it rather than merely listing permitted files. Create an empty root, copy the fixed allowlist, and link or copy the exact ordered shard paths from the authenticated manifest—never a broad glob. Run the verifier with that root as its only artifact input, then record the observed inventory. If a later comparison phase needs a different view, construct and audit that view separately.

For resumable enumeration, embed a generation fingerprint in every part and atomic sidecar. Include the script, adjacent lock, endpoint contract, raw inputs, full schema, scenario/tier definitions, and parent hashes. On resume, recheck types, non-null values, exact key arrays, semantic invariants, part hashes, and the ordered part-root digest; range metadata alone is insufficient.

Render all PDF pages with a pinned renderer. Inspect every page for clipping, overflow, unreadable labels, misleading axes, broken glyphs, blank pages, orphaned headings or captions, and table-header/row splits. Record page and contact-sheet hashes in visual QA evidence.

After rendering, generate a no-clobber visual-QA inventory that binds the report PDF hash, render-manifest hash, and every page/contact-sheet relative path, size, and hash. Record human or agent inspection separately as an attestation that binds the inventory hash and enumerates every inspected relative path and expected hash. Mechanically require set equality with the required inventory scope and reject duplicates, omissions, hash mismatches, or unexplained exclusions; counts alone do not prove coverage. A render manifest proves generated bytes; it does not prove which bytes were actually inspected. After a report-wide semantic or layout change, inspect every page again unless a sealed baseline and mechanically proven affected-page set make differential inspection auditable.

Keep short comparison tables together when practical; let long appendices split with repeated headers. Check full-size pages around every split. Avoid unconditional page breaks that create orphaned headings, bullets, or nearly blank pages.

Freeze reader-visible figure order after section composition, not from figure generation order. Assign numbers in that display order, generate numbered captions and body references from the same registry, and verify HTML DOM order, PDF text order, uniqueness, missing references, and stale references. Inline insertion can make a generation-order inventory disagree with what the reader sees.

Treat non-Python report dependencies as semantic inputs too. Record and seal the exact font bytes used for PDF generation and the renderer executable hash plus version; a Python lock does not pin a system font or an arbitrary `PATH` binary. Bind rendered page/contact hashes to that renderer and the source PDF.

Make those non-Python bytes portable in the handoff: copy the exact font and the renderer's complete transitive runtime closure (DLLs, resources, and required adjacent files) into a workspace-relative dependency snapshot, seal a canonical inventory/root of every relative path/hash/size, and verify from a receiver-supplied snapshot root. Execute the portable renderer copy—not the producer's absolute binary—for version, smoke, and full-page rendering, preferably with an isolated search path that proves the closure is sufficient. An absolute producer-machine path may remain provenance, but cannot be the only resolver for receiver-side verification.

## Review calibration

Use a blank-context reviewer when independent judgment can reveal a materially different error class: a new or changed mechanic, endpoint contract, runtime promotion, exhaustive computation, evidence classification, gameplay recommendation, broad report rewrite, or semantic visualization change. Give the reviewer raw artifacts and the task, not intended conclusions, and require actionable findings. After each material correction, discard that reviewer, use a fresh one, rerun affected checks, and continue until no useful issue remains.

Do not automatically request another reviewer for a localized edit whose acceptance condition is deterministic and already covered, such as changing a heading from peer to child, correcting a label, repairing one link, or moving a known table without changing content. For those edits, run the narrow structural/text/hash check, render the affected pages, and run the existing report closure. Escalate to a fresh reviewer only if the edit changes interpretation, exposes a new uncertainty, affects multiple claims/pages, or the checks disagree.

Record review convergence only for reviews that were actually warranted. A reviewer is an evaluation surface, not a ceremonial final gate.

Seal in dependency order. Verify report artifacts, create the report seal, obtain any warranted converged blank review, then create the handoff seal and run receiver-side read-only chain verification. If a post-review change alters report bytes, rerun the affected closure and update downstream seals; do not disturb unchanged certificate/analysis parents.
