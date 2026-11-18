# Dependency Baseline

## NuGet minimum publish age

NuGet 8 applies a source's minimum publish age when `dotnet package add` or
`dotnet package update` automatically selects a version.
An explicit version request bypasses the filter.
Configured package exceptions still apply.

The filter uses the registration `published` timestamp.
Both nuget.org and the BepInEx feed expose this metadata.
If an evaluated version lacks a publication date, automatic selection fails
rather than treating the version as old enough.

Ordinary restore, including locked restore, does not verify the age of
non-floating references or transitive dependencies.
Restore reports NU1020 for a floating reference when an applicable source
enforces a nonzero minimum and the package is not exempt.
It does not select an older version to satisfy the threshold.

Meeting the minimum age does not establish trust.
Review package identity, provenance, hashes, and vulnerability signals
separately.
See [Microsoft's NuGet documentation][nuget-minimum-publish-age] for
supported operations and limitations.

[nuget-minimum-publish-age]: https://learn.microsoft.com/en-us/nuget/consume-packages/minimum-package-publish-age
