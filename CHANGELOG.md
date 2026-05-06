<!-- SPDX-License-Identifier: MIT -->

# Changelog

All notable changes to this project are documented in this file.

This changelog is the canonical developer-facing release history. The
Thunderstore-facing package changelog in `assets/CHANGELOG.md` is derived from
stable release entries in this file and rewritten for users.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

## Unreleased

### Changed

- Added opt-in structured validation logging and a release-validation runbook
  for v0.2.0 release-candidate checks.
- Ported internal repository, build, release, and documentation maintenance
  practices from CruiserJumpPractice's `v0.2.0` preparation work.
- Updated the tested compatibility baseline to Lethal Company v81.5.
- Prepared stable-release automation for GitHub Releases and Thunderstore.

### Notes

- Compatibility:
    - Compatible with Lethal Company v81.5 (2026-04-17 UTC, Manifest ID:
      `6423525044216269478`).
        - The v81.5 test environment used BepInExPack v5.4.2305.
- Older Lethal Company versions are no longer claimed as tested by the current
  v0.2.0 release notes; Lethal Company v73 compatibility is recorded in the
  historical `v0.1.x` release entries below.

## v0.1.7 - 2025-11-23 UTC

### Fixed

- Fixed some typos in README.

### Notes

- Compatibility:
    - Compatible with Lethal Company v73 (2025-10-04 UTC, Manifest ID:
      `1749099131234587692`).
        - Backfilled as reference compatibility information while preparing
          the v0.2.0 release.

## v0.1.6 - 2025-11-23 UTC

### Added

- Added the `Enabled` config option to enable or disable this mod without
  uninstalling it.

### Notes

- Compatibility:
    - Compatible with Lethal Company v73 (2025-10-04 UTC, Manifest ID:
      `1749099131234587692`).
        - Backfilled as reference compatibility information while preparing
          the v0.2.0 release.

## v0.1.4 - 2025-11-23 UTC

### Added

- Initial Thunderstore release.

### Notes

- Compatibility:
    - Compatible with Lethal Company v73 (2025-10-04 UTC, Manifest ID:
      `1749099131234587692`).
        - Backfilled as reference compatibility information while preparing
          the v0.2.0 release.
