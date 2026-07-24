# Agent Instructions

Repository-local Agent Skills are deployed to `.agents/skills/` by
[APM](https://github.com/microsoft/apm). Do not edit that generated directory
directly.

## APM-managed Skills

- `apm.yml` pins the selected public
  [aoirint/skills](https://github.com/aoirint/skills); `apm.lock.yaml` records
  their resolved commits and content hashes.
- Keep this unpublished APM project at `version: 0.0.0` until its
  distribution and versioning design is explicitly decided.
- Use APM CLI 0.26.0 for lock operations. Its normal seven-day cooldown was
  explicitly waived because it fixes virtual-package `config-consistency`
  audit failures. The waiver covers only the CLI release time gate.
- A maintainer may explicitly waive the normal seven-day wait for a directly
  selected current `aoirint/skills` main commit. Record the waiver and exact
  full commit SHA in the pull request.
- That waiver applies only to the direct `aoirint/skills` commit selection. It
  does not cover dependencies of `aoirint/skills`; review those dependencies
  and enforce their cooldown independently.
- To restore the committed Skill set, run `apm install --frozen` from the
  repository root, then run `apm audit --ci`.
- Make all Skill changes in the public
  [aoirint/skills](https://github.com/aoirint/skills) repository. This
  repository only selects, pins, and deploys those Skills.
- To update a Skill dependency, review its source, commit pin, license, and
  cooldown first. Update `apm.yml`, remove only the validated project lock,
  regenerate it with APM 0.26.0, then run `apm install --frozen` and
  `apm audit --ci`. Commit the manifest, lockfile, notices, and generated
  `.agents/skills/` changes together.

## Markdown Checks

Use pnpm 11 or newer. Keep the exact package pin and all fail-closed cooldown
settings when reproducing the Markdown check locally:

```shell
pnpm \
  --config.minimumReleaseAge=10080 \
  --config.minimumReleaseAgeStrict=true \
  --config.minimumReleaseAgeIgnoreMissingTime=false \
  --config.minimumReleaseAgeExclude= \
  dlx markdownlint-cli2@0.22.0 \
  --config .markdownlint-cli2.yaml \
  '**/*.md'
```

Add `--fix` after the package version to apply supported automatic fixes, then
run the normal command again. Some rules, including prose line length, still
require a meaning-preserving manual edit.

## Pull Request Merges

- Merge pull requests with squash merge.
- Before confirming the merge, set the squash commit title to
  `<pull request title> (#<number>)`, including the pull request number as in
  GitHub's default squash-merge title.

## Documentation Boundaries

`docs/domain/` contains versioned base-game and reusable implementation
knowledge without SkipDropshipCompany-specific product decisions.
`docs/architecture/` contains the mod's models, logic, workflows,
responsibilities, and design decisions; it links to the domain knowledge it
uses. Add a new domain document when an architecture document needs knowledge
not already documented there. Do not duplicate base-game analysis in
architecture documents.

## Documentation Skill

Use `.agents/skills/software-documentation-maintenance/` when creating, restructuring,
maintaining, or reviewing developer documentation. Use
`.agents/skills/prose-quality-check/` when refining explanatory wording after the
document owner and technical evidence are established.

## Icon Assets

When changing `assets/icon.svg` or regenerating `assets/icon.png`, follow
`docs/icon-authoring.md`.
