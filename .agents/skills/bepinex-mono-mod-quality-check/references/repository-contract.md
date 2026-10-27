# BepInEx Repository Contract and Baseline

## Contents

- [Documentation responsibility](#documentation-responsibility)
- [Required Template Baseline](#required-template-baseline)
- [Establish the repository contract](#establish-the-repository-contract)

## Documentation responsibility

Use `domain-architecture-docs-workflow` to create and maintain the required `docs/domain/`, `docs/architecture/`, and
`docs/operations/` map, its indexes, canonical ownership, migrations, and links. This Skill remains responsible for the
BepInEx-specific content and correctness placed in that map:

- `domain/` records the exact tested game, loader, Harmony, Unity, networking, serialized-asset, and package-host facts
  needed by the integration.
- `architecture/` records the mod's Core/Interop boundaries, callback and authority policy, state lifetime,
  transactions, failure behavior, and product-selected integration choices.
- `operations/` records the verified restore, build, runtime validation, packaging, release, and Thunderstore procedures
  selected by the repository.

Apply `prose-quality-check` only after ownership and technical evidence are settled. Do not move a mod-selected policy
into reusable domain knowledge, and do not treat a well-structured document as proof that its game or release claim is
correct.

Use `github-actions-quality-check` for the shared event, trust, workflow/job permission, concurrency, runner,
action-pin, and artifact-lineage baseline. Use `github-workflow` for repository settings and enforcement. This Skill
owns only the BepInEx/.NET/Thunderstore commands, release identity, package contract, and domain-specific templates
layered on those baselines.

For an APM-managed repository, apply `apm-workflow`. Keep `apm audit --ci` and Markdown lint in the outer
`check-dotnet-bepinex-source` action so every event-owned entry workflow enforces the same repository baseline.

Use `unity-game-analyze` to establish version-specific game-code and serialized-asset evidence: the concrete call path,
effective prefab/scene values, object/load reachability, lifecycle timing, authority, and unresolved runtime inputs.
This Skill consumes that evidence to select and verify hooks, adapters, identity mappings, compatibility claims, and
tests. It remains responsible for the mod decision; `unity-game-analyze` must not approve a Harmony target merely
because a method exists.

## Required Template Baseline

Use the quality baseline in this Skill for a repository that does not already have stricter, equivalent rules.
Repository files establish target-specific facts such as game build, loader version, package host, and release
namespace; they do not replace this Skill's quality bar.

- Commit an SDK-style project, `nuget.config` with explicit sources and package source mapping, `global.json` selecting
  the release SDK, one `packages.lock.json` per resolving project, a narrow `.gitignore`, Markdown lint configuration,
  and contributor documentation.
- Keep the contributor agreement and the pull-request confirmation internally complete. Adopt the bundled CLA contract
  only after the maintainer explicitly selects the project license and contribution terms; otherwise use
  license-neutral contributor guidance without a CLA checkbox. When adopting the bundled contract, deploy
  `repository-contributing` and `github-pull-request-template` together; never infer that a CLA checkbox is unsupported
  without inspecting its referenced `CONTRIBUTING.md` section.
- For C# changes, require locked restore, format verification without restore, and a no-restore build. For Markdown,
  workflow, shell, APM, package, or release changes, apply the corresponding checks in
  [repository-quality-template.md](repository-quality-template.md).
- Apply the event-owned workflow, direct-job-graph, read-only planning, permission, concurrency, and artifact-lineage
  rules from `github-actions-quality-check`. This Skill supplies the BepInEx-specific source gate, release identity,
  archive, edge-build, and Thunderstore extensions.
- When the target adopts a bundled CI or publishing contract, copy its files exactly from this Skill's canonical
  `assets/` and verify them from the installed Skill during authoring and review. Consumer CI must run only committed
  repository-owned actions and scripts; it must not execute `.agents/skills/` or require this Skill at workflow
  runtime. Follow
  [canonical-templates.md](canonical-templates.md) for selection, synchronization, validation, and rollout;
  do not fork a template silently.
- Treat a missing required file, unpinned external executable input, unreviewed package source, absent lockfile, or
  unverified final archive as a finding; do not downgrade it to a repository preference.
- Provide a global `Enabled` configuration setting by default. It must disable the mod's declared functionality without
  implying that BepInEx unloads the plugin. Omit it only when the mod's size, lifecycle, or implementation constraints
  make a coherent global gate impractical; record that concrete constraint and the available narrower controls.
- Allow a target repository to add stricter checks. Record a concrete compatibility or host constraint before omitting a
  baseline item.

## Establish the repository contract

1. Establish the repository contract before proposing changes.
   - Read `AGENTS.md`, `README.md`, `CONTRIBUTING.md`, the project file, lockfile, package assets, and relevant CI or
     release scripts to find target-specific facts and existing stricter rules. Do not use their absence as a reason to
     omit this Skill's baseline.
   - Create the evidence ledger from the runbook before selecting a target framework, loader references, plugin
     identity, package metadata, archive layout, or publish destination. Mark an unavailable fact `blocked`; never fill
     it from another BepInEx repository.
   - When the maintainer designates peer repositories or requests horizontal rollout, follow
     [repository-family-alignment.md](repository-family-alignment.md) before applying the abstract baseline.
     Require a disposition for every missing, extra, changed, or newline-different target path. Reuse portable shared
     content exactly unless a concrete product, runtime, test, package-host, repository-visibility, or maintenance
     constraint requires the smallest possible difference.
   - Treat a requested game-version change as a compatibility alignment. Record the old and target version, Steam
     manifest/build identifier, matching managed-code and serialized-asset evidence handoff hashes, game-reference
     package version, every compatibility claim, and the required build and runtime checks. Do not claim the target
     version when code and asset evidence come from different builds.
   - Classify the request as `setup`, `alignment`, `implementation`, `release-readiness`, or `plan-only`. Execute only
     the runbook branches enabled by the evidence ledger and request type; record every disabled branch and its concrete
     reason.
   - For a new repository or a template-alignment review, read
     [repository-quality-template.md](repository-quality-template.md) after the runbook inventory. It is the
     normative file-by-file standard, not permission to fork a designated repository family's portable conventions.
   - If the evidence ledger enables a contract represented by a bundled template, read
     [canonical-templates.md](canonical-templates.md), select only the applicable template IDs, and use the
     sync script for application and authoring-time drift checks. Keep target-specific workflow values in the caller,
     not in a copied action. Do not make consumer CI depend on the installed Skill.
   - When a patch, reflection target, serialized name/value, Unity object, game lifecycle, or build-compatibility claim
     lacks a closed target-build trace, use `unity-game-analyze` and record its evidence handoff. Do not reconstruct the
     Unity code/asset procedure informally inside the mod review.
