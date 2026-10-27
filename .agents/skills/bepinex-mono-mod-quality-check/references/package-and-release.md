# BepInEx Package and Release Checks

## Package and release consistency

1. Check package and release consistency.
   - Trace the release artifact from the built DLL to its archive. Confirm that the archive layout and every required
     file match the package host and repository documentation.
   - Keep a versioned, repository-owned archive contract. Its host-neutral part must define the allowed root paths,
     exact plugin-DLL count, prohibited contents, and archive path-safety rules; add a host-specific extension only from
     the selected host's authoritative contract. Do not substitute another host's manifest or layout.
   - Reconcile package manifest identity, version, dependencies, compatibility claims, README, changelog, icon, and
     any explicitly selected license with the release intent. Do not create, infer, or package a license until the
     maintainer selects one.
   - When compatibility is repeated across the package README, developer
     changelog, and package changelog, inspect same-role entries in designated
     peer repositories before writing. Preserve the established hierarchy,
     dates, and reference links, while retaining each artifact's role: README
     compatibility sections may show the target and test environment; changelog
     `Notes` entries should state concise compatibility facts. Keep detailed
     evidence status in the evidence ledger, developer documentation, or PR
     notes unless it changes the release's user-facing compatibility meaning.
     Do not infer that an incomplete runtime result belongs in a package
     changelog or README merely because it is recorded in developer evidence;
     add it only when the same-role convention or maintainer selects it.
     Do not add a `Changed` entry that merely says the documentation was
     "clarified"; record the resulting compatibility state under `Notes`.
   - Separate package readiness from publication authorization. When the evidence ledger confirms a distribution host,
     keep the repository family's portable manifest, package README, user-facing changelog, editable and rendered icon,
     final-archive validation, inert publisher tooling, and a license only when explicitly selected complete even when
     an external upload is not yet enabled. If the host is blocked or explicitly none, do not invent or borrow
     host-specific metadata. Gate the publication side effect on explicit host, namespace, category, credential,
     runtime, and release-mode approval.
   - Verify version synchronization from produced outputs, not duplicated source literals. Generate loader-facing
     metadata from the project version or inspect the built assembly and compare project, assembly, loader-facing,
     manifest, archive, and tag versions that are enabled for the release.
   - Decode BepInEx loader metadata from the built assembly's actual custom attributes. String presence in DLL bytes
     does not prove `BepInPlugin`, `BepInProcess`, or dependency attributes carry the expected constructor values.
   - Verify identity, supported game/loader baseline, dependencies, and release version on the exact metadata and
     documentation files copied into the final archive. Repository-root prose does not satisfy a package-facing claim
     when it is not distributed.
   - When a package-facing README embeds screenshots or diagrams, verify that every reference resolves and renders in
     the package host or its documented renderer. Keep renderer-specific fallback files separate from editable
     developer sources, and do not treat root-README rendering as package validation. Package an image only when the
     host contract requires it; otherwise a stable repository-hosted fallback may avoid duplicating an asset whose
     authoring lifecycle belongs in developer documentation.
   - Keep developer changelog entries and user-facing release notes in their repository-defined roles. Either package a
     distinct publication-facing source or explicitly declare the canonical changelog dual-purpose; do not describe a
     derivation step that packaging does not perform. Do not claim untested game compatibility or publish a version
     already represented by an immutable release.
   - Reserve `0.0.0` for unpublished edge or development builds; do not use it
     as a released changelog version. Derive every other release version from
     the project source instead of hardcoding a specific public version in
     workflows. Keep pending work under `Unreleased` until a real release
     version and date are selected.
   - A negative package fixture proves only the first production guard it reaches. Derive it from the passing fixture,
     preserve every earlier invariant, mutate the intended property, and assert the intended branch-specific diagnostic
     or result. A generic exception or nonzero exit does not prove the advertised rejection branch.
   - When the project derives manifest or package versions in CI, verify that the project version, generated version,
     and loader-compatible version are deliberately handled for stable, prerelease, and edge builds.

## BepInEx release automation

1. Check BepInEx release automation when the repository uses GitHub Actions or GitHub Releases. Apply
   `github-actions-quality-check` for the shared workflow baseline and `github-workflow` for repository settings and
   enforcement.
   - Give the read-only release plan a full checkout with tag history (`fetch-depth: 0`) before resolving a version.
     During rollout, verify this in the committed consumer workflow itself; the canonical template being correct does
     not prove that an existing or previously copied workflow retained the setting. Run the main-push path after merge
     so a pull-request-only source check cannot hide a release-plan history failure.
   - Keep checksum material used to verify the build-to-release handoff inside the workflow artifact by default.
     Publish only the package archive unless the repository has an explicit user-facing checksum-asset contract.
   - Install the exact SDK selected by `global.json` in CI with a full-SHA-pinned setup action or another pinned,
     verified mechanism before restore. Do not assume a hosted runner already contains the release-critical SDK.
   - Separate validation artifacts, prereleases, and stable publishing according to the repository's version rules. Gate
     external package-host publication on the intended stable release mode and require exactly one reviewed package
     artifact. Keep a separate committed publication-authorization input disabled until the maintainer explicitly
     approves the side effect; a stable version alone is not authorization.
     Do not introduce a separate release mode merely because a numeric public
     release remains beta: when the repository classifies it as stable release
     mode, express beta status in its release metadata and notes.
   - Retain every integration-branch edge build as a workflow artifact, with its source commit and digest visible in the
     workflow summary, even when the edge version is deliberately not published.
   - Keep archive creation CI-owned. A locally callable validator is useful, but a second repository-local production
     packager, `release/` helper tree, or custom approval schema needs a distinct consumer and lifecycle. Never remove the
     stable release path while consolidating packaging ownership.
