---
name: pull-request-quality-check
description: Quality-check repository pull requests and PR-thread communication. Use when creating, updating, reviewing, or validating PR titles, bodies, review comments, replies, or thread notes.
---

# Pull Request Quality Check

## Check the pull request artifact

1. Identify the artifact: title, body, review, reply, or thread note.
2. For titles, enforce `<type>[optional scope][optional !]: <description>` and use `commit-message-quality-check` for type and breaking-change notation.
3. Before drafting or replacing a body, read the current repository PR template and applicable
   contributor guidance. Follow only their visible headings, required checkboxes, and applicable
   sections; use HTML comments only as guidance, not rendered content. If no template exists, use
   [references/fallback-pr-body.md](references/fallback-pr-body.md). Never infer a CLA,
   contributor agreement, checklist, sign-off, or other repository policy from the fallback. Do
   not invent unavailable template text or retain stale template content.
4. For AI-agent-prepared PR bodies, add this alert at the absolute top:

```markdown
> [!WARNING]
> This pull request was created with assistance from LLMs.
```

Use this alert for AI-assisted reviews, replies, and thread notes:

```markdown
> [!WARNING]
> This comment was created with assistance from LLMs.
```

Omit an alert only when assistance was not significant.

5. Keep verification evidence distinct: automated commands, CI results, non-AI manual checks, screenshots/videos, and AI-assisted inspections. Put requested AI-assisted inspections under `## Testing`, then `### AI-assisted inspections` after automated checks, with `Request: ...` and nested `AI-assisted result: ...`. Never describe AI-performed work as manual.
6. Write in English except for exact source material. Keep bodies reviewable, use bullets and backticks, avoid large diffs, state skipped verification, and use `prose-quality-check` or `security-check` for applicable prose or sensitive content.

## Reviews, notes, and CLI safety

Use `Update Note`, `Discussion Note`, or `Review Note` only when the task requests process, decision, or review-summary notes. Place `Request addressed: ...` after the required alert; group retrospective notes by meaningful theme, separate independently reviewable themes when requested, label inferences, and omit secrets, private paths, and hidden reasoning.

When using `gh`, write Markdown to a temporary file and pass `--body-file`. Verify the stored body with `gh pr view --json body`, correct quoting, and remove the temporary file.
