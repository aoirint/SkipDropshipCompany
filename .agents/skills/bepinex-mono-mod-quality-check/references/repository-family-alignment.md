# Repository Family Alignment

Use this procedure when the maintainer names peer repositories, asks for a
horizontal rollout, or expects a new mod to follow an established repository
family.

## Contents

- [Establish the comparison](#establish-the-comparison)
- [Build a delta ledger](#build-a-delta-ledger)
- [Apply the minimum difference](#apply-the-minimum-difference)
- [Documentation and history hygiene](#documentation-and-history-hygiene)
- [Independent review loop](#independent-review-loop)
- [Completion contract](#completion-contract)

## Establish the comparison

1. Record the target repository, every maintainer-designated peer, and the exact
   reviewed revision of each repository. Do not choose a different exemplar
   because it appears newer or more complete.
2. Preserve repository visibility for every target and peer. Inspect private
   repositories through an authorized local checkout or authenticated tooling,
   and do not copy unpublished source, logs, or configuration into public
   issues, pull requests, repositories, or evaluator contexts.
   Privacy can block an external publication side effect; it does not by
   itself justify omitting package assets, validation, or an inert publisher
   action needed to make the repository publication-ready.
3. Define the family-governed comparison set before inventory. It normally
   includes repository governance, toolchain selectors, CI composition,
   packaging and release contracts, documentation topology, and line-ending
   policy. Include product projects, tests, or implementation only when the
   maintainer or a canonical template names a shared structural contract; do
   not require unrelated mod behavior or source files to match.
4. Within that set, inventory tracked paths, directory shape, file order and
   section structure, portable file contents, package-host assets, and newline
   attributes. Compare content structure and newline policy, not only
   filenames.
5. Treat content shared by all designated peers as the presumptive family
   baseline. When peers differ, prefer a documented canonical template, then a
   peer whose role the maintainer designated. If neither exists, preserve the
   target until the difference is resolved or propose a portable improvement
   for the canonical source. Modify the canonical source or other consumers
   only when the maintainer explicitly places them in scope. Do not stop
   unrelated work, select by apparent recency, or silently blend peers.

## Build a delta ledger

Classify every target difference inside the family-governed comparison set
before editing:

| Difference | Required question |
| --- | --- |
| Missing from target | What target constraint makes the shared file or section inapplicable? |
| Extra in target | What product, runtime, test, host, visibility, or maintenance requirement needs it? |
| Changed in target | Why can the portable peer content not be reused unchanged? |

For each row, record the exact target path, peer path, disposition, and reason.
Use one of these dispositions:

- `match`: copy or render the canonical content exactly;
- `target-specific`: keep the smallest necessary difference and name the
  concrete constraint;
- `canonical-improvement`: propose the canonical change first; apply and
  validate it only in explicitly authorized repositories, and otherwise record
  the unperformed rollout;
- `remove`: delete an unexplained target-only addition after preserving any
  still-required facts in their canonical owner.

"Stricter", "cleaner", "newer", "more explicit", and "best practice" are not
sufficient reasons to fork a shared file. If a stricter control is portable,
propose it in the canonical source and roll it out; if it is not portable, name
the target-specific threat or compatibility condition.

## Apply the minimum difference

1. Copy exact portable files from the canonical template or render only the
   declared variables. Do not rewrite wording, comments, step names, ordering,
   indentation, or metadata outside those variables.
2. Preserve shared files that the target does not need to execute directly when
   they are part of the family governance contract, such as `AGENTS.md`, APM
   metadata, notices, contributor policy, ownership, and template drift checks.
3. Exclude product behavior and implementation from exact parity unless a
   named structural contract governs them. For governed project structure,
   game integration, tests, active publication steps, and private/public
   release paths, keep only the smallest difference named by the delta ledger.
   Keep portable package assets, final-archive validation, and publication
   tooling aligned when a family distribution host is confirmed, even when an
   external publish step remains disabled pending authorization or credentials.
4. Apply the shared `.gitattributes` before broad edits and renormalize tracked
   text. Verify the worktree newline policy separately from Git's normalized
   index representation.
5. Do not add documentation categories or evidence machinery merely because a
   general baseline permits them. Match the family document map and add a
   concern only when it has a distinct owner, audience, and change lifecycle.
6. Keep the family workflow entry points and ownership boundaries. When the
   family uses `pull-request.yml` and `main.yml`, add target-specific jobs or
   values inside that split instead of inventing a combined workflow. Keep
   packaging in CI, preserve stable publication while version `0.0.0` blocks
   it, and remove duplicate packagers only after CI owns every required step.

## Documentation and history hygiene

1. Match the family document names, section hierarchy, prose granularity, and
   terminology. Avoid repeating the game version in filenames, headings, and
   prose when one compatibility owner already establishes it. Use direct
   domain wording instead of foregrounding analysis or trace mechanics.
2. Align presentation separately from product evidence. Reuse the family's
   section order, table columns, and prose shape, but preserve product-specific
   setup, settings, limitations, and claims. Keep root and package-facing
   READMEs synchronized on shared facts while retaining their distinct
   maintainer and package-user audiences. Prefer the family configuration table
   when peers use one; copy its shape, not another product's rows.
3. Treat metadata attached to an exactly identified shared artifact as
   portable once identity and version agree. For example, a game-manifest date
   or dependency-release date may move with the same manifest or dependency
   version; it is not evidence that the target mod completed runtime testing.
   Do not delete a portable fact merely because the target did not observe it
   independently. When a low-level identifier is redundant in user-facing
   prose, omit it only after confirming a canonical technical owner retains it.
4. When the maintainer explicitly requests a history rewrite, treat unwanted
   private names, machine paths, or incorrect license text as a reachable-ref
   audit. Before claiming removal, enumerate the retained or published ref
   namespaces in scope, including relevant local and remote-tracking branches,
   tags, notes, stash, and replace refs. Correct the current tree, rewrite only
   the authorized repository, then re-scan the bounded ref set before pushing.
   Coordinate any force-push for already-shared history, do not reproduce the
   removed private text in a public change description, and do not claim
   removal from refs that were not inspected or rewritten.

## Independent review loop

When the maintainer requests convergence review, give a fresh evaluator only
the target, peer revisions, privacy boundary, governed comparison set, and
comparison rule. Use an evaluator authorized for every private repository in
that set; otherwise provide sanitized path/content inventories and diffs that
cannot disclose private implementation. Do not provide the intended fixes or
earlier findings. After each correction round, use a new evaluator and repeat
until a complete pass reports no actionable material difference. Record
genuine target-specific differences so reviewers do not repeatedly propose
destructive parity.

## Completion contract

Report:

- peer revisions and canonical template revisions;
- shared files matched exactly;
- every remaining target-specific difference and its concrete reason;
- canonical improvements and their rollout status;
- line-ending, template-drift, build, test, CI, package, and privacy checks; and
- any difference still lacking a disposition.

Do not call family alignment complete while an addition, omission, content
fork, or newline-policy change in the governed comparison set lacks a ledger
disposition.
