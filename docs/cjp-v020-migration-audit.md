<!-- SPDX-License-Identifier: MIT -->

# CJP v0.2.0 Migration Audit

This audit records the review state for the CruiserJumpPractice `v0.1.4..v0.2.0`
internal-improvement migration into SkipDropshipCompany.

## Pull Request Stack

- PR-01: Agent Skills and `AGENTS.md`.
- PR-02: repository governance files and agent worktree ignore rule.
- PR-03: build foundation, package source mapping, generated plugin metadata,
  and version-action manifest behavior.
- PR-04: Lethal Company v81.5 dependency and compatibility baseline.
- PR-05: final compile-only package pinning and lockfile refresh.
- PR-06: stable-release Thunderstore publishing automation.
- PR-07: developer changelog, Thunderstore release-note role, Markdown lint, and
  documentation policy.
- PR-08: plugin composition root and Harmony patch installation boundary.
- PR-09: release-validation runbook and structured logging decision.
- PR-10: this integration audit.

## Cross-Issue Consistency

- Later branches are stacked on the PR-01 Agent Skills base.
- Build and dependency changes are split so package-source mapping lands before
  the Lethal Company v81.5 baseline.
- Stable Thunderstore publishing depends on the version-action manifest behavior
  from the build-hardening stages.
- Documentation describes stable Thunderstore publishing and GitHub-only
  prereleases after the release automation stage.
- Runtime refactoring happens after the build baseline and avoids documentation
  or CI churn.
- Validation planning builds on the small composition root but does not add
  runtime logging before maintainer review.

## Non-Applicability Decisions

- CruiserJumpPractice gameplay features were not ported.
- InputUtils, LethalNetworkAPI, OdinSerializer, cruiser state, magnet, keybind,
  and network behavior patterns were not ported because SkipDropshipCompany has
  no matching feature surface.
- A broad Core/Interop/use-case architecture split was not ported because the
  current SkipDropshipCompany runtime is small enough that a composition root
  and Harmony installer provide the useful boundary without extra layers.
- Structured validation logging was deferred. The runbook records a future
  `[SDC_VALIDATION]` JSONL direction if validation evidence later needs parser-
  ready logs.

## Verification

Final local checks on the full stacked branch:

- `dotnet restore SkipDropshipCompany/SkipDropshipCompany.csproj --locked-mode`
- `dotnet build SkipDropshipCompany/SkipDropshipCompany.csproj --configuration Release --no-restore`
- `dotnet format SkipDropshipCompany/SkipDropshipCompany.csproj --no-restore --verify-no-changes`

Not run locally:

- Markdown lint, because Node.js and `markdownlint-cli2` are not installed in
  this environment. The pinned GitHub Actions workflow should run it in CI.
- Thunderstore publish, because it requires `THUNDERSTORE_TOKEN` and should run
  only from the stable release workflow.
- In-game smoke testing, because a Lethal Company runtime environment is not
  available in this workspace.
