# GitHub Actions Validation and Reporting

## Evidence categories

Keep these categories distinct:

- **Observed**: workflow text, action metadata, API responses, logs, and artifact
  contents actually inspected.
- **Requested**: exact settings or workflow values the maintainer asked to
  apply.
- **Applied**: mutations whose request or command succeeded.
- **Verified**: applied values confirmed by post-change read-back or a run of
  the exact resulting workflow.
- **Unverified**: inaccessible settings, unavailable logs, unsupported
  endpoints, or runs that have not occurred.
- **Exception**: an explicit maintainer decision identifying its narrow scope,
  unmet gate, reason, and follow-up.

Do not convert an unavailable source into a pass. A successful actionlint run
does not prove repository settings, runtime behavior, or artifact correctness.

## Automated checks

- Run actionlint across entry workflows, reusable workflows, and local actions.
  Configure its ShellCheck integration only when the exact ShellCheck command is
  available and record any disabled integration.
- Run ShellCheck on every changed standalone shell script and record an empty
  target set when none exists. Review inline `run:` scripts through actionlint.
- Run pinact across every changed workflow and local action with
  `pinact run --check --min-age 7`. Record each external action's full SHA,
  version comment, publisher, provenance, and release-age evidence.
- Run repository-specific clean-clone commands through the ecosystem Skill.
  Do not invent commands from workflow names.

## Completion record

Report:

1. event and trust boundaries;
2. visible job graph and required-check compatibility;
3. effective workflow/job permissions and credential persistence;
4. runner selection and lifecycle evidence;
5. external action, download, and container review;
6. artifact lineage and release idempotency;
7. actionlint, ShellCheck, pinact, and ecosystem-check results separately;
8. repository-setting evidence and post-change read-back when applicable;
9. skipped checks, unavailable evidence, blockers, residual risk, and approved
   exceptions.
