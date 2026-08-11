# GitHub Actions Naming and Readability

## Purpose

Make workflow topology understandable without opening every implementation. Apply these rules to
new or materially restructured workflows and local actions. Preserve an established public status
context or integration path until its consumers and repository rules are migrated deliberately.

## Naming layers

| Layer | Rule | Examples |
| --- | --- | --- |
| Workflow file | Use lowercase kebab-case for the event or lifecycle responsibility. Avoid implementation-only names when the workflow owns more than that implementation. | `pull-request.yml`, `main.yml`, `release.yml` |
| Workflow `name` | Use concise Title Case that identifies the lifecycle responsibility and is unique in the repository. Add a domain qualifier only when multiple workflows would otherwise collide. | `Pull Request`, `Main`, `Docker Release` |
| Job ID | Use lowercase kebab-case for one visible responsibility. Prefer stable outcome names over tool names. Qualify only to distinguish parallel responsibilities. | `checks`, `unit-tests`, `build-container` |
| Job `name` | Set an explicit concise Title Case display name. Prefer singular responsibility names, including for required-check contexts. Keep a required-check name stable; for a matrix, include only the dimension needed to distinguish instances. | `Check`, `Unit Test`, `Test (${{ matrix.python-version }})` |
| Composite Action path | Use `.github/actions/<verb>-<domain>-<scope>/action.yml`. Omit the scope only while the remaining name is unique and unambiguous across every language in the repository. Use `action.yml`; let the directory carry the responsibility. | `.github/actions/check-python-api/action.yml`, `.github/actions/build-docker-worker/action.yml` |
| Composite Action `name` | Use a concise responsibility phrase that distinguishes the domain and result. Avoid `CI`, `Check`, `Setup`, or `Build` alone. | `Check Docker source`, `Set up locked Python` |
| Step `name` | Use a responsibility-revealing verb phrase. Distinguish repeated checkout, upload, download, login, and publication steps by source or destination. | `Check out proposed source`, `Upload Linux package` |

Keep names aligned across layers without forcing identical text. A `pull-request.yml` workflow may
be named `Pull Request`, contain a `check` job named `Check`, and call
`.github/actions/check-docker/action.yml` named `Check Docker source`: each layer answers a different
question.

When a repository family standardizes lifecycle workflow names, audit every entry workflow by its
top-level `name`, not by file name or job names. A workflow triggered by pushes to the integration
branch remains `Main` when it also builds or publishes artifacts; keep `Build` and `Deploy` for the
jobs that perform those narrower outcomes. Do not preserve a repository-local workflow-name
exception merely because its file is still named `build.yml`.

Before renaming a workflow or job, inventory branch rules, merge queues, badges, API consumers,
documentation, and reusable-workflow callers. Treat a required job display-name change as a
repository-enforcement migration, not a cosmetic edit.

## Responsibility vocabulary and topology

Start with four broad capability categories when they fit the repository:

- **Check** validates repository contents and constraints, including formatting, lint, types,
  documentation, schemas, generated files, licenses, and policy.
- **Test** exercises software behavior, including unit, integration, end-to-end, smoke,
  compatibility, and release-artifact tests.
- **Build** produces an executable, distributable, or later-stage artifact.
- **Deploy** delivers or publishes an already produced artifact.

Classify by the primary outcome. Compilation performed only to run tests remains part of Test.
Version detection and version locking are workflow release policy, not generic Build or Deploy
capabilities.

Do not turn the vocabulary into a fixed pipeline or four mandatory jobs. Classification describes
responsibility; `needs` describes execution order; jobs and workflows describe operational
boundaries. Combine related checks or tests when separation adds little value. Split a job when
parallel feedback, an independent retry, a different runner, permissions, environment, selective
execution, required-check visibility, or an artifact boundary justifies its startup and maintenance
cost. Add a workflow only when its trigger, trust, permissions, concurrency, environment,
ownership, or operational lifecycle warrants a separate boundary.

Place tests by measured cost, determinism, infrastructure, credentials, security exposure, and
feedback value rather than by the labels `unit`, `integration`, or `end-to-end`. Cheap deterministic
integration tests can belong on pull requests; expensive or credentialed tests may not.
Use `test-quality-check` to classify test evidence and judge its behavioral
value. This reference owns only the operational workflow/job boundary implied
by cost, permissions, runners, credentials, and artifact lineage.

Keep Composite Actions policy-neutral. Name them for reusable capabilities such
as `check-python-api`, `test-node-web`, `build-docker-worker`, or
`deploy-python-package`; do not encode `pull-request`, `main`, or `release`
policy in the action. Treat every repository as though another language or
package may be added later: domain identifies the ecosystem or artifact and
scope identifies the package, service, or deliverable when needed. Workflows
own lifecycle policy and jobs own concrete execution units.

For releases, build once after release identity is locked, test that exact artifact, and deploy the
same artifact. A distinct Release concept does not require a distinct workflow: use a dedicated
workflow only when its operational boundary earns the added complexity; otherwise a conditional
release path in the integration workflow is valid.

## Comments

Add a short comment next to a decision whose reason is not recoverable from the keys themselves:

- why proposed-source and integration-source workflows differ;
- why a run is or is not cancellable;
- why a job needs broader permissions, credentials, a full runner, or an unusual timeout;
- why an artifact is built once, retained, verified, or handed to publication;
- why a suppression, compatibility constraint, or non-obvious expression is necessary.

Explain **why**, not **what**. Do not restate a step name, pin comment, YAML key, or obvious command.
Put a job-wide or step-wide comment immediately above that job or step. Put a property-specific
comment inside the mapping immediately above the affected property. Update or remove the comment
when the design changes.

## Vertical spacing

Use one blank line between sibling job mappings and between sibling step list items. When a leading
comment explains the next sibling job or step, put the sibling-separating blank line before that
comment and keep the comment attached to the item. This blank-before-comment rule does not apply to
a property-specific comment inside one mapping: for example, a runner rationale may follow the job
`name` and sit immediately above `runs-on`. Within a job, separate `runs-on`, `permissions`, `needs`,
`if`, `outputs`, and `steps` into readable logical groups when more than one group is present. Do not
add blank lines that split a single mapping from its properties or a comment from its target.

Review spacing in the rendered or copied consumer file as well as in a template source. YAML parsing
and actionlint do not enforce this readability contract.
