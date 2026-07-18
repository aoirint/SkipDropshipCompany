---
name: maintain-mod-documentation
description: Create, restructure, and maintain scalable developer documentation for a software mod or plugin. Use when organizing docs into game/framework domain knowledge and mod-specific architecture, documenting implementation decisions, replacing monolithic architecture documents, or establishing repository rules for developer documentation.
---

# Maintain Mod Documentation

Build documentation that lets a developer change a mod without rediscovering
its external dependencies or guessing the mod's own design.

## Classify before writing

Inventory existing documentation and the implementation it describes. For each
fact, apply this ownership test:

- **Domain**: it remains useful if this mod is replaced by a different mod that
  integrates with the same game, framework, or external system.
- **Architecture**: it explains a model, workflow, boundary, policy, or tradeoff
  chosen by this mod.
- **Operations**: it is a repeatable maintainer procedure, such as packaging,
  asset authoring, release, or runtime validation.

Do not classify by a file's current name. Split mixed documents at the fact
boundary. Keep a fact in one canonical place and link to it elsewhere.

## Establish a scalable document map

Use only the directories justified by the repository. Prefer this layout:

```text
docs/
  README.md                 # developer-doc entry point and boundary rules
  domain/                   # external and reusable implementation knowledge
    README.md
  architecture/             # mod-specific design
    README.md
  operations/               # optional repeatable maintainer procedures
```

Keep `docs/README.md` short. It must state the ownership rule, direct readers
to the indexes, and say that an architecture need for undocumented external
knowledge requires a new domain document first. Add `operations/` only when
there are operational documents that benefit from a shared index; do not move
a single focused procedure just to fill the directory.

Make `domain/README.md` and `architecture/README.md` indexes, not duplicate
summaries. Each index names the question answered by every document.

For a small mod, an architecture index plus one concern-specific architecture
document is sufficient. For a larger mod, split only when concerns have
different change triggers or audiences. Good architecture boundaries include:

- model and invariants;
- workflow or lifecycle;
- layer and integration boundaries;
- a consequential design decision that needs alternatives and rationale.

Do not create a template-shaped file with no distinct question to answer.

## Write domain documents

Write one domain document per independently versioned external concern. Record
the target version/build when the facts are version-specific. Group relevant
implementation members by owning type and give names, signatures, field types,
and roles sufficient for Harmony or reflection access.

Inspect the target implementation, and applicable serialized assets when they
can override the relevant behaviour, before stating an external claim. Separate
verified behaviour from an inference or an intentionally incomplete probe. Do
not include local paths, machine-specific data, or substantial decompiled
source text. Replace findings in the existing concern document when its target
version changes; do not create a version-suffixed copy by default.

Separate these sections when applicable:

1. **Target**: product/version/build scope.
2. **Implementation reference**: members to inspect or access.
3. **Behaviour or lifecycle**: externally observed code behaviour.
4. **Implementation choices**: alternative ways to integrate with that external
   system.

For every meaningful external-integration choice, use:

```md
### <decision>

#### <option>

<conditions, effects, and rationale>
```

Make the recommended option explicit only when the source evidence supports
it. Explain why the alternatives do not meet the same state, timing, ownership,
or synchronization boundary. Do not put this mod's UX, feature policy, or
internal model in a domain document.

## Write architecture documents

Start every architecture document by linking the domain documents it assumes.
Describe the mod's own concepts and choices rather than copying target-game
declarations.

For each concern, answer only the questions that matter:

- **Model**: What values or stores exist? Which invariants and lifetime apply?
- **Flow**: Which component initiates work, which owns each transition, and
  what happens on rejection or failure?
- **Boundary**: What belongs in Core/application logic versus framework/game
  interop? Which direction may dependencies flow?
- **Decision**: What alternatives existed, which one is used, and why does that
  choice fit this mod's goals?

Link to domain documents for base-game facts. If the architecture cannot state
its rationale without explaining a new base-game fact, add or extend the domain
document first, then link to it. Keep operational procedures out of
architecture unless they are part of the runtime design.

## Migrate and maintain safely

1. Plan the destination and canonical owner of every section before editing.
2. Read the source and existing documentation; do not infer external behaviour
   from mod code when the target implementation can be inspected.
3. Add the destination documents and indexes before deleting a monolith.
4. Update all links, repository instructions, and top-level documentation
   entries in the same change.
5. Delete the old document only after every part has a canonical destination.
6. Preserve history with focused commits; do not amend unrelated commits.

When repository instructions guide future contributors, add a concise version
of the ownership rule there. Do not paste the entire skill into repository
instructions.

## Review checklist

Before handing off, verify all of the following:

- Every technical fact has one canonical owner.
- Domain documents contain no mod-specific product decision.
- Architecture documents do not duplicate external implementation analysis.
- Each architecture document has a distinct developer question and links to
  the domain knowledge it uses.
- Version-specific domain facts name their target version/build.
- Implementation choices use H3 decisions and H4 options, with reasons.
- All relative Markdown links resolve and no removed document is referenced.
- The repository instructions and documentation indexes describe the final
  layout.

When reviewing rather than editing, report each failed checklist item with its
location, the problem, and the smallest corrective action. State explicitly
when no issue is found.

Report the resulting map, validation performed, and any intentionally omitted
directory or split with its reason.
