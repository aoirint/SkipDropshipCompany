---
name: github-actions-quality-check
description: Quality-check GitHub Actions workflows and composite actions. Use when creating, editing, reviewing, or documenting workflow triggers, permissions, runners, concurrency, action pins, secrets, or CI linting policy.
---

# GitHub Actions Quality Check

## Review automation changes

1. Inspect changed workflow or action files and the repository guidance that describes them.
2. Check triggers, branch filters, merge-queue behavior, and `workflow_dispatch` against the intended responsibility.
   Treat an established publication trigger and canonical version source as repository contracts:
   preserve them unless the requested change explicitly replaces them.
3. Check workflow and job `permissions`. Start from `contents: read`, grant only required access, and document unusual write access.
4. Check concurrency groups and cancellation rules for PRs, pushes, releases, merge queues, and publishing.
5. Check runner labels, local composite actions, expressions, comments, cache paths, and suppressions.
   Validate every action input against the documentation or metadata for the exact pinned action
   version; do not infer an input name from a different action, tool, or release. Add
   `.github/actionlint.yaml` only for deliberate labels or variables that actionlint cannot infer.
6. Pin third-party actions and reusable workflows to complete commit SHAs with accurate version comments. For external actions, downloaded tools, or containers, use `security-check` to review provenance, release age, pinning, permissions, and runtime behavior.
7. Run the repository's documented `actionlint`, ShellCheck, and `pinact` checks. Use `pinact run --check --min-age 7` for pin verification and `GITHUB_TOKEN` when available. Check standalone changed automation shell scripts with ShellCheck; if no targets exist, record that scope.
8. Summarize actionlint, ShellCheck, pinact, other automated checks, AI-assisted inspections, and skipped checks separately. Record any reduced validation scope caused by unavailable optional integrations.

## Review publishing workflows

- Gate immutable publication on all required quality and build results for the exact source commit.
  A separately running check is not a gate unless repository rules or the publishing workflow
  demonstrably waits for its success.
- Derive tags, release identity, and artifacts from the repository's canonical version source.
  Bind newly created tags and resumable drafts to the reviewed source commit.
- Make retried automation idempotent. Define safe behavior for development placeholders, an
  already-published immutable version, and a matching mutable draft; fail on conflicting tags,
  drafts, or releases.
- Before treating an existing immutable release as a successful no-op, verify its exact expected
  assets and the attestations or provenance required by the repository's supported release
  contract. A rerun must not hide a failed post-publication verification.
- Keep validation read-only and isolate release credentials in the publishing job or protected
  environment. Use `security-check` for final-artifact and archive inspection.

## Review repository enforcement

- Compare required status-check contexts with the exact check names produced by current workflows.
  Check the integration-branch freshness policy and merge-queue compatibility when applicable.
- Verify release immutability, tag rules, Actions permissions, and protected environments from
  repository settings. If settings are inaccessible, report the corresponding readiness claim as
  unverified.

Use `prose-quality-check` for contributor-facing workflow guidance or PR notes. Keep workflows deterministic, least-privilege, and reviewable without adding unnecessary runtime dependencies.
