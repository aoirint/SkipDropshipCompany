# Agent Instructions

Repository-local Agent Skills are deployed to `.agents/skills/` by
[APM](https://github.com/microsoft/apm). Do not edit that generated directory
directly.

## APM-managed Skills

- `apm.yml` pins the selected public
  [aoirint/skills](https://github.com/aoirint/skills); `apm.lock.yaml` records
  their resolved commits and content hashes.
- The initial pin is an explicit maintainer-approved exception to the normal
  seven-day dependency cooldown.
- To restore the committed Skill set, run `apm install --frozen` from the
  repository root, then run `apm audit --ci`.
- Make all Skill changes in the public
  [aoirint/skills](https://github.com/aoirint/skills) repository. This
  repository only selects, pins, and deploys those Skills.
- To update a Skill dependency, review its source, commit pin, license, and
  cooldown first. Update `apm.yml`, run `apm lock`, review `apm.lock.yaml`,
  run `apm install --frozen` and `apm audit --ci`, then commit the manifest,
  lockfile, and generated `.agents/skills/` changes together.

## Documentation Boundaries

`docs/domain/` contains versioned base-game and reusable implementation
knowledge without SkipDropshipCompany-specific product decisions.
`docs/architecture/` contains the mod's models, logic, workflows,
responsibilities, and design decisions; it links to the domain knowledge it
uses. Add a new domain document when an architecture document needs knowledge
not already documented there. Do not duplicate base-game analysis in
architecture documents.

## Documentation Skill

Use `.agents/skills/mod-documentation-quality-check/` when creating, restructuring,
maintaining, or reviewing developer documentation.

## Icon Assets

When changing `assets/icon.svg` or regenerating `assets/icon.png`, follow
`docs/icon-authoring.md`.
