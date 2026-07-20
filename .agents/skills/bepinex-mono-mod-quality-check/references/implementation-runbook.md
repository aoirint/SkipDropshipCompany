# BepInEx Mono Implementation Runbook

Run this procedure in order. It is the reproducible implementation contract for
the quality baseline in this Skill. A target repository supplies values for the
evidence ledger; it does not choose which baseline controls are optional.

## Contents

- [1. Classify and inventory](#1-classify-and-inventory)
- [2. Evidence ledger](#2-evidence-ledger)
- [3. Required file inventory](#3-required-file-inventory)
- [4. Apply changes in dependency order](#4-apply-changes-in-dependency-order)
- [5. Conditional branches](#5-conditional-branches)
- [6. Verification matrix](#6-verification-matrix)
- [7. Completion report](#7-completion-report)

## 1. Classify and inventory

1. Set exactly one primary request type: `setup`, `alignment`,
   `implementation`, `release-readiness`, or `plan-only`.
2. List repository files before editing. Inspect `AGENTS.md`, `README.md`,
   `CONTRIBUTING.md`, all `*.sln` and `*.csproj`, `nuget.config`, every
   `packages.lock.json`, `.gitignore`, `.markdownlint-cli2.yaml`, package
   assets, `.github/workflows/`, `.github/actions/`, and release scripts.
3. For `alignment`, compare every item in the required inventory below with the
   repository. A missing baseline item is a finding. Do not mark it optional
   because a predecessor repository did not use it.
4. For `plan-only`, produce the same inventory and evidence ledger, label every
   uninspected item `unverified`, and do not claim a check passed.

## 2. Evidence ledger

Create this table before writing any target-specific value. `confirmed` must
cite a local file, package-host documentation, repository setting, or runtime
record. `blocked` stops only the dependent branch, not unrelated baseline work.

| Fact | Allowed status | Required before |
| --- | --- | --- |
| Game build, runtime, and supported platform | confirmed / blocked | target framework, compatibility claim, runtime pass |
| Exact BepInEx major and version | confirmed / blocked | loader reference, target framework, runtime pass |
| Plugin GUID, name, assembly name, owner, and version source | confirmed / blocked | plugin metadata, manifest, release identity |
| Game API, patch target, timing, and supported mod set | confirmed / blocked | patch implementation, compatibility claim |
| Package host and its authoritative contract | confirmed / blocked / none | host-specific archive rules or publishing |
| Release mode and version classification | confirmed / blocked / none | tag, GitHub Release, external publishing |
| GitHub Actions, GitHub Releases, Thunderstore, and APM use | yes / no / blocked | respective conditional branch |
| Repository/org settings and publish-secret scope | confirmed / blocked | CI/release readiness pass |

Never infer a `confirmed` value from a different game, mod, package host, or
repository. If a value is blocked, write the literal placeholder only in a
design document; do not commit it into executable metadata, manifests, or
publishing configuration.

## 3. Required file inventory

Create or retain each baseline artifact below unless its listed exception is
recorded in the evidence ledger and completion report.

| Artifact | Required content / invariant | Only valid exception |
| --- | --- | --- |
| Solution and SDK-style plugin project | explicit `TargetFramework`, `LangVersion`, identity/version synchronization path | `plan-only` has no repository to edit |
| `nuget.config` | `<clear />`; explicit approved sources; `packageSourceMapping` covering direct, transitive, and framework packages | no NuGet resolution is possible or required, with evidence |
| `global.json` | exact SDK version used by CI and release builds | no .NET SDK is required, with evidence |
| `packages.lock.json` | one committed lockfile per resolving project | project resolves no packages, with project-file evidence |
| `.gitignore` | narrow local/generated rules; never hides source, assets, lockfiles, or docs | none |
| `README.md` | setup, checks, debugging, packaging, compatibility evidence, release path | none |
| `CONTRIBUTING.md` | change-to-check matrix and contributor requirements | none |
| `CHANGELOG.md` and `LICENSE` | intentional release-history and license roles | repository has an explicit replacement, named in the report |
| `.markdownlint-cli2.yaml` | lint committed Markdown, respect `.gitignore`, narrow documented exceptions | none |
| versioned archive contract | host-neutral root/path/DLL/prohibited-content rules | no package/archive is produced, with evidence |
| APM files and deployed output | `apm.yml`, lock, generated target as one unit | ledger says APM `no` |
| GitHub workflows/actions | event-owned pull-request and integration-branch workflows, shared source-quality gate, build/release only when enabled | ledger says GitHub Actions `no` |
| Host manifest/publish action | exact selected-host extension only | host is `none` or blocked |
| Canonical-template selection | selected template IDs, canonical-content destinations, documented `-Check` command | no bundled template matches the enabled contract |

## 4. Apply changes in dependency order

Complete each numbered stage before beginning the next. If an enabled stage is
blocked, leave it unimplemented, record the blocker, and continue with later
independent stages.

### 4.1 Repository hygiene and documentation

1. Define intentional roles for solution/project files, `assets/`, `docs/`,
   package assets, and generated output.
2. Add `.gitignore` local rules first: game installs, profiles, `bin/`, `obj/`,
   IDE caches, logs, local work/build directories, agent worktrees, and only
   actually generated metadata. Keep any pinned upstream ignore template as a
   separate, commit-linked block below local rules.
3. Run `git status --short`. Stop and correct the ignore rules if a source,
   asset, lockfile, or document that must be committed becomes hidden.
4. Write README and CONTRIBUTING content from the evidence ledger. README names
   setup, restore, format, build, test, debugging, packaging, compatibility
   evidence, and release steps. CONTRIBUTING maps each changed surface to its
   required verification.
5. Add Markdown lint configuration. It must target committed `**/*.md`, honor
   `.gitignore`, exclude only transient agent/worktree paths, and explain every
   disabled rule inline.
6. When the evidence ledger enables a bundled template, select its IDs from
   `assets/template-map.json`, apply them with `scripts/sync_templates.ps1`, and
   add the same selection with `-Check` to contributor documentation and CI.
   Do not apply Thunderstore templates when the package host is absent,
   blocked, or different.

### 4.2 Project, module, and dependency boundary

1. Create or preserve one SDK-style plugin project. Set `TargetFramework` and
   `LangVersion` only from confirmed game/loader evidence.
2. Establish one checked synchronization path for assembly name, project
   version, BepInEx GUID/name/version, generated plugin information, manifest,
   archive name, tag, and release title. Verify every enabled output uses it.
3. Keep the plugin entry point/composition root limited to wiring, lifecycle,
   configuration/logging, and bounded patch registration. Do not put gameplay
   policy or framework callbacks together in it.
4. Keep a small plugin cohesive. Add Core only when gameplay policy/state/use
   cases need to be isolated from BepInEx/Harmony/Unity/game access. Core owns
   policy, state, results, and its ports and has none of those framework/game
   references. Interop owns callbacks, configuration, logging, game access,
   networking, and port adapters. A callback validates/translates then
   delegates; Core never reaches a game singleton directly.
5. When one callback/update feeds multiple outputs, establish one coherent
   update boundary in this order: let Interop read each required live object,
   method, reflection field, and physics/network result once; convert those
   reads to framework-free direct observations; let Core derive conditions and
   proxy results once; then pass the same immutable frame/result to every HUD,
   world-rendering, logging, or other presentation path. Presentation code must
   not query the game again. Record duplicate-read consolidation as an
   intentional timing/read-count change, preserve externally significant call
   ordering, and compare every output label, threshold, identity, and absence
   case with the pre-refactor contract.
6. Keep direct observations, derived conditions, and deliberately incomplete
   diagnostic proxies separate in type and property names. Group fields whose
   availability is correlated under one optional aggregate or explicit result
   variant so the model cannot represent impossible partial combinations.
   Preserve unreadable/unknown separately from observed false, empty, or
   absent values.
7. Keep an update frame local to the handler/use case by default. Add a Core
   store only when a named cross-callback rule requires history, smoothing,
   change detection, save/restore, or another retained value; document its
   replacement, reset, and session lifetime. Core ownership of a value type is
   not by itself a reason to retain the latest frame.
8. Add a separate Core or test project only for documented test, reuse, build,
   or dependency-isolation value. For every new project, repeat the target
   framework review, source mapping, lockfile, restore, format, build, and test
   coverage steps. Verify it is not included in the player package unless the
   archive contract explicitly requires it.
9. Configure game and BepInEx references compile-only when the player install
   supplies them. Mark analyzers, generators, and build-only metadata helpers
   `PrivateAssets="all"`. Do not use local game paths as a restore workaround.
10. Add `nuget.config`, then generate and commit the lockfile for every
   resolving project. Before accepting a new source/package/lock delta, review
   canonical URL, publisher, immutable version or digest, available hash,
   license, transitive graph, and seven-day release age.
11. For every Harmony/game callback, write down the allowed execution role and
   failure behavior. Check the current role explicitly before state mutation,
   scans, UI, or network work; unavailable network state fails closed. Wrap the
   primary callback and diagnostic sink independently so neither can throw into
   the game callback.
12. For each all-or-nothing observation, list every required layer used to
   classify an entry: the entry itself, metadata, and classification fields.
   Fail the observation when any layer is unavailable or Unity-destroyed; do
   not silently skip it and return a partial result.
13. For each base-game transaction, name the original values and all prepared,
    spawned, or synchronized side effects. Put the restoration scope before the
    first destructive mutation and keep the first through last fallible commit
    operation inside it. On failure, restore exact original values, remove all
    residue that can be removed safely, emit restoration evidence, and still
    prevent the exception from escaping the game callback.
    Guard rollback steps independently when one failed restoration operation
    would otherwise prevent later cleanup or re-synchronization; record the
    residual state that could not be restored.
    For each acquisition step, fault immediately after allocation,
    instantiation, registration, and spawn. The step must either transfer an
    owned cleanup handle to rollback or clean up its partial resource before it
    throws.
14. Select Harmony prefix/postfix timing from the event meaning. Use postfix
    for `completed`, `reset`, or `applied` observations unless the mod must
    alter inputs before the base call. Verify that skipped/throwing base methods
    do not advance completion-dependent mod state.
15. For every patched RPC/network method, create a boundary ledger with one row
    per host-local, remote-client, dedicated-server, and receive path that can
    occur. Record role/stage, authoritative method arguments, fields mutated
    before the hook, and fields updated only by a later RPC receive. Read or
    rewrite only values proven authoritative at the chosen hook.
    Enumerate every baseline caller of a shared boundary and the caller classes
    exposed to other patches. Carry an explicit transaction-origin token or
    pair entry/exit hooks when the shared callback alone cannot distinguish the
    intended action from unrelated synchronization.
    Add a boundary harness that calls the actual Harmony prefix/postfix methods
    in the documented vanilla order. Exercise host-local, remote-client, and
    unrelated shared-callback paths. Give method arguments, mirrored fields,
    and deferred receive-stage fields distinct sentinel values, then fault each
    stage so provenance and rollback cannot pass by equality coincidence.
    Encode only mutations present in the inspected base-game body. Assert every
    pre-hook sentinel, including fields intentionally left stale until a later
    receive stage. A test that manually advances mirrored state earlier than
    the real call graph is invalid evidence.
16. Keep distinct names or value types for protocol indexes, catalog indexes,
    network object IDs, and stable domain IDs. Add a fixture where a catalog
    index differs from the stable ID and prove downstream hashing,
    serialization, or persistence follows the contractually named identity.
17. Write a lifecycle truth table for each eligibility/state predicate. Include
    every named positive state plus adjacent loading, departing, travelling,
    reset, and unavailable negatives. Do not implement a narrower product state
    with a broader convenience proxy.

### 4.3 Archive contract and package content

1. Create a small versioned archive contract in repository-owned documentation
   or a deterministic validator. Its host-neutral base specifies allowed root
   paths, required repository-owned files, the exact count and path of plugin
   DLLs, prohibited runtime/game DLLs, `bin/`, `obj/`, logs, profiles, local
   paths, secrets, and archive traversal/absolute-path/unsafe-link entries.
   Map each prohibition to a direct validator assertion and a representative
   failing fixture or deterministic test. Path normalization does not prove
   unsafe-link rejection; inspect the archive metadata needed for every claim,
   or narrow the documented contract.
   Preserve raw entry names for separator/syntax checks before normalization,
   require regular-file entry types, and inspect the packaged managed assembly
   for expected assembly name, loader GUID/name/version, and enabled process
   restriction. A correctly named corrupt or wrong binary must fail.
   Decode the assembly custom-attribute table and compare the constructor
   values of `BepInPlugin`, `BepInProcess`, and required dependency attributes;
   do not substitute byte-string search. Maintain one positive fixture and one
   mutation fixture for every documented rejection branch, including missing,
   unexpected, duplicate, extra-DLL, corrupt-DLL, wrong identity/version, and
   wrong or missing loader attributes.
   Fixture tests must invoke the production validator/package path or the exact
   shared library it calls. Do not copy validation rules into a test helper and
   test the copy.
2. Build into a clean staging directory. Copy only files allowed by the archive
   contract. Create exactly one archive, inspect its entries, and compute a
   SHA-256 digest.
3. If a package host is confirmed, add only its authoritative extension:
   manifest fields, root layout, dependency syntax, namespace/category, version
   restrictions, authentication, and overwrite behavior. Do not use a
   Thunderstore manifest or GitHub asset convention for another host.
4. Extract the final archive and inspect the files users actually receive.
   Verify that package-facing metadata/documentation states the enabled plugin
   identity, release version, supported game/loader baseline, and dependencies
   required by the product contract. Verify those claims against the project
   and evidence ledger; do not accept an undistributed root README as proof.
5. Trace the changelog file copied by packaging. It must be documented either
   as a distinct publication-facing source or as a canonical dual-purpose
   changelog, and it must contain the packaged stable version. Do not claim a
   generated/derived release-note step unless automation performs and verifies
   it.
6. Block package publication when host contract, package identity, runtime
   evidence, or final-archive validation is blocked or failing.

### 4.4 Automation

1. If APM is `yes`, preserve/create `apm.yml`, pin remote sources to full SHAs,
   check provenance/license/last-changed-subdirectory cooldown, record third
   party notices, then commit manifest, lockfile, and generated output together.
2. If GitHub Actions is `yes`, create/align event-owned entry workflows: `Pull
   Request` for `pull_request` and `merge_group` when used; `Main` for the
   protected integration-branch push. Both run the same source-quality gate on
   their checked-out commit. `Main` uses direct `needs` dependencies to gate
   build, artifact upload, and publication; never substitute API polling or an
   `await-quality` job. Run source quality in this order: checkout; external-tool
   setup and verification; ShellCheck; actionlint; pinact; SDK setup; locked
   restore; format; no-restore build; tests; Markdown lint; archive validation
   when relevant. Use read-only permissions, explicit Bash, and PR-only
   cancellation concurrency. Add manual dispatch only for a documented
   diagnostic or recovery operation.
   Read the SDK version from `global.json` and install it explicitly using a
   full-SHA-pinned setup action or pinned verified equivalent before restore;
   verify `dotnet --version` matches. Do not depend on runner inventory.
   Compare the workflow with README/CONTRIBUTING and committed lint/check
   configuration. Every promised check and every retained configuration must
   be invoked by a documented local command and CI, or be removed with a
   concrete reason.
3. Pin third-party actions by full SHA plus accurate version comment, containers
   by digest, and downloaded executable tools by adjacent version and checksum.
   Cache only verified archives and use committed lockfiles as NuGet cache keys.
4. If GitHub Releases is `yes`, create a build job that creates one archive and
   digest from the integration-branch commit, then uploads it for every build
   including unpublished edge builds; publish only a downloaded-and-verified copy.
   The release job alone receives `contents: write`; it creates a draft, adds
   all assets and checksum, then publishes. It fails on an existing tag,
   release, or asset and never rebuilds or replaces an artifact. Require
   immutable releases where available; otherwise record residual risk. After
   publication, query the release record and verify its tag, target commit,
   immutable/prerelease state, exact asset count, asset digest, and each
   intentionally skipped host-publishing step.
5. If external publishing is enabled, gate it to the confirmed stable mode,
   exactly one reviewed prebuilt archive, passing digest/archive/runtime checks,
   and a credential scoped to the one publish step. Never expose it to PR jobs.

## 5. Conditional branches

| Condition | Required action | Explicitly do not do |
| --- | --- | --- |
| APM = no | omit APM files/checks and record this | create empty APM configuration |
| GitHub Actions = no | document local checks | invent workflow YAML or pin rules |
| GitHub Releases = no | retain archive/digest validation if packaging | require draft/immutable-release behavior |
| Package host = none | define archive contract if an archive exists | create a host manifest or upload workflow |
| Package host = Thunderstore | apply Thunderstore section in the template | publish edge/prerelease without a supported contract |
| Custom package host | add only verified host extension | borrow Thunderstore/GitHub metadata |
| No automated tests | record no-test status and test strategy | invent a test command |
| New/changed automated tests | run documented test command after build | treat build as test evidence |
| Runtime evidence blocked | complete static checks | claim compatibility or approve stable publishing |

## 6. Verification matrix

Run every row whose trigger is true. A command that cannot run is `skipped`,
not passed; record the command, reason, and resulting risk.

| Trigger | Required verification | Pass condition |
| --- | --- | --- |
| C# or project change | `dotnet restore --locked-mode`; `dotnet format --no-restore --verify-no-changes`; `dotnet build --no-restore` | all exit 0; lockfiles unchanged after restore |
| Automated tests exist or changed | documented test command after build | exit 0 and relevant tests execute |
| Markdown/package text | checked-in Markdown linter over committed Markdown | exit 0 |
| `.gitignore` change | `git status --short` plus intended/unintended file review | no required file hidden |
| One callback/update feeds multiple outputs | observation-source spy or call-count fixture when a harness exists; otherwise source and built-call-site inspection plus recorded runtime gap; compare every output from deliberately distinct direct/derived/proxy and absent/unknown inputs | each live query runs once per subject/update; every output consumes the same frame/result; impossible partial availability is unrepresentable; unknown does not collapse into false/absence |
| Callback/interop boundary change | role-denied/unavailable-state check; throwing-primary-callback check; throwing-diagnostic-sink check; all-or-nothing nullable-layer check | no unauthorized side effect or state transition; no exception escapes; no partial observation is emitted |
| Base-game transaction change | fault injection at first mutation, every synchronization/RPC/spawn step, and final commit | exact original state restored; no prepared/spawned residue; restoration and bounded callback-exception evidence emitted |
| Resource preparation/acquisition | fault after each allocation, instantiation, registration, and spawn using the production transaction | every acquired resource is transferred to rollback ownership or cleaned internally; no residue survives |
| Network/RPC patch change | boundary-ledger review plus host-local, remote-client, server, and receive-path checks as applicable | every hook reads authoritative inputs/fields at that stage; both local and remote flows reach the intended exactly-once result |
| Harmony provenance/transaction boundary | actual patch-entry harness in vanilla call order with distinct argument/field/receive sentinels and per-stage faults | provenance encloses the intended mutation on every path; unrelated callers do nothing; rollback restores the authoritative value rather than a mirrored stale field |
| Shared RPC/event domain action | enumerate callers and test intended plus non-domain provenance paths | explicit origin reaches the effect exactly once; unrelated synchronization leaves state unchanged |
| Identifier-dependent behavior | fixture where catalog/protocol index differs from stable domain ID | hash/persist/log/serialize result follows the contractually named identity |
| Lifecycle predicate change | positive-and-adjacent-negative truth table | every named positive passes; loading/departing/travelling/reset/unavailable negatives fail unless explicitly included |
| NuGet source/package/lock change | source/publisher/version/hash/license/transitive/age review | ledger records approval; mapping/locks cover every resolver |
| Workflow/action/shell change | ShellCheck, `actionlint`, `pinact run --check --min-age 7`, manual pin/permission/concurrency/secret review | all pass; every executable input is pinned/verified |
| Bundled template adopted or changed | run `sync_templates.ps1 -Check` with the repository's selected IDs in the canonical Skill and every opted-in consumer | every selected destination exists and satisfies its manifest comparison mode; exclusions and local variants are documented |
| Lint/check config or contributor command | trace config to local command and enabled CI step | each retained config is consumed and every promised command is runnable in both documented and CI contexts |
| APM change | `apm lock`; lock review; `apm install --frozen`; `apm audit --ci` | expected refs/hashes and no drift |
| Package/release change | clean staging, archive-contract inspection, SHA-256, exact-artifact handoff check | one valid archive; digest matches |
| Package binary identity | decode built and archived assembly/custom-attribute metadata against project/package contract | assembly name, `BepInPlugin` GUID/name/version, `BepInProcess`, required dependencies, manifest, and archive identity agree; one mutation fixture per rejection rule fails |
| Validator/policy fixture | invoke production command/path or the exact shared library it calls | fixture failure proves production enforcement; no duplicated test-only rule implementation |
| Compatibility/release claim | clean-profile runtime test | record exact game build, BepInEx version, mod set, install path, scenario, result |
| Structured validation logging requested | inspect representative startup, success, denial/failure, receiver/apply, restoration, and swallowed-exception records | role is observed or neutral rather than asserted; records prove the named outcome with boundary/source, result, and only necessary bounded before/after values; inner and outer swallowed exceptions are recorded; privacy exclusions hold |
| GitHub Release | repository-setting review, fail-on-existing release path, and post-publication release-record inspection | settings/gaps recorded; tag and target commit match; immutability, prerelease/stable mode, asset count, and asset digest match; intentionally skipped host publishing is recorded |

## 7. Completion report

Use this exact order:

1. Request type and repository state.
2. Evidence ledger, including every `blocked` fact and dependent blocked branch.
3. Baseline inventory: `present`, `added`, `finding`, or `valid exception` for
   every artifact in section 3.
4. Files changed and synchronization paths verified.
5. Verification matrix results: `passed`, `failed`, or `skipped` with the exact
   command/reason/risk.
6. Runtime and package-host evidence.
7. Remaining risks and the precise condition that prevents approval or
   publication.

Do not call the repository ready when any required baseline artifact is a
finding, any enabled verification is skipped/failed, a release artifact is not
verified, or a compatibility/publishing claim lacks its required evidence.
