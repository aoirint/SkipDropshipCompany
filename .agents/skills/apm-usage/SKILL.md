---
name: apm-usage
description: Set up, pin, deploy, audit, and update APM-managed agent dependencies safely. Use when creating or editing apm.yml or apm.lock.yaml, installing APM, adding an Agent Skill, plugin, or MCP dependency, validating a pinned deployment, or preparing a cooldown-aware update proposal.
---

# APM Usage

Keep agent context reproducible and reviewable. Reuse an existing APM
installation; do not update it incidentally. Require a seven-day cooldown,
full commit pins, lockfile hashes, and review for every changed third-party
dependency.

## 1. Inspect before changing

1. Locate `apm`, `apm.yml`, `apm.lock.yaml`, `apm-policy.yml`, existing agent
   directories, and repository guidance. Run `apm --version` if the binary is
   already available.
2. If `apm.yml` exists, preserve it. Do not run `apm init --yes`, because it
   overwrites an existing manifest.
3. If no manifest exists, inspect the repository's agent targets and run
   interactive `apm init` from the project root. Select only the targets the
   project actually uses; review the generated `apm.yml` before adding deps.
4. Treat agent dependencies as supply-chain-sensitive code. Check the
   repository policy and any organization policy before proceeding.

## 2. Install APM safely when it is absent

Prefer the installed project or user copy when it satisfies the task. Do not
run a floating one-line installer or `apm self-update` merely to obtain a newer
version.

Read [the bootstrap manifest](references/apm-bootstrap.json) and follow the
matching section of [the OS installation template](references/apm-install.md).
The manifest is the single source of truth for the pinned version, release
date, assets, and SHA-256 values.

When a new APM installation is required:

1. Confirm that no usable `apm` is already on `PATH`, then follow only the
   matching OS section in the reference. Verify its hard-coded SHA-256 before
   extraction or execution. Never set `APM_SKIP_CHECKSUM=1`.
2. Install to a user-writable, dedicated APM directory unless the project
   explicitly requires an administrator-managed location. Verify with
   `apm --version` and record the exact version.
3. For an enterprise mirror, use HTTPS, set `APM_NO_DIRECT_FALLBACK=1`, and
   verify that the mirror serves the identical reviewed artifact and checksum.
   Do not treat this as a substitute for review of the artifact.

## 2.1 Propose a bootstrap-version refresh

Do not change the bootstrap manifest automatically. Use this proposal flow:

1. Record the current and candidate tags, release URLs, publication dates,
   platform assets, SHA-256 values, and the candidate eligibility date.
2. Require the candidate's publication date plus seven full days and explicit
   maintainer approval.
3. Run `uv run --locked --script scripts/propose_bootstrap_update.py` to collect an eligible
   candidate without changing files. Attach its JSON output to the proposal.
4. After approval, run the same command with `--write --confirm-version <candidate>`.
   It updates only `references/apm-bootstrap.json` and refuses an unconfirmed
   or mismatched version.
5. Review the manifest diff and the matching OS template before committing.

Do not use `apm self-update` to perform this refresh.

## Reference sources

| Source | Use |
| --- | --- |
| [Official APM repository](https://github.com/microsoft/apm) | Verify supported configuration, CLI behavior, and upstream guidance. |
| [APM documentation](https://microsoft.github.io/apm/) | Consult the maintained usage and reference documentation. |
| [APM releases](https://github.com/microsoft/apm/releases) | Review release tags, publication dates, assets, and release notes. |
| [GitHub Releases API](https://api.github.com/repos/microsoft/apm/releases?per_page=30) | Obtain machine-readable release metadata; this is the endpoint used by the proposal script. |
| [APM security policy](https://github.com/microsoft/apm/security/policy) | Follow the upstream vulnerability-reporting process. |
| [Pinned bootstrap manifest](references/apm-bootstrap.json) | Obtain this skill's reviewed version, eligible date, asset names, and SHA-256 values. |

## 3. Add an Agent Skill or dependency

Use this staged workflow for every remote skill, prompt, agent, plugin, hook,
or MCP dependency:

1. Identify the exact source repository, optional virtual subdirectory,
   release/tag, and full 40-character commit SHA. Review the source, its
   release history, declared capabilities, transitive dependencies, and changes
   that could execute code or add MCP access.
2. Verify from an authoritative source that the adopted content is at least
   seven days old. For a virtual subdirectory in a monorepo, identify the last
   commit that changed that subdirectory at the selected ref and calculate the
   cooldown from that commit, not from an unrelated later commit elsewhere in
   the repository. If its age, provenance, or required hash cannot be verified,
   stop and report the blocker. Do not bypass the cooldown without a documented
   maintainer exception.
3. Before committing a third-party skill, determine its license from an
   authoritative upstream license file or policy. Add or update the
   repository-root `THIRD_PARTY_NOTICES.md` with its source repository and
   virtual path, full commit SHA, license identifier and canonical text or URL,
   plus any supplied copyright or NOTICE text. Do not put this record in an
   APM-managed target directory such as `.agents/skills/`. Treat a missing or
   unclear license or required notice as a blocker unless a maintainer records
   an explicit exception.
4. Add the dependency to `apm.yml` with the full commit SHA, never a default
   branch, `latest`, or an unbounded range. Keep MCP entries under the same
   review gate and do not put tokens in tracked YAML.
5. Run `apm lock` to resolve and download without deploying to agent targets.
   Review `apm.lock.yaml`: each dependency must resolve to the expected commit
   and carry its content hash. Commit the manifest and lockfile together.
6. Run `apm install --frozen` only after that review. It must reproduce the
   reviewed lockfile and must not resolve a new dependency. Never use
   `--force`, `--allow-insecure`, or an insecure HTTP source unless an explicit
   documented exception authorizes it.
7. Run `apm audit --ci` after deployment. Fix lockfile, integrity, drift, or
   policy findings rather than suppressing them. Add `apm install --frozen` and
   `apm audit --ci` to the project's existing CI when the user requests CI
   enforcement.

## 4. Propose updates; do not silently apply them

Pinned dependencies stay unchanged until a maintainer approves an update.
Create an update proposal, issue, or pull request with no deployment changes
first:

1. Run `apm outdated` and `apm update --dry-run` to discover candidates
   without writing files.
2. For each candidate, record the current and proposed tag and full commit
   SHA, upstream release URL and date, SHA-256/content-hash changes, source
   review summary, new transitive dependencies, and any MCP, hook, permission,
   or target-output changes.
3. State the earliest eligible adoption date: release date plus seven full
   days. Keep the proposal open until that date and explicit maintainer
   approval; a newer upstream release restarts its own cooldown.
4. After approval, run interactive `apm update` for only the approved package.
   Review the plan before accepting it, then review the `apm.yml`,
   `apm.lock.yaml`, and deployed-file diff. Run `apm audit --ci` and commit the
   result. Re-check the upstream license and notices; update the matching
   `THIRD_PARTY_NOTICES.md` entry in the same commit when the source, virtual
   path, commit, license, or attribution changes.
5. In CI and ordinary repeat installs, keep using `apm install --frozen`.
   Update discovery and update application must remain separate actions.

## 5. Reconcile third-party notices

Before handoff, reconcile `THIRD_PARTY_NOTICES.md` with the reviewed lockfile:

1. For every third-party skill whose files are committed from an APM target,
   keep one current notice entry with the source, selected virtual path, locked
   SHA, license, and required attribution.
2. When removing a dependency, run `apm prune` and confirm its deployed files
   are gone before removing its notice entry. Retain the notice when a copy or
   derivative remains in the repository.
3. Treat a changed license, copyright, or NOTICE file as an adoption-review
   change, not a metadata-only update. Resolve it before deploying the new
   version.

## Completion checklist

- `apm.yml` is present and was not overwritten accidentally.
- Every remote dependency is pinned to a reviewed full commit SHA.
- `apm.lock.yaml` is committed and contains the expected resolved commits and
  content hashes.
- Each adopted release, or the last change to a selected virtual subdirectory,
  met the seven-day cooldown or has a documented maintainer-approved exception.
- Each committed APM-deployed third-party skill has one current
  `THIRD_PARTY_NOTICES.md` entry, including its license and required notices;
  removed skills have no stale entry unless a copy remains.
- Deployment used `apm install --frozen`, and `apm audit --ci` passed.
- Updates have a reviewable, cooldown-aware proposal before application.
