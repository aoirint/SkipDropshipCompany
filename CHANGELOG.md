# Changelog

All notable changes to this project are documented in this file.

This changelog is the canonical developer-facing release history. The
Thunderstore-facing package changelog in `assets/CHANGELOG.md` is derived from
stable release entries in this file and rewritten for users.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

## Unreleased

## v0.2.1 - 2026-05-08 UTC

### Fixed

- Prepared `v0.2.1` as a patch release for the stable `v0.2.0` package
  content.
    - The `v0.2.0` GitHub release tag already exists.
    - The `v0.2.0` release workflow failed before Thunderstore publication
      because the repository `THUNDERSTORE_TOKEN` secret was unavailable to
      the upload step.

### Notes

- This is a publication recovery release.
    - It is expected to match `v0.2.0` in gameplay behavior.
    - It is expected to keep the same compatibility baseline and package
      metadata intent as `v0.2.0`.
    - The only intended difference from `v0.2.0` is the published release
      version.

## v0.2.0 - 2026-05-08 UTC

### Changed

- Rebuilt the current stable release line for Lethal Company v81.5 after
  release-candidate validation.
- Ported internal repository, build, release, and documentation maintenance
  practices from CruiserJumpPractice's `v0.2.0` preparation work.
- Prepared stable-release automation for GitHub Releases and Thunderstore.
- Added opt-in structured validation logging guidance for release-candidate
  checks. This is disabled by default and is intended to keep future validation
  evidence concise.

### Fixed

- Fixed prerelease validation artifacts being rejected by BepInEx 5 before
  plugin startup. The stable `v0.2.0` release uses valid loader-facing metadata
  from `<Version>0.2.0</Version>`, while non-stable CI artifacts keep their
  prerelease identity in GitHub release tags and artifact names.

### Notes

- Compatibility:
    - Compatible with Lethal Company v81.5 (2026-04-17 UTC, Manifest ID:
      `6423525044216269478`).
        - The v81.5 validation environment used BepInExPack v5.4.2305.
- Validation:
    - `v0.2.0-alpha.2` real-game validation passed for host-side direct
      delivery, normal dropship fallback cases, ejection reset behavior, mixed
      queued purchases, and configuration spot checks.
    - The maintainer accepted missing client-initiated purchase coverage as a
      residual validation risk for this stable release. Client-side logs
      confirmed clients loaded the mod and did not run server-side
      instant-delivery logic.
- Older Lethal Company versions are no longer claimed as tested by the current
  v0.2.0 release notes; Lethal Company v73 compatibility is recorded in the
  historical `v0.1.x` release entries below.

## v0.2.0-alpha.2 - 2026-05-08 UTC

### Fixed

- Fixed prerelease artifacts being rejected by BepInEx 5 before plugin startup:
    - `v0.2.0-alpha.1` validation was blocked because BepInEx skipped the
      plugin type when the loader-facing version contained the SemVer
      prerelease suffix.
    - The release workflow now passes non-stable BepInEx plugin metadata as
      `0.0.0` to the CI build, which keeps the metadata compatible with
      BepInEx 5's `System.Version` validation without making that fallback
      part of the source project file.
    - The project version now prepares the next validation artifact as
      `v0.2.0-alpha.2`; GitHub release tags and artifact names continue to
      carry the prerelease identity.

## v0.2.0-alpha.1 - 2026-05-06 UTC

### Changed

- Added opt-in structured validation logging guidance for v0.2.0
  release-candidate checks.
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
- This is the first `v0.2.0` alpha artifact for release-candidate validation.
  Real-game alpha validation still needs to be performed against the packaged
  artifact before stable release preparation starts.
- Prerelease artifacts are GitHub-only; Thunderstore publication remains
  limited to stable releases.

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
