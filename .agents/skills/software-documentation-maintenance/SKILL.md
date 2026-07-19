---
name: software-documentation-maintenance
description: >-
  Design, restructure, and maintain a software repository's documentation
  system, including its required docs/domain, docs/architecture, and
  docs/operations map, canonical ownership, technical evidence, indexes, links,
  and change synchronization. Use when creating or migrating developer docs,
  documenting system behavior or design decisions, establishing documentation
  rules, or reviewing repository-wide documentation completeness. Pair with
  prose-quality-check for wording and with a domain skill for specialized
  technical correctness.
---

# Software Documentation Maintenance

Build a documentation system that lets each reader find the applicable truth,
understand its evidence and scope, and update it when the software changes.

## Contents

- [Goals](#goals)
- [Responsibility boundaries](#responsibility-boundaries)
- [Required documentation map](#required-documentation-map)
- [Workflow](#workflow)

## Goals

- Give every document a named audience, question, canonical owner, and change trigger.
- Separate external technical knowledge, internal design, and operations in a predictable layout.
- Keep claims synchronized with source code, configuration, schemas, assets, automation, and
  independently versioned dependencies.
- Migrate documentation without losing facts, rationale, links, or discoverability.

## Responsibility boundaries

This skill owns repository-level documentation architecture and technical-truth maintenance:

- which documents should exist and how readers discover them;
- which document canonically owns a fact;
- what evidence and version scope support a technical claim;
- how external knowledge, internal design, operations, product guidance, and governance relate;
- which implementation changes require documentation updates.

Use `prose-quality-check` after these decisions are settled to improve wording, paragraph and list
shape, audience fit, and preserved nuance. Prose review must not silently decide document ownership,
technical evidence, or migration coverage.

Resolve companion Skill names from the canonical metadata in the same Skill collection that
provides this Skill, not from an older deployed copy in the repository being documented. The
current prose-only companion is `prose-quality-check`. Treat any other name for it in target
instructions, manifests, lockfiles, generated deployments, or handoff text as one coordinated
migration finding; do not repeat a legacy consumer name as if it were current.

Use a domain-specific quality skill for specialized correctness. For example,
`bepinex-mono-mod-quality-check` owns which BepInEx, Harmony, Unity, game-version, networking,
Core/Interop, packaging, release, and Thunderstore facts are required and whether they are correct.
This skill owns their canonical placement, indexes, scope labels, links, and synchronization. When
both apply, establish the document map here, obtain or verify the technical facts with the domain
skill, then place them without duplicating their source of truth.

## Required documentation map

Require this base structure for a software repository:

```text
README.md
docs/
  README.md
  domain/
    README.md
  architecture/
    README.md
  operations/
    README.md
```

- `README.md` is the repository entry point. Link to `docs/README.md` and keep product-facing or
  contributor-facing entry information there rather than duplicating the developer index.
- `docs/README.md` defines the ownership boundaries and links all three section indexes.
- `docs/domain/` owns external and reusable knowledge about independently versioned APIs,
  protocols, frameworks, platforms, data formats, upstream products, and integration techniques.
- `docs/architecture/` owns this software's models, invariants, workflows, component boundaries,
  policies, tradeoffs, and accepted limitations.
- `docs/operations/` owns repeatable development, test, migration, deployment, release, recovery,
  incident, and artifact-generation procedures.

Keep all four indexes even when a section has no detailed document yet. State that it currently has
no entries and name the condition that would add one. This makes absence explicit instead of making
readers guess whether documentation is missing.

Extend the base map when a distinct audience or change lifecycle justifies it. Examples include
`docs/user/`, `docs/api/`, `docs/security/`, `docs/decisions/`, or nested concern directories. Add
each extension to `docs/README.md`, define its ownership against the three base sections, and avoid
parallel categories that could canonically own the same fact. Existing repositories may retain
clear equivalent names during a staged migration, but the final map must provide the required base
paths unless repository instructions explicitly document an approved exception.

## Workflow

### 1. Read and inventory

Read repository instructions, every documentation index, the root README, contributor guidance,
relevant implementation and configuration, and configured Markdown or link checks. Preserve local
terminology unless it conflicts with the required ownership boundaries.

For every current document and incoming fact, record:

- audience and question or task served;
- canonical facts owned and documents depended on;
- evidence source and version or environment scope;
- change trigger, such as an API, workflow, configuration, UI, or release-process change;
- discovery path from `README.md` through `docs/README.md` and the applicable section index.

Treat missing ownership, duplicated truth, undocumented dependencies, stale claims, and orphaned
documents as separate findings.

### 2. Classify at the fact boundary

Use the required map rather than a file's current name:

- Put facts that remain useful if this software is replaced by another integration in `domain/`.
- Put facts about this software's own design choices and behavior in `architecture/`.
- Put repeatable maintainer actions in `operations/`.
- Keep user contracts, installation, configuration, examples, troubleshooting, and compatibility in
  the root README or an indexed user-facing extension; link to technical detail instead of copying it.
- Keep contribution, review, security-reporting, and agent policy in their repository governance
  files; link to operations when a procedure is shared.

Keep each fact in one canonical place and link to it elsewhere. Split a mixed document when its
facts have different owners, audiences, evidence sources, or change triggers. Do not create a
concern document without a distinct question, but do retain each required section index.

### 3. Build indexes and concern documents

Make indexes navigational rather than duplicative. State the section boundary and name the question
answered by every child document. Ensure the root README exposes `docs/README.md`; do not rely on
contributors discovering `docs/` by browsing the tree.

Split concern documents when they change independently. Good architecture concerns include models
and invariants, workflows and lifecycles, component or integration boundaries, and consequential
decisions. Good domain concerns follow an independently versioned external dependency. Good
operations concerns follow a repeatable procedure with one trigger and verification contract.

### 4. Establish evidence and scope

Inspect authoritative source code, configuration, schemas, generated artifacts, automation,
serialized assets, or upstream specifications before asserting current behavior. Record an
applicable version, build, API level, environment, or deployment target once in a clear `Target` or
`Scope` section when truth varies. Separate:

- verified current behavior from inference;
- observed behavior from intended or proposed behavior;
- public contract from implementation detail;
- external-system facts from this product's policy or design choice.

Do not include machine-local paths, secrets, incidental generated output, or substantial copied
third-party source. Give enough evidence detail for a future maintainer to repeat the check. Inspect
configuration, schemas, declarative bindings, generated code, and assets when they can override or
complete executable behavior.

For `domain/`, document the members, messages, fields, ordering, ownership, failure behavior, or
lifecycle required to integrate safely. Replace the canonical concern document when its target
advances unless parallel targets are intentionally supported. Describe alternatives only when they
are credible, and distinguish evidence-derived integration constraints from this product's choice.

For `architecture/`, answer the applicable questions:

- **Model**: Which concepts, values, stores, and invariants exist, and what lifetime do they have?
- **Flow**: What initiates work, who owns transitions, and how do rejection, retries, partial
  completion, and cleanup behave?
- **Boundary**: Which dependencies and responsibilities belong to each component, and in which
  direction may they flow?
- **Decision**: Which credible alternatives existed, what was selected, and why does it fit the
  product constraints?

Start each architecture document by linking the domain documents it assumes. If its rationale
requires an undocumented external fact, add or update the domain document first. Describe known
defects and intended-but-unimplemented paths explicitly instead of presenting the desired design as
current behavior.

For `operations/`, state prerequisites, inputs, exact commands or actions where precision matters,
expected outputs, state-changing effects, rollback or recovery, verification, and the trigger for
updating the procedure. Verify commands, paths, environment variables, CI jobs, package contents,
and release targets against the repository.

### 5. Migrate without losing information

1. Create a section-level map from every source passage to its canonical destination.
2. Add the required directories, destinations, and indexes before deleting or renaming sources.
3. Preserve rationale, limitations, evidence status, and historical context; shorter text is not
   equivalent when it loses decisions or failure behavior.
4. Update root discovery, inbound and outbound links, repository instructions, examples,
   navigation, and generated or package-facing copies in the same change.
5. Search for stale paths, headings, Skill names, terminology, commands, and duplicated claims.
6. Delete an old document only after every still-valid fact has a destination and the new path is
   discoverable.

### 6. Verify and report

Run repository-configured Markdown, link, spelling, example, schema, build, or documentation-site
checks. Inspect the documentation diff against implementation and configuration changes. Verify:

- all required directories and indexes exist and their boundaries do not overlap;
- the root README links the developer documentation entry point;
- every maintained document is reachable through an index and answers one named question;
- every technical fact has one canonical owner and an identifiable evidence source;
- version and environment scope is explicit where truth varies;
- current, inferred, proposed, deprecated, and known-broken states are distinguishable;
- architecture links its external assumptions instead of duplicating them;
- operational steps state prerequisites, effects, failure behavior, and verification;
- all relative links, anchors, commands, code examples, and referenced paths resolve;
- renamed or removed documents and Skill names have no stale references;
- changed contracts, dependencies, workflows, and operations have corresponding documentation
  updates or an explicit reason why none is needed.

When reviewing rather than editing, report each finding with its location, violated ownership or
evidence rule, reader impact, and smallest corrective action. State explicitly when no issue is
found. On handoff, report the resulting map, checks performed, evidence gaps, and any extension or
approved exception with its reason.
