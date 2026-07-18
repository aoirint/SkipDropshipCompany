# Agent Instructions

Use repository-local Agent Skills from:

- `.agents/skills/`

## Documentation Boundaries

`docs/domain/` contains versioned base-game and reusable implementation
knowledge without SkipDropshipCompany-specific product decisions.
`docs/architecture/` contains the mod's models, logic, workflows,
responsibilities, and design decisions; it links to the domain knowledge it
uses. Add a new domain document when an architecture document needs knowledge
not already documented there. Do not duplicate base-game analysis in
architecture documents.

## Icon Assets

When changing `assets/icon.svg` or regenerating `assets/icon.png`, follow
`docs/icon-authoring.md`.
