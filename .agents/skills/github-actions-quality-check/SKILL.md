---
name: github-actions-quality-check
description: Quality-check GitHub Actions workflows and composite actions. Use when creating, editing, reviewing, or documenting workflow triggers, permissions, runners, concurrency, action pins, secrets, or CI linting policy.
---

# GitHub Actions Quality Check

## Review automation changes

1. Inspect changed workflow or action files and the repository guidance that describes them.
2. Check triggers, branch filters, merge-queue behavior, and `workflow_dispatch` against the intended responsibility.
3. Check workflow and job `permissions`. Start from `contents: read`, grant only required access, and document unusual write access.
4. Check concurrency groups and cancellation rules for PRs, pushes, releases, merge queues, and publishing.
5. Check runner labels, local composite actions, expressions, comments, cache paths, and suppressions. Add `.github/actionlint.yaml` only for deliberate labels or variables that actionlint cannot infer.
6. Pin third-party actions and reusable workflows to complete commit SHAs with accurate version comments. For external actions, downloaded tools, or containers, use `security-check` to review provenance, release age, pinning, permissions, and runtime behavior.
7. Run the repository's documented `actionlint`, ShellCheck, and `pinact` checks. Use `pinact run --check --min-age 7` for pin verification and `GITHUB_TOKEN` when available. Check standalone changed automation shell scripts with ShellCheck; if no targets exist, record that scope.
8. Summarize actionlint, ShellCheck, pinact, other automated checks, AI-assisted inspections, and skipped checks separately. Record any reduced validation scope caused by unavailable optional integrations.

Use `document-quality-check` for contributor-facing workflow guidance or PR notes. Keep workflows deterministic, least-privilege, and reviewable without adding unnecessary runtime dependencies.
