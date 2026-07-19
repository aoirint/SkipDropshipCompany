# Repository Quality Template

Use this checklist to create or align a BepInEx Mono repository. Choose values
from the target game's compatibility evidence and package-host contract; do not
copy another repository's versions, team names, or categories.

The sections below are this Skill's quality standard, not suggestions to defer
to an existing repository. A repository may add stricter controls. Omit a
listed control only when a concrete target-runtime, package-host, or deployment
constraint makes it inapplicable, and record that constraint in the review.

## Contents

- [Repository foundation](#repository-foundation)
- [APM-managed agent guidance](#apm-managed-agent-guidance)
- [NuGet and C# project quality](#nuget-and-c-project-quality)
- [Documentation and Markdown](#documentation-and-markdown)
- [GitHub repository and CI](#github-repository-and-ci)
- [Composite actions](#composite-actions)
- [Release automation](#release-automation)
- [Thunderstore publishing](#thunderstore-publishing)
- [Completion review](#completion-review)

## Repository foundation

- Keep the solution, plugin project, `assets/`, `docs/`, `CHANGELOG.md`,
  `LICENSE`, `README.md`, and `CONTRIBUTING.md` in intentional, documented
  roles. Keep base-game evidence separate from mod-specific architecture and
  operations documentation.
- Start `.gitignore` with only project-local paths: game installs, mod-manager
  profiles, generated plugin metadata, local `work/` or `build/` directories,
  and agent worktrees. Do not ignore source, release assets, lockfiles, or
  developer documentation.
- Add a reviewed Visual Studio/.NET ignore template only when useful. Pin the
  upstream source to a commit URL, preserve its template block, and keep local
  rules separate. Run `git status --short` after changes so an ignore rule does
  not conceal a tracked or intended artifact.
- Ignore `bin/`, `obj/`, IDE caches, local logs, downloaded game files, and
  local profiles. Never use an ignore rule as a substitute for keeping secrets
  out of the repository.
- Ignore common local secret/configuration files such as `.env` and nested
  `.env` variants. If the repository intentionally commits an example, keep a
  narrow negation for that exact example and confirm it remains visible.
- Keep an explicit generated-code path only when the build actually produces
  it. Do not use broad `Generated/` or wildcard ignores that can hide authored
  C# files.

## APM-managed agent guidance

Use APM when the repository uses Codex agent guidance.

1. Add `apm.yml` from the repository root with only the targets that the
   repository actually supports. Preserve an existing manifest; do not replace
   it with unattended initialization.
2. Install consumer-facing skills as pinned dependencies from
   `aoirint/skills/.apm/skills/<skill-name>` or another reviewed source. Keep
   maintenance-only Skills out of the consumer's production dependency set.
3. Pin each remote source to a full commit SHA, review its provenance, license,
   last changed subdirectory commit, and seven-day cooldown. Record third-party
   deployed content in `THIRD_PARTY_NOTICES.md`.
4. Commit `apm.yml`, `apm.lock.yaml`, and the generated target output together.
   Treat `.agents/skills/` as deployment output: do not edit it by hand.
5. After a source or dependency change, run `apm lock`, review the locked refs
   and hashes, run `apm install --frozen`, then `apm audit --ci`. Add the frozen
   install and audit checks to CI when the repository enforces agent setup.

See `$apm-usage` for installation, cooldown, license, and update details.

## NuGet and C# project quality

- Use an SDK-style plugin project with an explicit target framework and C#
  language version selected for the tested BepInEx and game runtime. Record the
  compatibility reason; do not upgrade it only because a newer SDK exists.
- Commit one `packages.lock.json` per project that resolves packages. Restore
  with `dotnet restore --locked-mode` locally and in CI; use `--no-restore` for
  format and build after that successful restore.
- Keep each direct package version deliberate. Use exact ranges when the game
  or loader ABI requires a fixed version, and keep build-only analyzers,
  generators, and metadata helpers `PrivateAssets="all"`.
- Mark game/loader dependencies compile-only when the player installation
  supplies them. Do not accidentally bundle BepInEx, Unity, or game DLLs into
  the package. Use aliases when references would otherwise make type ownership
  ambiguous.
- Add a repository `nuget.config` with `<clear />` package sources and
  `packageSourceMapping`. Map every direct and transitive package namespace to
  the intended source, including target-framework reference packages. Treat an
  unmapped package, a broadened source, or a mapping exception as a dependency
  review item.
- Review package source URLs, lockfile changes, and transitive dependency
  changes with `$security-check`. Do not add local game DLL paths or unreviewed
  feeds as a convenient restore workaround.
- Keep the assembly name, BepInEx metadata, project version, generated plugin
  information, and package identity on a verified synchronization path.
- For a game-specific plugin with a stable tested process name, add and verify
  `BepInProcess` so the loader does not activate it in unrelated processes.
- At Harmony/game callback boundaries, make the declared client/host/server
  role explicit, fail closed when network role is unavailable, and contain
  failures from both the primary callback and its diagnostic sink. For
  all-or-nothing observations, treat missing entries, metadata, or
  classification fields as observation failure rather than partial success.

## Documentation and Markdown

- Apply `software-documentation-maintenance` and require `docs/README.md`,
  `docs/domain/README.md`, `docs/architecture/README.md`, and
  `docs/operations/README.md`. Link the developer entry point from the root
  README. Keep base-game, loader, Harmony, Unity, networking, and package-host
  evidence in `domain/`; keep mod-selected models, boundaries, workflows, and
  failure policy in `architecture/`; keep verified setup, locked restore,
  formatting, build, debugging, packaging, release, runtime validation, and
  Thunderstore procedures in `operations/`.
- Keep contribution requirements and the matching check matrix in
  `CONTRIBUTING.md`. Link to operations instead of duplicating full procedures.
- Add `.markdownlint-cli2.yaml` that lints committed `**/*.md` by default,
  respects `.gitignore`, and excludes only transient agent/worktree locations.
  Keep every rule exception documented and narrow: line length, nested-list
  indentation, repeated changelog headings, and PR-template first-line rules
  are typical justified exceptions.
- Run Markdown lint for README, changelog, package assets, docs, and GitHub
  templates. A pinned Docker image with disabled network access is a valid local
  path; CI may use a full-SHA-pinned Markdown action. Use the repository's
  configuration in either case.
- Keep developer-facing prose in English unless the repository defines another
  policy. Distinguish developer changelog history from user-facing package
  release notes.

## GitHub repository and CI

- Configure GitHub Actions to require full-length commit-SHA pins when the
  repository or organization setting is available. Keep a verified version
  comment next to every third-party action reference. Pin container images by
  digest and verify checksums before executing downloaded tools.
- Use a dedicated lint workflow for pushes, pull requests, merge queues when
  used, and manual dispatch. Set read-only workflow permissions, explicit Bash
  defaults, and a concurrency group that cancels obsolete pull-request runs but
  never cancels an active release.
- Keep CI stages ordered and reproducible: checkout; install/check external
  linters; actionlint and ShellCheck; action-pin verification; .NET setup;
  locked restore; `dotnet format --no-restore --verify-no-changes`; build; and
  Markdown lint. Run ShellCheck before actionlint when actionlint can use it
  for inline shell validation.
- Keep documentation, checked-in lint/check configuration, and CI in one
  executable contract. Every retained configuration must be consumed by a
  documented local command and an enabled CI step; remove stale configuration
  or unsupported promises instead of treating file presence as enforcement.
- Use `pinact run --check --min-age 7`, `actionlint`, and ShellCheck for
  workflow or composite-action changes. Cache downloaded tool archives only;
  verify their checksum on every extraction. Keep the tool version and checksum
  adjacent and update them as one supply-chain-reviewed change.
- Cache NuGet using the committed lockfile path. Keep the .NET SDK version,
  target framework, and CI documentation aligned. Do not rely on the ambient
  runner toolchain for release-critical behavior.
- Install the `global.json` SDK explicitly in CI with a full-SHA-pinned setup
  action or pinned verified equivalent, then assert `dotnet --version` before
  locked restore.
- Review GitHub repository settings in addition to workflow YAML: Actions
  source restrictions and SHA-pin enforcement, default token permissions,
  branch protection/required checks, immutable releases, and release secrets.
  Record settings that cannot be inspected as verification gaps.

## Composite actions

- Prefer an exact bundled action from this Skill when its documented contract
  matches the repository. Apply and verify it with the template sync script;
  keep project paths, package names, categories, credentials, and other local
  values in the calling workflow. Do not edit the copied action locally.
- Extract repeated versioning, packaging, or publishing behavior into a local
  composite action only when it has a stable repository-wide contract. Give it
  explicit inputs and outputs, Bash `set -euo pipefail`, and focused validation
  for missing inputs and unexpected artifact counts.
- A version action should classify stable, prerelease, and edge modes from a
  documented project-version source. It must keep the compiled DLL, artifact
  name, Git tag, and package manifest aligned without applying a prerelease
  version where the package host rejects it.
- A publishing action must accept a prebuilt artifact, not rebuild it. Pass
  credentials through environment variables scoped to that one step, avoid
  logging them, and return only non-sensitive URLs or identifiers.
- Keep composite actions small. Put reusable game/framework behavior in C#;
  keep only CI orchestration and deterministic repository tasks in YAML or
  shell. Test changed action scripts with ShellCheck and their calling workflow
  with actionlint and pinact.
- Add the selected template IDs and `sync_templates.ps1 -Check` command to
  contributor documentation and CI. A repository that needs a different
  contract must stop selecting that template and document the concrete reason;
  an untracked local edit is drift, not customization.

## Release automation

- Build the package once from the release commit after locked restore. Archive
  the DLL and required files, calculate a digest, and upload that artifact.
  Release jobs must download and verify this exact artifact instead of building
  a second copy.
- Keep a small, versioned archive contract in the repository. Define a
  host-neutral base with allowed root paths, an exact intended-plugin-DLL count,
  prohibited runtime/local/build contents, and archive path-safety checks.
  Add host-specific fields and layout rules only as an extension based on that
  host's authoritative documentation.
- Keep every archive-contract statement executable: map each required or
  prohibited property to a validator assertion and a representative failing
  fixture. Inspect link metadata when claiming unsafe-link rejection; path
  traversal checks alone are insufficient.
- Validate raw archive entry syntax before normalization, require regular-file
  entry types, and inspect the packaged managed assembly rather than trusting
  its filename. Verify assembly name, loader GUID/name/version, process
  restriction, and enabled manifest/archive version against produced metadata.
- Decode actual managed custom attributes for BepInEx identity, process, and
  dependency checks; byte-string presence is not semantic validation. Keep a
  positive fixture and a mutation fixture for every documented archive
  rejection branch.
- Set release-job-only `contents: write`; keep all other jobs read-only. Scope
  package-host tokens to the publishing step and never expose them to pull
  request or untrusted-code workflows.
- Enable immutable releases where GitHub supports them. Create a draft, attach
  every asset and checksum, then publish. Automation must fail if the target
  tag, release, or asset already exists; do not update published artifacts.
- Separate validation artifacts, prereleases, and stable releases. Version
  classification, tag creation, GitHub release visibility, and package-host
  publishing conditions must agree. Use concurrency keyed to the release/tag to
  prevent duplicate publication.
- Inspect the final ZIP before publishing: expected root layout, one intended
  DLL, manifest, README, changelog, icon, license, and no game/runtime DLLs,
  local paths, logs, or build intermediates.
- Inspect the contents of the packaged README/manifest/changelog themselves.
  Required identity, supported game/loader baseline, dependencies, and release
  version must appear in the files users receive and agree with the project and
  evidence ledger. If developer and publication changelogs are separate, trace
  the packaging input to the publication source; otherwise document the
  canonical changelog as intentionally dual-purpose.

## Thunderstore publishing

Apply this section only when the chosen package host is Thunderstore.

- Keep the package ZIP root compatible with Thunderstore's package contract:
  plugin DLL, `manifest.json`, `README.md`, `CHANGELOG.md`, icon, and license
  when required by the repository's package policy. Validate the final archive,
  not just the `assets/` source directory.
- Keep `manifest.json` name, version, website, description, and dependency
  strings deliberate. Dependency changes require the compatible loader/game
  baseline, installation impact, rollback risk, and test evidence. Put detailed
  compatibility evidence in package documentation and changelog; do not make
  claims solely from a successful build.
- Use a service-account `THUNDERSTORE_TOKEN` stored only as a GitHub secret.
  Pass it only to the stable release publishing step. Do not print it, write it
  to artifacts, or make it available to PR jobs.
- Submit exactly one prebuilt ZIP for one stable version. Fail on zero or
  multiple matches. Keep prerelease and edge builds on GitHub only if the
  selected Thunderstore workflow cannot represent those versions.
- Keep namespace, community, and category inputs explicit and reviewable.
  Treat a package upload as an external side effect: require a reviewed stable
  artifact, manifest/package inspection, runtime validation, and the intended
  release mode before submission.

## Completion review

- The ignore rules expose all source, assets, lockfiles, and documentation while
  excluding only validated local/generated paths.
- APM, if used, has a reviewed manifest, lockfile, generated deployment, and
  passing frozen-install/audit checks.
- NuGet sources are mapped, package locks are committed, and restore is locked.
- Markdown, C#, shell, workflow, pin, and package checks match the touched
  surface and are documented for contributors.
- CI pins all external executable inputs, uses least privilege, and publishes
  only the verified build artifact.
- GitHub and Thunderstore releases, when used, are immutable or explicitly
  marked with their residual risk, version-consistent, and backed by runtime
  compatibility evidence.
