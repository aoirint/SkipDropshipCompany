# Development Operations

## Dependency updates

Use the SDK selected by `global.json`. Identify the project that declares
the package, its `packages.lock.json`, and the applicable `nuget.config`.
Use the checked-in package sources and source mappings.

1. Review the package identity, publisher, source, license, advisories, and
   publication date. Verify the configured seven-day minimum before accepting
   a version. Explicit version requests and ordinary restore do not enforce
   the minimum publish age.
2. Update the intended package reference to an exact version. Refresh the
   lockfile for the affected project with `--force-evaluate --use-lock-file`
   and the correct `--configfile`. Do not refresh unrelated projects.
3. Review the project and lockfile diffs. Check resolved versions, transitive
   changes, and content hashes. Investigate unexpected changes before
   keeping the update.
4. Run a locked restore, then the repository's format, build, and applicable
   tests. Keep the project and lockfile changes together in the PR and verify
   its CI before merging.

### SkipDropshipCompany commands

Run from the repository root in PowerShell. For a plugin dependency update:

```powershell
dotnet restore SkipDropshipCompany/SkipDropshipCompany.csproj `
  --configfile SkipDropshipCompany/nuget.config --force-evaluate --use-lock-file
dotnet restore SkipDropshipCompany/SkipDropshipCompany.csproj `
  --configfile SkipDropshipCompany/nuget.config --locked-mode
```

See [Quality checks](../../README.md#quality-checks) for
format and build verification. This repository has no test project.
