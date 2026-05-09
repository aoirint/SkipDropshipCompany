This changelog is the user-facing release notes for Thunderstore.

For internal implementation details and developer-facing release history, see
the [GitHub changelog][github-changelog].

If you find a release-note error, encounter a bug, or want to report another
project issue, see [CONTRIBUTING.md][contributing], then report it in
[GitHub Issues][github-issues].

## v0.2.5 - 2026-05-09 UTC

This release updates the Thunderstore documentation.

No gameplay changes are introduced.

### Fixed

- Clarified the comparison with `Nexor/InstantBuy`.
  The configurable `Company Only` scope is now documented separately from
  modded Company moon coverage.
  SkipDropshipCompany is documented as working on the Company (vanilla-only)
  plus the limited orbit cases described in the README.

## v0.2.4 - 2026-05-09 UTC

This release updates the Thunderstore documentation.

No gameplay changes are introduced.

### Fixed

- Corrected the comparison with `Nexor/InstantBuy`.
  `Nexor/InstantBuy` works only on Company moons by default since v0.0.8
  because its `Company Only` option defaults to enabled.
  Users can still configure `Nexor/InstantBuy` to run on other moons by
  disabling that option.

## v0.2.3 - 2026-05-09 UTC

This release updates the Thunderstore documentation.

No gameplay changes are introduced.

### Fixed

- Restored the `Nexor/InstantBuy` comparison note about excluding specified
  items from instant purchases.
  The note was unintentionally removed while updating the Company moons
  comparison for `v0.2.2`.
  SkipDropshipCompany does not have an equivalent item-exclusion configuration
  option.

## v0.2.2 - 2026-05-09 UTC

This release updates the Thunderstore documentation.

No gameplay changes are introduced.

### Changed

- Clarified how this mod differs from `Nexor/InstantBuy`:
    - `Nexor/InstantBuy` can be configured to work only on Company moons since
      its Thunderstore v0.0.8 release.
        - Correction added in v0.2.4: this wording was incomplete.
          `Nexor/InstantBuy` works only on Company moons by default since
          v0.0.8 because its `Company Only` option defaults to enabled.
          Users can configure it to run on other moons by disabling that
          option.
    - SkipDropshipCompany has no configuration except for the first-day option
      to change its direct-delivery behavior.
- Added `HQHQTeam/HQoL` to the compatibility notes and comparison section.
  `HQHQTeam/HQoL` includes a faster company dropship feature, while
  SkipDropshipCompany skips the dropship delivery and places purchased items on
  the ship directly.

### Fixed

- Corrected the `dozyote/EarlyDropship` comparison.

### Notes

- Compatibility:
    - Compatible with Lethal Company v81.5 (2026-04-17 UTC, Manifest ID:
      `6423525044216269478`).

## v0.2.1 - 2026-05-08 UTC

This release rebuilds SkipDropshipCompany for Lethal Company v81.5 and includes
internal improvements.

No gameplay changes are introduced.

### Changed

- Rebuilt for Lethal Company v81.5.
- Improved internal implementation structure and release flow.
- Added the Thunderstore `AI Generated` category to the package metadata:
    - The Lethal Company Thunderstore community currently provides this
      category for authors to disclose when a `significant portion` of a mod
      was created using AI tools.
    - This project uses the category to disclose AI assistance in project work;
      it is package metadata rather than a gameplay feature.
    - The project decided to use this category because:
        - It has used and expects to keep using AI tools to assist with
          development and maintenance, and to reduce workload.
        - The applicable disclosure threshold is not clear.
    - Human maintainer review remains the project policy.

### Notes

- Compatibility:
    - Compatible with Lethal Company v81.5 (2026-04-17 UTC, Manifest ID:
      `6423525044216269478`).

## v0.1.7 - 2025-11-23 UTC

### Fixed

- Fixed some typos in README.

### Notes

- Compatibility:
    - Compatible with Lethal Company v73 (2025-10-04 UTC, Manifest ID:
      `1749099131234587692`).
        - Backfilled as reference compatibility information while preparing
          the v0.2.1 release.

## v0.1.6 - 2025-11-23 UTC

### Added

- Added the `Enabled` config option to enable or disable this mod without
  uninstalling it.

### Notes

- Compatibility:
    - Compatible with Lethal Company v73 (2025-10-04 UTC, Manifest ID:
      `1749099131234587692`).
        - Backfilled as reference compatibility information while preparing
          the v0.2.1 release.

## v0.1.4 - 2025-11-23 UTC

### Added

- Initial release on Thunderstore.

### Notes

- Compatibility:
    - Compatible with Lethal Company v73 (2025-10-04 UTC, Manifest ID:
      `1749099131234587692`).
        - Backfilled as reference compatibility information while preparing
          the v0.2.1 release.

[contributing]: https://github.com/aoirint/SkipDropshipCompany/blob/main/CONTRIBUTING.md
[github-changelog]: https://github.com/aoirint/SkipDropshipCompany/blob/main/CHANGELOG.md
[github-issues]: https://github.com/aoirint/SkipDropshipCompany/issues
