---
name: release-note-workflow
description:
  Create, update, or review user-facing stable release notes and publication readiness. Use when deriving stable notes
  from a canonical changelog or checking release readiness; not for canonical changelog authoring or publishing.
---

# Release Note Workflow

Derive user-facing stable notes from the canonical developer changelog; do not create a competing source of truth.

## Prepare stable notes

1. Locate the canonical changelog, user-facing notes destination, version source, publication channel and format, tag
   convention, release workflow, and verification command. Prefer maintainer input, published docs, existing history,
   workflow/config behavior, then scripts or metadata. Report conflicts or missing evidence as blockers.
2. Read the exact canonical stable-release section. If it, the confirmed stable version, or UTC date is missing, produce
   clearly labeled draft review text and list the missing inputs; never use a release-ready placeholder heading.
3. Confirm that the release is stable. Keep prerelease-only detail in the developer changelog unless the publication
   channel explicitly supports prerelease notes; roll still-relevant prerelease work into the next stable release.
4. Rewrite for users: behavior, installation or update impact, compatibility, breaking changes, removals, deprecations,
   security, known limitations, and one sourced reason or background sentence per user-visible change. Mark
   insufficiently sourced items as draft rather than inventing rationale.
5. Describe public package metadata using the target publication channel's history. A first public compatibility label
   is `Changed`, while `Fixed` requires a publicly exposed incorrect label. Keep internal correction history in the
   changelog.
6. Use `Notes`, after change categories, for compatibility and limitation metadata unless the channel requires another
   layout. Source claims, preserve confidence levels, and group paired tools or dependencies with their tested
   product/version. Exclude test dependencies, diagnostics, and issue/PR IDs unless users need them.
7. Use `prose-quality-check` for nuanced prose and `security-check` for security- or supply-chain-sensitive content.

## Check readiness

Before publication, confirm the version and documented tag convention, local and remote tag availability, required note
format, release-workflow inclusion, and documented verification result. Treat missing stable metadata, tag convention or
remote-tag verification, publication format/destination, prerelease-policy evidence, workflow inclusion, or failed
verification as blockers. Do not publish, create tags or releases, or expose private paths, transcripts, or credentials.

For review-only requests, make no edits and return a compact status matrix for the source section, notes destination,
version and tags, channel and format, workflow inclusion, verification, required updates, and blockers.
