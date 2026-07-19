# Agent Instructions

Repository-local Agent Skills are deployed to `.agents/skills/` by
[APM](https://github.com/microsoft/apm). Do not edit that generated directory
directly.

## APM-managed Skills

- `apm.yml` pins the selected public
  [aoirint/skills](https://github.com/aoirint/skills); `apm.lock.yaml` records
  their resolved commits and content hashes.
- A maintainer may explicitly direct this repository to use the current
  `aoirint/skills` main commit before the normal seven-day cooldown. Record the
  exception in the pull request and retain an exact full commit pin.
- To restore the committed Skill set, run `apm install --frozen` from the
  repository root, then run `apm audit --ci`.
- Make all Skill changes in the public
  [aoirint/skills](https://github.com/aoirint/skills) repository. This
  repository only selects, pins, and deploys those Skills.
- To update a Skill dependency, review its source, commit pin, license, and
  cooldown first. Update `apm.yml`, run `apm lock`, review `apm.lock.yaml`,
  run `apm install --frozen` and `apm audit --ci`, then commit the manifest,
  lockfile, and generated `.agents/skills/` changes together.

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
