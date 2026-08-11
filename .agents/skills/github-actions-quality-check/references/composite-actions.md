# Composite Action Contracts

## Common contract

Use a Composite Action for one policy-neutral capability that always runs on
the caller's runner. Keep checkout, events, permissions, concurrency,
environments, and required-context policy in the entry workflow. Use a reusable
workflow instead when reuse needs a runner or matrix, job permissions, services,
secrets, environments, job outputs, or cross-job artifacts.

Keep the interface consistent without forcing implementations to be identical:

- Put the action at `.github/actions/<verb>-<domain>-<scope>/action.yml`. Omit
  `<scope>` only when `<verb>-<domain>` is unique and remains unambiguous in a
  multi-language monorepo. Examples include `check-python-api`,
  `test-node-web`, and `build-docker-worker`.
- Use kebab-case for paths, input/output names, and step IDs.
- Name the action with an imperative, domain-qualified responsibility such as
  `Check Python package` or `Build Docker image`. Do not use bare `CI`, `Setup`,
  `Check`, `Test`, or `Build`.
- Name each step with a verb phrase for its observable result. Mention a tool
  only when the tool distinguishes the responsibility.
- Let callers own `working-directory` unless directory selection is a genuine
  reusable input. Do not add speculative inputs or outputs.
- Give one action one end-to-end runner capability. If it needs a runtime,
  package manager, dependency restore, or cache to produce its result, the
  action owns that setup, cache binding, locked install, and cleanup. Do not
  require callers to assemble lifecycle fragments in the correct order.
- Use `${{ github.action_path }}` to invoke files shipped beside the action;
  use `${{ github.workspace }}` only for consumer repository paths.
- Keep one blank line between sibling steps. Do not put a blank line between a
  list item and its `uses`, `with`, `shell`, `env`, or `run` properties.

The workflow context vocabulary and the action vocabulary answer different
questions. A job may have stable ID `check` and visible context `Check`, then
call `.github/actions/check-python` named `Check Python package`. Qualify a
visible context only when one event would otherwise emit duplicates, for
example `Python Check` and `Docker Check`. For matrices, qualify workers and
require one unqualified aggregate context when repository policy needs a stable
single gate.

## Ownership and composition

Keep each reusable implementation in one owning Skill. Other Skills may link
to it, render it from that owner, or compose the installed local action; they
must not carry a second editable copy. A domain Skill owns only behavior unique
to its domain. A shared runtime or workflow-tool installer belongs to the
corresponding ecosystem or GitHub Actions Skill.

Composition must preserve the consumer repository as a self-contained unit:
copy every selected action and its adjacent scripts into `.github/actions/`,
then use repository-local `uses:` paths. Do not make a generated consumer
action reach into an installed Skill directory at workflow runtime.

Split actions at independently reusable lifecycle boundaries, not at every
command. An internal setup action may centralize one runtime contract for
several outer actions, but each check, test, build, or deploy action must call
that setup itself and finish its own lifecycle. Avoid both a monolithic
multi-ecosystem switch and caller-visible `setup` / `restore` / `cleanup`
fragments.

Composite Actions have no `post` phase. When a capability creates a
repository-local dependency tree or other disposable state, add an
`if: always()` cleanup step to the outer action that created it. Put ephemeral
tools and caches in runner-temporary or tool-managed locations when possible;
document why no explicit cleanup is required. Cleanup must not erase committed
files, caller-owned outputs, or caches managed by the runner service.

## Embedded-code boundary

Treat `run:` as a program, regardless of YAML location. Keep only one command or
short, linear environment/output plumbing inline. Move code beside the action
when it contains parsing, loops, functions, multi-branch decisions, reusable
behavior, non-trivial quoting, or logic that merits focused tests.

Invoke a bundled program directly and preserve its exit status:

```yaml
- name: Check project metadata
  shell: bash
  env:
    EXPECTED_VERSION: ${{ inputs.expected-version }}
  run: python3 "${{ github.action_path }}/check_project.py"
```

Do not hide a language program in a Bash heredoc. Pass expressions through
`env:` instead of interpolating untrusted or syntactically significant values
into source text. Keep secrets out of command arguments and diagnostics.

## Language-specific contract

| Domain | Action example | Implementation and paired review |
| --- | --- | --- |
| Shell | `Check shell source` | Use strict failure propagation appropriate to the shell, quote expansions, and run ShellCheck. Put multi-step logic in a `.sh` file and apply `code-quality-check`. |
| Python | `Check Python package` | Put maintained logic in typed functions plus a thin `main()` returning an exit code. Run Ruff formatting/lint, strict mypy over the script, and focused tests with statement and branch coverage under `python-quality-check`. |
| Node.js | `Check Node web source` | Derive Node and package-manager versions from committed metadata, restore the keyed cache, replay the frozen lock, call repository scripts, and remove the action-created dependency tree with `if: always()`. Apply `node-quality-check`; do not embed JavaScript in YAML or install unpinned tools ad hoc. |
| Docker | `Check Docker source` | Keep Dockerfile, Compose, build-context, and container checks in a Docker-owned action or script. Apply `docker-quality-check` and use a full runner when daemon access is required. |
| .NET / C# | `Test .NET package` | Derive the SDK from committed configuration, restore locked dependencies, and keep build/test/package commands aligned with the owning ecosystem or domain Skill. Put substantial PowerShell or Bash logic in tested scripts. |
| Hugo | `Check Hugo web site` | Treat Hugo validation as a site responsibility while composing the single Node/pnpm setup implementation owned by `node-quality-check`; complete setup through cleanup inside the Hugo action and apply `hugo-quality-check` for mounts, themes, rendering, and build output. |

Add a language-specific action only when the repository has that responsibility.
Do not create one monolithic action that conditionally detects ecosystems, and
do not copy a common body across languages. Share the interface and naming
rules; let each ecosystem own its commands, lockfiles, runtime, cache, and tests.

## Verification

Run `actionlint` for workflows and use an action-aware validator or a temporary
`.github/actions/` fixture for action metadata. Run ShellCheck for embedded and
standalone shell, plus the formatter, linter, type checker, and tests required
by each maintained language. Run `pinact run --check --min-age 7` for external
actions. Finally, exercise every changed Composite Action through a
representative caller workflow; YAML parsing alone does not prove its path,
inputs, outputs, runner assumptions, or exit behavior.
