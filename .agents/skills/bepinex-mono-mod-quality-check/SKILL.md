---
name: bepinex-mono-mod-quality-check
description:
  Review or create a BepInEx Mono mod for C# project and module structure, plugin identity, game compatibility,
  dependency boundaries, repository-family alignment, GitHub CI and release automation, package readiness, release
  metadata, and verification. Use when implementing, reviewing, preparing a release for, diagnosing quality gaps in,
  or applying a minimal-difference peer rollout to a BepInEx 5 Mono plugin or its build and release workflow.
---

# BepInEx Mono Mod Quality Check

## Goals

- Preserve a loader-visible plugin identity that agrees with the assembly, generated metadata, and package metadata.
- Keep C# project and module boundaries aligned with gameplay policy, framework integration, and composition
  responsibilities.
- Keep game and BepInEx compatibility explicit, evidence-backed, and scoped to what was actually tested.
- Produce reproducible builds and installable release packages without committing local game or build output.
- Keep GitHub Actions and release automation pinned, least-privilege, and traceable to the built artifact.
- Report findings as actionable evidence, not assumed conventions from another mod.

## Common requirements

- Use the Skill baseline unless the repository has stricter equivalent rules.
  Repository files supply target facts; missing required files and unverified
  dependencies or archives remain findings, with only concrete recorded exceptions.
- Establish the evidence ledger before target-specific choices. Mark unavailable
  facts `blocked`; stop only dependent branches and continue independent work.
- Preserve plugin identity, compile-only runtime references, explicit NuGet
  sources/mapping and project lockfiles, the selected SDK, and applicable locked
  restore, format, build, test, Markdown, APM, CI, archive, and runtime gates.
- Require source/publisher/version/hash/license/transitive review and seven-day
  eligibility for adopted external code. Use `security-check` for these surfaces
  and for private-resource disclosure boundaries.
- Keep runtime compatibility, package readiness, and publication authorization
  distinct. Do not infer game/loader/host/license/credential facts or approval
  from another repository, successful compilation, or a stable version.
- Keep shared template ownership in canonical `assets/`; use authoring-time
  synchronization and drift checks. Consumer CI executes committed repository
  tooling and must not depend on the installed Skill.
- Use `domain-architecture-docs-workflow` for documentation ownership,
  `prose-quality-check` after facts are settled, `unity-game-analyze` for target
  code/asset evidence, `github-actions-quality-check` for shared workflow rules,
  `github-workflow` for settings, and `apm-workflow` when APM is enabled.

## Workflow

1. Classify the request as `setup`, `alignment`, `implementation`,
   `release-readiness`, or `plan-only`. Follow
   [implementation-runbook.md](references/implementation-runbook.md) in order.
   It owns the evidence ledger, required file inventory, dependency order,
   conditional branches, verification matrix, and completion report. Do not
   replace it with an informal checklist.
2. Read [repository contract and baseline](references/repository-contract.md)
   to establish repository facts, responsibilities, required quality controls,
   and any enabled family/template/game-version branches before proposing changes.
3. Select the operation and load the enabled detail below. Preserve the runbook's
   dependency order; a request type does not waive an applicable baseline gate.

   | Request type | Required detail |
   | --- | --- |
   | `setup` or `alignment` | [Project/plugin](references/project-and-plugin.md), [build/dependencies](references/build-and-dependencies.md), and [package/release](references/package-and-release.md) checks for every enabled inventory branch |
   | `implementation` | [Project/plugin](references/project-and-plugin.md) and [build/dependencies](references/build-and-dependencies.md); [package/release](references/package-and-release.md) when package, release, or compatibility contracts are affected |
   | `release-readiness` | [Package/release](references/package-and-release.md) and [build/dependencies](references/build-and-dependencies.md); [project/plugin](references/project-and-plugin.md) for identity, boundary, and compatibility evidence |
   | `plan-only` | The same inventory, family delta ledger when applicable, and evidence ledger; load detail for the planned surfaces, label uninspected items `unverified`, and do not claim checks passed |

4. Run enabled verification from the runbook and
   [verification/reporting](references/verification-and-reporting.md). Record
   every disabled branch, omitted baseline item, skipped command, and concrete
   reason. Missing runtime, final-artifact, setting, or host evidence cannot
   become a compatibility or release-readiness pass.
5. Use the runbook's ordered completion report, with findings grouped by
   structure/plugin/build/CI/package/runtime, checks passed or skipped, and
   remaining risks. Keep current visibility and publication authorization intact.

## Reference Baseline

Read [repository-derived-baseline.md](references/repository-derived-baseline.md)
for a new project or missing documented structure/quality gates; it records
transferable practices rather than replacing the target contract.

Read [repository-quality-template.md](references/repository-quality-template.md)
for setup or alignment, after the runbook inventory. It is the normative
file-by-file standard for ignore rules, APM, NuGet, Markdown, CI, composite
actions, and conditional Thunderstore publishing.

Read [repository-family-alignment.md](references/repository-family-alignment.md)
when peers are designated or a minimal-difference rollout is requested. Preserve
its delta ledger, privacy boundary, canonical-first improvement rule, newline
review, and independent convergence loop.

Read [canonical-templates.md](references/canonical-templates.md) when an enabled
contract can adopt an exact bundled action or script; it owns template selection,
synchronization, drift checks, and multi-repository maintenance.

Read [guest-feature-authorization.md](references/guest-feature-authorization.md)
before choosing authorization transport, settings, lifetime, or tests for guest
features that can substantially change game balance. Non-host use defaults to
denied; a local guest setting cannot grant authoritative host consent.
