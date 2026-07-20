# Canonical Repository Templates

Use the bundled templates only when the target repository adopts the matching
release contract. They are exact reusable files, not examples to rewrite per
repository.

## Bundled templates

The mapping in `assets/template-map.json` installs these template groups:

- `repository-contributing` and `github-pull-request-template`: keep the
  contributor workflow, AI-assistance disclosure, and Contribution License
  Agreement text and confirmation synchronized as one contract. Do not deploy
  the CLA checkbox without the bundled agreement, or the agreement without its
  required confirmation.
- `github-generate-version`: derive stable, prerelease, and edge versions and
  synchronize project and Thunderstore manifest versions.
- `github-publish-thunderstore-action` and
  `github-publish-thunderstore-script`: submit one prebuilt package to
  Thunderstore without rebuilding it.

The version template requires the marker and file contract it documents. The
Thunderstore templates apply only when the evidence ledger selects
Thunderstore and the release workflow supplies the required reviewed inputs.
Keep `thunderstore_namespace`, `thunderstore_community`, and
`thunderstore_categories` as target-specific render variables; never replace
them with values copied from another repository.

`assets/github/workflows/` contains rendered CI skeletons rather than
exact-sync template IDs: project and package-host values are intentionally
render variables. Render the paired `pull-request.yml.template`,
`main.yml.template`, and the local `install-workflow-tools`, `setup-dotnet`,
and `lint-source` Composite Actions together. The small actions expose their
individual toolchain, environment, and lint responsibilities; `lint-source`
validates source on the caller's runner;
`Main` repeats that validation on the pushed integration commit, resolves
read-only version and release state in `plan`, then gates build and publication
through direct `needs`. Build receives the planned identity before it writes
package source files and retains every artifact, including non-published edge
output. Do not fold the event boundaries back into one trigger-heavy workflow
or add manual dispatch without a named operator procedure.

## Initial provenance

The canonical assets were seeded from reviewed first-party consumer files.
Portable files matched across consumers; the contributor guide was generalized
only by replacing repository-specific names and URLs with repository-relative
wording. Their initial source Git blob identities are recorded without making
those consumers part of the quality contract:

| Template | Initial Git blob |
| --- | --- |
| `repository-contributing` | `2b8b39f943946719558a0877088c1268f9a87d75` |
| `github-generate-version` | `0535faef6bfa4195e015c3a8c1e6c575d3e2c9ec` |
| `github-publish-thunderstore-action` | `18dbef7efb310fe4cf03d7318cbd779c45ea5638` |
| `github-publish-thunderstore-script` | `0e3bcb2ae5ff850e1a25db6591565ec0f67af0ee` |

After this import, `aoirint/skills` is the source of truth; consumer history is
evidence, not an upstream to copy automatically.

## Adopt or verify

Run the script from the installed Skill. Select only templates enabled by the
repository contract; omitting `-Template` selects all bundled templates.

```powershell
& .agents/skills/bepinex-mono-mod-quality-check/scripts/sync_templates.ps1 `
  -RepoRoot . -Apply `
  -Template repository-contributing,github-pull-request-template
```

Use the same selection in contributor documentation and CI, replacing
`-Apply` with `-Check`. The check fails when a selected destination is missing
or modified. YAML must match exactly after Git-policy line-ending
normalization; executable shell files must match byte-for-byte, including LF.
Select `repository-contributing` and `github-pull-request-template` together;
they are not independently adoptable because the checkbox confirms the
agreement in the companion document.

## Improve without drift

1. Name `aoirint/skills` and the files under this Skill's `assets/` directory
   as the canonical source. Treat consumer copies as generated from it.
2. Inspect all known consumers before changing the template. Separate a
   portable improvement from target-specific paths, names, credentials,
   categories, or policy.
3. Edit the canonical asset first. Do not promote an unvalidated consumer copy
   merely because it is newer.
4. Apply the candidate to a scratch consumer and run its parsing, ShellCheck,
   actionlint, pinact, build, package, archive, and dry-run checks as applicable.
5. Apply the validated asset to every opted-in consumer with the sync script.
   Keep a target-specific variation outside the template and document why that
   consumer cannot use exact synchronization.
6. Run `-Check` with the same template selection in every consumer, followed
   by its repository checks. Every selected file must satisfy the comparison
   mode declared in `template-map.json`; do not weaken that mode downstream.
7. Commit the canonical change before or with linked rollout PRs. Record the
   canonical commit, consumer list, intentional exclusions, and validation
   results so later maintenance can find the complete rollout set.

Never edit `.agents/skills/` in this repository directly. Change the canonical
`.apm/skills/` asset, then regenerate deployed Skill files with APM.
