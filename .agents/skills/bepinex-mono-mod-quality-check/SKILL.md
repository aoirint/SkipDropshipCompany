---
name: bepinex-mono-mod-quality-check
description: Review or create a BepInEx Mono mod for C# project and module structure, plugin identity, game compatibility, dependency boundaries, GitHub CI and release automation, packaging, release metadata, and verification. Use when implementing, reviewing, preparing a release for, or diagnosing quality gaps in a BepInEx 5 Mono plugin or its build and release workflow.
---

# BepInEx Mono Mod Quality Check

## Goals

- Preserve a loader-visible plugin identity that agrees with the assembly, generated metadata, and package metadata.
- Keep C# project and module boundaries aligned with gameplay policy, framework integration, and composition responsibilities.
- Keep game and BepInEx compatibility explicit, evidence-backed, and scoped to what was actually tested.
- Produce reproducible builds and installable release packages without committing local game or build output.
- Keep GitHub Actions and release automation pinned, least-privilege, and traceable to the built artifact.
- Report findings as actionable evidence, not assumed conventions from another mod.

## Documentation responsibility

Use `software-documentation-maintenance` to create and maintain the required
`docs/domain/`, `docs/architecture/`, and `docs/operations/` map, its indexes,
canonical ownership, migrations, and links. This Skill remains responsible for
the BepInEx-specific content and correctness placed in that map:

- `domain/` records the exact tested game, loader, Harmony, Unity, networking,
  serialized-asset, and package-host facts needed by the integration.
- `architecture/` records the mod's Core/Interop boundaries, callback and
  authority policy, state lifetime, transactions, failure behavior, and
  product-selected integration choices.
- `operations/` records the verified restore, build, runtime validation,
  packaging, release, and Thunderstore procedures selected by the repository.

Apply `prose-quality-check` only after ownership and technical evidence are
settled. Do not move a mod-selected policy into reusable domain knowledge, and
do not treat a well-structured document as proof that its game or release claim
is correct.

Use `unity-game-analyze` to establish version-specific game-code and serialized-asset evidence:
the concrete call path, effective prefab/scene values, object/load reachability, lifecycle timing,
authority, and unresolved runtime inputs. This Skill consumes that evidence to select and verify
hooks, adapters, identity mappings, compatibility claims, and tests. It remains responsible for the
mod decision; `unity-game-analyze` must not approve a Harmony target merely because a method exists.

## Required Template Baseline

Use the quality baseline in this Skill for a repository that does not already
have stricter, equivalent rules. Repository files establish target-specific
facts such as game build, loader version, package host, and release namespace;
they do not replace this Skill's quality bar.

- Commit an SDK-style project, `nuget.config` with explicit sources and package
  source mapping, `global.json` selecting the release SDK, one `packages.lock.json` per resolving project, a narrow
  `.gitignore`, Markdown lint configuration, and contributor documentation.
- Keep the contributor agreement and the pull-request confirmation internally
  complete. When adopting the bundled contract, deploy
  `repository-contributing` and `github-pull-request-template` together; never
  infer that a CLA checkbox is unsupported without inspecting its referenced
  `CONTRIBUTING.md` section.
- For C# changes, require locked restore, format verification without restore,
  and a no-restore build. For Markdown, workflow, shell, APM, package, or
  release changes, apply the corresponding checks in
  [repository-quality-template.md](references/repository-quality-template.md).
- Use event-owned CI entry workflows: pull requests (and merge queues when used) validate proposed
  source with a `lint` job (and a `test` job when the repository has tests), while the integration
  branch re-runs those jobs on its exact commit and directly gates `build`, retained edge artifacts,
  and `release` through `needs`. Keep `lint-source` as a Composite Action name, not a catch-all job
  name. When version or publication state must be resolved, use a read-only `plan` job and make
  `build` depend directly on `lint`, optional `test`, and `plan`; have `release` consume the
  verified build artifact and any needed plan output. Do not add manual dispatch
  or polling jobs without a documented operator/recovery need.
- When the target adopts a bundled CI or publishing contract, copy its files
  exactly from this Skill's canonical `assets/` and enforce the manifest's
  exact-content drift checks. Follow
  [canonical-templates.md](references/canonical-templates.md) for selection,
  synchronization, validation, and rollout; do not fork a template silently.
- Treat a missing required file, unpinned external executable input, unreviewed
  package source, absent lockfile, or unverified final archive as a finding;
  do not downgrade it to a repository preference.
- Allow a target repository to add stricter checks. Record a concrete
  compatibility or host constraint before omitting a baseline item.

## Workflow

Follow [implementation-runbook.md](references/implementation-runbook.md) in
order for every setup, alignment, implementation, or release-readiness task.
It defines the required evidence ledger, file inventory, change order,
conditional branches, verification matrix, and report format. Do not replace
that procedure with an informal checklist.

1. Establish the repository contract before proposing changes.
   - Read `AGENTS.md`, `README.md`, `CONTRIBUTING.md`, the project file, lockfile, package assets, and relevant CI or release scripts to find target-specific facts and existing stricter rules. Do not use their absence as a reason to omit this Skill's baseline.
   - Create the evidence ledger from the runbook before selecting a target framework, loader references, plugin identity, package metadata, archive layout, or publish destination. Mark an unavailable fact `blocked`; never fill it from another BepInEx repository.
   - Treat a requested game-version change as a compatibility alignment. Record
     the old and target version, Steam manifest/build identifier, matching
     managed-code and serialized-asset evidence handoff hashes, game-reference
     package version, every compatibility claim, and the required build and
     runtime checks. Do not claim the target version when code and asset
     evidence come from different builds.
   - Classify the request as `setup`, `alignment`, `implementation`, `release-readiness`, or `plan-only`. Execute only the runbook branches enabled by the evidence ledger and request type; record every disabled branch and its concrete reason.
   - For a new repository or a template-alignment review, read [repository-quality-template.md](references/repository-quality-template.md) after the runbook inventory. It is the normative file-by-file standard, not a source of target-specific values.
   - If the evidence ledger enables a contract represented by a bundled template, read [canonical-templates.md](references/canonical-templates.md), select only the applicable template IDs, and use the sync script for both application and CI drift checks. Keep target-specific workflow values in the caller, not in a copied action.
   - When a patch, reflection target, serialized name/value, Unity object, game lifecycle, or build-compatibility claim lacks a closed target-build trace, use `unity-game-analyze` and record its evidence handoff. Do not reconstruct the Unity code/asset procedure informally inside the mod review.
2. Check C# project and module structure.
   - Start from the repository's existing layout. Do not introduce projects, layers, or folder taxonomy solely to resemble another mod.
   - Keep the loader entry point and composition root small. It may create the controller, adapters, state, and use cases, but it must not become the location for gameplay policy or framework callbacks.
   - Put gameplay policy, state transitions, result values, and use cases in framework-free Core modules when the mod has enough behavior to benefit from a boundary. Core may depend on ports it defines, but not on BepInEx, Harmony, Unity, or game-specific types.
   - Put BepInEx configuration/logging, Harmony callbacks, Unity/game-object access, networking, and concrete port adapters in Interop modules. Keep callbacks thin: validate or guard the framework boundary, translate data, and delegate to Core-facing controller methods.
   - Introduce a Core port and an Interop implementation when a use case needs game or framework access. Do not hide that access behind static singletons in Core.
   - When one callback or update supplies multiple outputs, capture live game/framework state once in Interop as a framework-free observation, derive conditions once in Core, and pass one immutable frame or result to every presenter. Do not let HUD, world rendering, logging, or other presentation paths independently resample the same live state.
   - Keep direct observations, derived conditions, and intentionally incomplete diagnostic proxies distinct in names and types. Represent values that become available together as one optional aggregate or result variant instead of correlated nullable properties, and preserve `unknown` or `unreadable` separately from observed `false` or absence.
   - Do not add persistent Core state merely because a value type belongs in Core. Keep an update frame transient unless history, smoothing, change detection, save/restore, or another explicit cross-callback rule requires a store.
   - Keep a module cohesive around one responsibility. Split a module when it mixes policy with framework access or when its lifecycle/state ownership cannot be stated clearly; do not split it merely by file type or pre-emptive reuse.
   - Keep one plugin project when it enforces the needed boundary. Add separate C# projects only when a concrete test, reuse, build, or dependency-isolation need outweighs the added target-framework and packaging complexity.
3. Check the plugin boundary.
   - Ensure the assembly name, plugin GUID, plugin name, and loader-facing version have one documented source of truth or a verified synchronization path.
   - Confirm the plugin entry point uses BepInEx metadata and that generated plugin-info values, if used, agree with the project configuration. For a game-specific plugin with a stable tested executable name, declare `BepInProcess` and verify it in the built assembly.
   - Keep Harmony or equivalent patch registration deliberate: use a stable identifier, a bounded assembly/patch scope, and lifecycle behavior that does not duplicate registration.
   - Make every patch/callback boundary fail-safe according to its declared behavior. Guard the primary callback and its diagnostic/error-reporting path independently so a logging, serialization, or reporting failure cannot escape into the game callback.
   - Gate callback-driven state changes, scans, UI work, and network sends on the mod's declared execution role (`client`, `host`, or server) using an explicit role check at the Interop boundary. Treat unavailable network state as not authorized for that role; do not rely on incidental RPC stage or prior state to suppress work.
   - When an observation must be all-or-nothing, enumerate every prerequisite needed to classify each entry. A missing/destroyed entry, required metadata object, or required classification field fails the whole observation; never return a partial count merely because one nullable layer was skipped.
   - For transactional changes to base-game state, separate prepare, commit, and rollback. Start the restoration scope before the first destructive mutation; keep every fallible synchronization, spawn, RPC, and commit operation inside it; and restore the exact original values plus remove prepared/spawned residue on failure. A swallowed outer exception is not a substitute for rollback.
   - Treat resource acquisition as a transaction too. If allocation or instantiation succeeds and later validation in the same prepare step fails, return an owned cleanup handle or clean up internally before throwing. Fault tests must cover failure after each acquired resource, not only before a stage begins.
   - Before patching a networked method, record its boundary ledger for every relevant path: execution role, send/server/receive stage, authoritative arguments, synchronously mutated fields, and deferred RPC effects. Select the hook and state source from that ledger; never read a mirrored field in a postfix unless the call graph proves it is updated before the hook. Cover host-local and remote-client paths when both exist.
   - For a shared RPC/event boundary, enumerate every baseline caller and relevant mod-extensible caller class. Preserve an explicit transaction-origin signal or paired hook so a state synchronization callback is not treated as proof that the intended domain action occurred. Negative provenance paths must leave base-game state unchanged.
   - Validate the real Harmony boundary, not only extracted Core policy. Invoke the actual prefix/postfix entry points in the target version's call order for every host and remote path. Use deliberately different method-argument, pre-call field, post-call field, and deferred-receive values so an incorrect authority source or provenance window cannot pass through accidental equality.
   - Make the boundary harness reproduce the base method's actual mutations and omissions. Do not manually update a fake field at a point where the inspected game method leaves it stale. Assert each intermediate sentinel before invoking the patch so the test proves timing rather than merely reaching the expected final value.
   - Preserve protocol indexes, catalog/list indexes, network IDs, and stable domain IDs as separately named or typed values through preparation and application. Hash, persist, log, or serialize only the identity named by the product contract; never relabel a collection index as a domain ID.
   - Choose prefix or postfix from lifecycle semantics. Observe completion-dependent state in a postfix unless pre-call mutation is required; if the base method is skipped or throws, do not report or persist completion that did not occur.
   - For every lifecycle predicate, enumerate the named positive states and immediately adjacent negative transitions such as loading, departing, travelling, resetting, and unavailable state. Build and verify a truth table rather than using a broad proxy such as `not in orbit` when the product names a narrower state.
   - Treat game API calls, patches, serialized names, and timing assumptions as compatibility-sensitive. Preserve the exact tested game baseline and request runtime evidence when a code-only review cannot establish it.
4. Check build and dependency boundaries.
   - Match the target framework and language version to the repository's documented BepInEx and game runtime compatibility; do not upgrade either opportunistically.
   - Keep game and loader references compile-only when the release package relies on the player's installed runtime. Keep development analyzers and code generators private to the project.
   - When updating a game-specific compile reference, update its resolved
     package and lockfile to the target game version, then reconcile every
     version-bearing project, package, changelog, and compatibility document
     field. A successful compilation proves only API resolution; it does not
     establish runtime compatibility.
   - Require a committed lockfile for every resolving project and run `dotnet restore --locked-mode`. After restore, run `dotnet format --no-restore --verify-no-changes` and a no-restore build. A missing lockfile or unlocked restore is a reproducibility finding.
   - When a change adds or modifies automated tests, run the repository's documented test command after the locked restore and no-restore build. A separate Core or test project is justified only by its stated test, reuse, build, or dependency-isolation benefit, and must meet the same lockfile and source-mapping rules as every other resolving project.
   - Require `nuget.config` to clear unreviewed default sources and map direct and transitive package patterns to their intended source. Review every new feed, package, lockfile change, CI action, container, or downloaded tool for canonical source, publisher, immutable version or digest, content hash where available, license, transitive effects, and seven-day release age before adoption.
   - Do not commit game installations, mod-manager profiles, generated plugin metadata, `bin/`, `obj/`, or machine-local paths.
5. Check package and release consistency.
   - Trace the release artifact from the built DLL to its archive. Confirm that the archive layout and every required file match the package host and repository documentation.
   - Keep a versioned, repository-owned archive contract. Its host-neutral part must define the allowed root paths, exact plugin-DLL count, prohibited contents, and archive path-safety rules; add a host-specific extension only from the selected host's authoritative contract. Do not substitute another host's manifest or layout.
   - Reconcile package manifest identity, version, dependencies, compatibility claims, README, changelog, icon, and license with the release intent.
   - Verify version synchronization from produced outputs, not duplicated source literals. Generate loader-facing metadata from the project version or inspect the built assembly and compare project, assembly, loader-facing, manifest, archive, and tag versions that are enabled for the release.
   - Decode BepInEx loader metadata from the built assembly's actual custom attributes. String presence in DLL bytes does not prove `BepInPlugin`, `BepInProcess`, or dependency attributes carry the expected constructor values.
   - Verify identity, supported game/loader baseline, dependencies, and release version on the exact metadata and documentation files copied into the final archive. Repository-root prose does not satisfy a package-facing claim when it is not distributed.
   - Keep developer changelog entries and user-facing release notes in their repository-defined roles. Either package a distinct publication-facing source or explicitly declare the canonical changelog dual-purpose; do not describe a derivation step that packaging does not perform. Do not claim untested game compatibility or publish a version already represented by an immutable release.
   - When the project derives manifest or package versions in CI, verify that the project version, generated version, and loader-compatible version are deliberately handled for stable, prerelease, and edge builds.
6. Check GitHub repository settings, CI, and release automation when the repository uses GitHub Actions or GitHub Releases.
   - Require the repository or organization Actions setting that enforces full-length commit-SHA pins. Independently verify every third-party `uses:` reference has a full commit SHA and an accurate version comment; inspect reusable workflows, container digests, and downloaded-tool checksums too.
   - When the repository publishes GitHub Releases, require repository-level immutable releases where GitHub makes the setting available. Automation must attach every asset before publishing the release and must fail rather than replace an existing tag, release, or asset. If the setting is unavailable, record the residual risk and require an explicit fail-on-existing-release/tag/asset path instead of silently treating releases as immutable.
   - Trace one release from its source commit through locked restore, build, archived artifact, and release asset. Publish only the artifact produced by that build; do not rebuild a separately checked-out revision in the release job. Create and verify an artifact digest across the build and release jobs.
   - Install the exact SDK selected by `global.json` in CI with a full-SHA-pinned setup action or another pinned, verified mechanism before restore. Do not assume a hosted runner already contains the release-critical SDK.
   - Separate validation artifacts, prereleases, and stable publishing according to the repository's version rules. Gate external package-host publication on the intended stable release mode and require exactly one reviewed package artifact.
   - Retain every integration-branch edge build as a workflow artifact, with its source commit and digest visible in the workflow summary, even when the edge version is deliberately not published.
   - Default workflow permissions to read-only. Scope `contents: write` and publishing secrets to the release job that needs them, and never expose a publish credential to pull-request validation.
7. Run the narrowest relevant checks, then widen for the changed surface.
   - For C# or project changes, run locked restore, format verification, and no-restore build. Run the documented tests when automated tests are present or changed. Use the solution or project path required by the repository layout.
   - For documentation or package text, run Markdown lint over every committed Markdown file using the checked-in configuration.
   - For workflows, composite actions, or shell scripts, run ShellCheck, `actionlint`, and `pinact run --check --min-age 7`; check full-SHA action pins, container digests, downloaded-tool checksums, permissions, concurrency, and secret scope.
   - Reconcile declared tooling with execution. Every committed lint/check configuration and every command promised in README or CONTRIBUTING must have a runnable documented command and an enabled CI invocation, or be removed with the documented reason.
   - For APM changes, run `apm lock`, review locked refs and hashes, run `apm install --frozen`, then `apm audit --ci`.
   - For release or compatibility changes, inspect the final archive and perform runtime validation. State exactly what environment, game version, loader version, and mod set were tested; record missing runtime evidence as a blocker rather than guessing.
   - When the product requires structured validation logging, check evidence quality as well as event coverage. A record that proves a state transition must identify the observed execution role, boundary/source, result, and bounded non-sensitive before/after values needed to distinguish successful application from callback reachability. Derive authority at the event boundary; use a neutral role when unavailable. Every exception swallowed inside a patch callback, including an inner transaction catch, emits the bounded callback-exception event in addition to domain restoration evidence. Preserve the product's privacy exclusions.
8. Report the result in this order: scope and evidence consulted; findings grouped by structure/plugin/build/CI/package/runtime; checks passed or skipped; and remaining compatibility or release risks. State every omitted baseline item and its concrete constraint; do not report a quality pass based only on an existing repository convention.

## Reference Baseline

Read [repository-derived-baseline.md](references/repository-derived-baseline.md) when starting a new BepInEx Mono project or when the repository does not already document its structure and quality gates. It records transferable practices; it is a baseline, not a replacement for the target repository's contract.

Read [repository-quality-template.md](references/repository-quality-template.md) when creating or aligning a repository. It turns that baseline into a checklist for ignore rules, APM, NuGet, Markdown, CI, composite actions, and conditional Thunderstore publishing.

Read [canonical-templates.md](references/canonical-templates.md) when a target
can adopt an exact bundled Composite Action or script. It defines the canonical
assets, synchronization command, drift gate, and multi-repository maintenance
procedure.
