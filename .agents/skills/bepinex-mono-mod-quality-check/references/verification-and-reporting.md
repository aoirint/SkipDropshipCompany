# BepInEx Verification and Reporting

## Verification

1. Run the narrowest relevant checks, then widen for the changed surface.
   - For C# or project changes, run locked restore, format verification, and no-restore build. Run the documented tests
     when automated tests are present or changed. Use the solution or project path required by the repository layout.
   - For documentation or package text, run Markdown lint over every committed Markdown file using the checked-in
     configuration.
   - For workflows, composite actions, or shell scripts, apply the automated and AI-assisted validation scopes from
     `github-actions-quality-check`, then run the BepInEx/.NET/package checks enabled by this Skill.
   - Reconcile declared tooling with execution. Every committed lint/check configuration and every command promised in
     README or CONTRIBUTING must have a runnable documented command and an enabled CI invocation, or be removed with the
     documented reason.
   - For APM changes, run `apm lock`, review locked refs and hashes, run `apm install --frozen`, then `apm audit --ci`.
   - For release or compatibility changes, inspect the final archive and perform runtime validation. State exactly what
     environment, game version, loader version, and mod set were tested; record missing runtime evidence as a blocker
     rather than guessing.
   - When the product requires structured validation logging, check evidence quality as well as event coverage. A record
     that proves a state transition must identify the observed execution role, boundary/source, result, and bounded
     non-sensitive before/after values needed to distinguish successful application from callback reachability. Derive
     authority at the event boundary; use a neutral role when unavailable. Every exception swallowed inside a patch
     callback, including an inner transaction catch, emits the bounded callback-exception event in addition to domain
     restoration evidence. Preserve the product's privacy exclusions.
   - If the maintainer marks content for history rewriting, correct the current tree, inspect every reachable branch and
     tag for the exact unwanted text, and rewrite only the authorized repository before publication or coordinated
     force-push. Re-scan after rewriting. Do not disclose the removed private text in a public commit or pull request.
     Use `commit-message-quality-check` for reconstructed commits so required contribution attribution is preserved.
   - Preserve repository visibility. Creating a repository, changing its visibility, or enabling public package or
     release publication requires an explicit maintainer instruction; a migration request alone does not authorize it.

## Report

1. Report the result in this order: scope and evidence consulted; findings grouped by
   structure/plugin/build/CI/package/runtime; checks passed or skipped; and remaining compatibility or release risks.
   State every omitted baseline item and its concrete constraint; do not report a quality pass based only on an existing
   repository convention.
