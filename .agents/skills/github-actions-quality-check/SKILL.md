---
name: github-actions-quality-check
description: >-
  Review, design, create, or repair GitHub Actions workflows and local actions
  for event boundaries, dependency structure, permissions, concurrency,
  immutable pins, runner selection, validation, artifacts, releases, and
  repository enforcement. Use for .github/workflows, .github/actions, Actions
  policy, required-check design, or CI template changes; use github-workflow
  for issue, pull-request, comment, and squash-merge text or operations.
---

# GitHub Actions Quality Check

## When to Use

Use this Skill for GitHub Actions workflow files, local Composite Actions,
reusable workflows, Actions repository settings, required-check contexts, and
CI templates. Pair it with the ecosystem Skill that owns the commands being
automated and with `security-check` for third-party executable inputs,
permissions, secrets, publishing credentials, and artifacts.

Use `github-workflow` for issue and pull-request artifacts, comments, reviews,
and squash merges. Do not use this Skill to infer application test commands,
release identity, package layout, or deployment policy that the repository and
its ecosystem Skill do not establish.

## Goals

- Make event, trust, privilege, and lifecycle boundaries explicit.
- Re-run required validation on the exact proposed and integrated commits.
- Keep job dependencies direct, artifacts traceable, and publication
  idempotent.
- Keep Actions and downloaded tools immutable, provenance-reviewed, and
  least-privilege.
- Produce evidence-calibrated findings and verification records.

## Workflow

1. Inventory before editing.
   - Read repository guidance, every entry workflow, every reachable local
     action or reusable workflow, release scripts, required-check settings, and
     the ecosystem's local validation contract.
   - Record each event, job name, runner, `needs` edge, permission, concurrency
     rule, external `uses:`, download, secret, cache, artifact, and publication
     side effect.
   - Distinguish observed facts, requested changes, unavailable evidence, and
     proposed policy. Never infer compliance from a related setting or a prior
     run.
2. Design event-owned entry workflows.
   - Validate untrusted proposed source on `pull_request`; include
     `merge_group` when merge-queue required checks use that job.
   - Re-run required validation on the protected integration branch's exact
     pushed commit. Keep this run uncancelled.
   - Keep pull-request and integration-push entry workflows separate because
     their cancellation, source trust, and lifecycle differ.
   - Add `workflow_dispatch` only for a documented operator or recovery need.
     Use `workflow_run` only for a separately reviewed trust boundary. Never
     emulate a direct dependency with polling or an `await-quality` job.
3. Build a direct job graph.
   - Read [naming-and-readability.md](references/naming-and-readability.md)
     before naming or renaming workflow files, workflows, jobs, steps, or local
     actions. Read [composite-actions.md](references/composite-actions.md)
     before creating or substantially changing a local Composite Action. Use
     `check`, `test`, `build`, and `deploy` as a small
     conceptual vocabulary when it fits, but split jobs and workflows only
     when their operational boundaries justify the cost.
   - Make build depend directly on every required validation and plan job.
     Make release consume the verified build artifact and required plan output.
   - Keep planning read-only: it may resolve canonical release state, but must
     not mutate tracked files, tags, releases, or other GitHub state.
   - Reuse a local Composite Action for a stable same-runner sequence. Use a
     reusable workflow only when job-level matrices, outputs, runners, or
     permission boundaries require one, and document that reason.
   - Keep one blank line between sibling jobs and between sibling steps. Add a
     concise adjacent comment only when it preserves non-obvious design intent,
     such as a trust boundary, cancellation exception, or artifact handoff.
4. Minimize authority and cancellation.
   - Start workflow permissions at `contents: read`. Grant writes only on the
     job that demonstrably needs them; document every unusual permission.
   - In read-only checkout steps, set `persist-credentials: false` unless a
     later step demonstrably needs repository credentials.
   - Cancel only superseded pull-request and merge-queue runs, keyed by pull
     request number or ref. Do not cancel default-branch validation or an
     in-progress immutable publication.
   - Do not execute untrusted proposed source with `pull_request_target` or a
     write-capable token.
5. Review runners and executable inputs.
   - Read [runner-selection.md](references/runner-selection.md) before selecting
     or changing a GitHub-hosted runner.
   - Give every `uses:` step a responsibility-revealing `name`. Pin every
     external action and reusable workflow to a full 40-character commit SHA
     with an accurate same-line release comment.
   - For each changed external action, downloaded tool, or container, use
     `security-check` to verify publisher and source, immutable identity,
     release age, checksum or digest where applicable, runtime behavior, and
     requested permissions. A seven-day cooldown is a minimum gate, not proof
     of trust.
   - Before restricting an Actions allowlist, recursively inventory every
     reachable `uses:`. Allow only required names; with SHA enforcement, an
     individual `owner/action@*` allowlist entry permits future reviewed pins
     without wildcarding an owner.
6. Preserve exact-source artifact and release gates.
   - Build once from the validated commit, validate the final packaged artifact,
     record its digest and source identity, and pass that artifact forward.
   - Do not rebuild in the release job. Verify the downloaded artifact before
     publishing it.
   - Derive release identity from one canonical source in a read-only plan.
     Make retries idempotent and verify an existing immutable release and its
     assets instead of silently replacing them.
   - Isolate write permissions and credentials in the publication job or
     protected environment. Use the artifact-specific ecosystem Skill and
     `security-check` for final-container inspection.
   - When a repository uses a root `VERSION` file on its integration branch as
     the release request and identity, read
     [version-file-releases.md](references/version-file-releases.md) and apply
     its event, planning, idempotency, publication, and migration contract.
7. Validate source and embedded shell.
   - Treat every embedded `run:` block as maintained source code. Apply
     `code-quality-check` and the applicable language Skill, including
     `python-quality-check` for embedded Python. Keep short orchestration
     inline; move parsing, branching, reusable functions, or behavior that
     needs focused tests into a repository-owned script. Follow the placement,
     invocation, and validation contract in
     [composite-actions.md](references/composite-actions.md).
   - Run the repository-documented `actionlint`, applicable standalone
     ShellCheck, and `pinact run --check --min-age 7`. Supply `GITHUB_TOKEN` to
     pinact when available.
   - Validate action inputs against metadata or documentation for the exact
     pinned revision. Treat unknown inputs, stale version comments, mutable
     pins, lint failures, and unreviewed suppressions as findings.
   - Use [validation-and-reporting.md](references/validation-and-reporting.md)
     for evidence categories, tool scope, and the completion report.
8. Review repository enforcement when it is in scope.
   - Read [repository-enforcement.md](references/repository-enforcement.md)
     before auditing or changing Actions policy, required checks, merge rules,
     environments, or the default-branch ruleset.
   - Verify every required status context is a current pull-request job name
     and also runs on `merge_group` when a merge queue uses it. Never create a
     ruleset that can block every merge because its check cannot run.
   - For apply requests, record the requested payload and perform post-change
     read-back. For audit-only requests, leave inaccessible values unverified.
9. Summarize separately:
   - automated checks and exact scope;
   - AI-assisted structural and security inspection;
   - observed facts, applied changes, and post-change verification;
   - blockers, unavailable evidence, approved exceptions, and skipped checks.

## Templates

Read [template-contract.md](references/template-contract.md) before creating or
repairing event-owned validation. The bundled files under `assets/github/`
provide a minimal entry-workflow and tool-installation baseline. Copy them into
the consumer repository, then make only repository-evidenced substitutions.
When an outer source-check action runs Markdown validation, use
`prose-quality-check` as the rule owner and copy its
`assets/markdownlint-cli2.yaml` to `.markdownlint-cli2.yaml`. When an applicable
domain Skill owns a reviewed derived configuration, such as
`hugo-quality-check`, use that domain asset instead. Add only
repository-evidenced generated, vendored, or submodule paths to `ignores`; do
not fork or silently weaken the shared prose baseline.
Consumer CI must run committed repository-owned files and must not execute the
installed Skill at workflow runtime.
