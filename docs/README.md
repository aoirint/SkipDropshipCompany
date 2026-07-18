# Developer Documentation

## Documentation boundaries

- `domain/` contains versioned Lethal Company and reusable implementation
  knowledge. It must not prescribe this mod's product behaviour, model, or
  design choices.
- `architecture/` contains this mod's models, workflows, responsibilities,
  and design decisions. It links to the domain knowledge it relies on.
- Keep a domain document focused on one game or technical concern. Add a new
  domain document when an architecture document needs knowledge not already
  covered there.
- Keep an architecture document focused on one mod concern. Do not copy
  base-game member declarations or behaviour analysis into it; link to the
  relevant domain document instead.

## Focused procedure

- [Icon authoring](icon-authoring.md) describes the package icon source and
  regeneration workflow. It remains a standalone document because it is the
  only operational procedure in this repository.

Start with [architecture/README.md](architecture/README.md) for the mod design,
and [domain/README.md](domain/README.md) for supporting knowledge.

