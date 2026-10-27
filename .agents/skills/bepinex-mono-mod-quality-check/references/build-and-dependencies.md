# BepInEx Build and Dependency Checks

## Build and dependency boundaries

1. Check build and dependency boundaries.
   - Match the target framework and language version to the repository's documented BepInEx and game runtime
     compatibility; do not upgrade either opportunistically.
   - Keep game and loader references compile-only when the release package relies on the player's installed runtime.
     Keep development analyzers and code generators private to the project.
   - When updating a game-specific compile reference, update its resolved package and lockfile to the target game
     version, then reconcile every version-bearing project, package, changelog, and compatibility document field. A
     successful compilation proves only API resolution; it does not establish runtime compatibility.
   - Require a committed lockfile for every resolving project and run `dotnet restore --locked-mode`. After restore, run
     `dotnet format --no-restore --verify-no-changes` and a no-restore build. A missing lockfile or unlocked restore is
     a reproducibility finding.
   - When a change adds or modifies automated tests, run the repository's documented test command after the locked
     restore and no-restore build. A separate Core or test project is justified only by its stated test, reuse, build,
     or dependency-isolation benefit, and must meet the same lockfile and source-mapping rules as every other resolving
     project.
   - Require `nuget.config` to clear unreviewed default sources and map direct and transitive package patterns to their
     intended source. Review every new feed, package, lockfile change, CI action, container, or downloaded tool for
     canonical source, publisher, immutable version or digest, content hash where available, license, transitive
     effects, and seven-day release age before adoption.
   - Do not commit game installations, mod-manager profiles, generated plugin metadata, `bin/`, `obj/`, or machine-local
     paths.
