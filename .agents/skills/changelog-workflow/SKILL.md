---
name: changelog-workflow
description:
  Maintain a canonical developer changelog and versioned release history. Use when adding or correcting Keep a Changelog
  entries, prerelease history, compatibility context, or release-source material; not for user-facing release notes or
  publishing.
---

# Changelog Workflow

Maintain the repository's canonical developer changelog, usually `CHANGELOG.md`. Keep it human-readable, newest first,
and distinct from user-facing release notes.

## Update a release entry

1. Locate the canonical changelog and confirm its role before editing.
2. Keep `Unreleased` first. Move entries to a versioned section only after the maintainer confirms the stable version
   and UTC release date; never add placeholder release headings.
3. Use Keep a Changelog categories where applicable: `Added`, `Changed`, `Deprecated`, `Removed`, `Fixed`, and
   `Security`. Put supplemental compatibility, test, limitation, or migration context under `Notes`, after change
   categories.
4. Use newest-first headings such as `## v1.2.3 - 2026-05-03 UTC`.
5. Record durable maintainer context: migrations, architecture, CI/build/package changes, prerelease history, security
   exceptions or residual risk, compatibility confidence, limitations, and the reason a user-visible change matters.
6. Source compatibility, support, environment, and migration claims. Mark unsourced claims as missing input or draft
   material; distinguish confirmed, best-effort, limited-check, and known-issue evidence. Group paired tools and
   dependencies with the product/version they qualify.
   When the same compatibility statement appears in multiple changelogs or
   user documentation, use an established canonical hierarchy and preserve its
   entries, nesting, dates, and reference links. Do not flatten a tested
   environment into a generic dependency label or delete supported-platform
   details to make copies look shorter.
7. Preserve historical accuracy. Mark backfilled metadata as backfilled, retain meaningful prerelease history, and use
   complete prerelease versions when they matter. Treat `Unreleased` as the prospective final release state: remove
   proposals, implementation iterations, and settings-category moves that were withdrawn or superseded within the same
   pull request. Record only the final user- or maintainer-visible change, such as the settings that exist after the
   change. Retain a withdrawn or superseded note only when a published artifact or a prior versioned entry makes that
   history durable.
8. Describe public package metadata from actual publication-channel history, not local or GitHub-only artifacts. Treat a
   newly public compatibility label as `Changed`; use `Fixed` only for a correction already exposed publicly.
9. Write concise factual bullets. Avoid issue or PR identifiers unless the maintainer needs the durable link. Use
   `prose-quality-check` for prose changes that risk losing confidence, relationship, or historical-status nuance.

## Handoff and boundaries

Prepare or correct canonical source material before user-facing release notes. If source material is complete, hand off
to `release-note-workflow`; if version, UTC date, target channel, or needed rationale is missing, report the gap. Do not
publish, tag, create releases, upload artifacts, or expose private paths, transcripts, or credentials.
