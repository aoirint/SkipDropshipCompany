# VERSION-Driven Releases

## Purpose

Use this option when a repository intentionally makes a root `VERSION` file on the
protected integration branch the canonical release request and identity. Keep it distinct
from tag-push, GitHub Release event, manual-dispatch, and external release-manager paths.

## Entry and migration contract

- Trigger the integration workflow only from a push to the protected integration branch.
- Read `VERSION` from the checked-out integrated commit. Do not accept release identity from
  event payloads, workflow inputs, branch names, or mutable repository variables.
- Remove prior release-created, tag-push, or manual publication triggers when the maintainer
  requests a replacement without compatibility. Search workflows and documentation for stale
  entry paths; do not leave a second release authority.
- Keep proposed-source validation in a separate `pull_request` and `merge_group` workflow.
  Never expose release or registry credentials to proposed source.
- Do not cancel integration runs. A superseded run may own a distinct release request or an
  immutable publication already in progress.

## Read-only release plan

1. Parse one canonical version from `VERSION`. By default, accept one bare stable or
   prerelease value matching
   `^[0-9]+\.[0-9]+\.[0-9]+(-[0-9A-Za-z.-]+)?$`, with only one optional trailing newline.
   Reject a `v` prefix, leading or embedded whitespace, extra lines, and build metadata.
   Use a different SemVer subset only when repository evidence requires it; document and test
   that canonicalization contract.
2. Derive the Git identity, normally `v<VERSION>`, and stable or prerelease mode solely from
   that parsed value.
3. Read the remote tag and release state without mutating it. Apply this truth table:
   - Tag absent and release absent: select a new release.
   - Tag present and matching release present: select ordinary edge/integration mode; the
     immutable release is already complete.
   - Only one exists, the release names another tag, or the tag target violates the repository's
     immutable-release contract: fail safely for maintainer recovery.
4. Never retarget, replace, or silently repair an existing immutable identity. Existing
   identities do not enable version or latest aliases in later builds; they enable only the
   repository's ordinary edge/integration aliases.
5. Prefer state-based retryability over inspecting only the current diff: if an earlier run
   failed before creating the identity, a later integration run may safely retry the same
   unrepresented `VERSION` after repeating validation, build, publication, and tests.

Keep this plan read-only and expose only the validated version, tag, release mode, and other
inputs required by direct downstream jobs.

## Build, publish, test, release

Use this dependency order:

```text
check + plan -> build/publish -> test published artifact -> create release
```

- Build once from the exact integrated commit. Publish or hand off that same artifact; do not
  invoke another build in test or release jobs.
- Record the source SHA and content digest. Test the final published artifact by digest when
  the registry supports it. When multiple registries are contractual destinations, verify
  each destination and confirm that corresponding tags resolve to the intended artifact.
- Define repository-owned tag behavior for edge, stable, prerelease, latest, and variant
  aliases. Generate tags only after the release plan so an existing identity cannot overwrite
  immutable version aliases.
- Create the Git tag and GitHub Release only after every required artifact test succeeds.
  Target the exact integrated SHA that produced the tested artifact.
- Create releases immutably and derive prerelease/latest flags from the validated version.
  Re-read remote identity immediately before mutation when concurrent writers are possible.
- Grant registry credentials and `packages: write` only to publication jobs. Grant
  `contents: write` only to release creation. Keep check, plan, build-only, and test jobs at
  `contents: read`; use `persist-credentials: false` for read-only checkout.

## Verification

- Run source validation for missing, malformed, noncanonical, stable, and prerelease values.
- Observe proposed-source and merge-queue checks before requiring their exact job contexts.
- Observe an unchanged or existing `VERSION` integration run and confirm edge-only behavior.
- Observe a new release identity and verify build/test completion precedes release creation.
- Read back the tag target, release state, prerelease/latest flags, image tags, source labels,
  and registry digests. Confirm no removed legacy trigger can still publish.
- Record settings or event cases that cannot be exercised as unverified; actionlint alone is
  not runtime proof.
