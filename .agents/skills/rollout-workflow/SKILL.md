---
name: rollout-workflow
description: Roll out a validated canonical change from a designated primary repository to compatible peers with only necessary local differences. Use for shared Agent Skills, repository rules, workflows, or documentation conventions.
---

# Rollout Workflow

Treat the primary repository as the canonical source. Propagate only a change
that is already validated there; do not use rollout work to redesign it.

## Establish scope

1. Use a user-designated or already documented primary; do not select one by
   apparent completeness. Name the primary repository, peer repositories,
   canonical source paths, and exact change range. Stop for clarification when
   no primary is explicit.
2. Confirm each peer has the matching destination and intended compatibility.
   Exclude a peer when its architecture, policy, or repository convention makes
   the primary change inapplicable; record the reason instead of forcing parity.
3. Inspect every peer for local changes at the destination before editing. Do
   not overwrite peer-owned content or unrelated work.

## Propagate with minimal difference

1. Copy the canonical content unchanged whenever it is portable.
2. Make a peer-specific edit only for a concrete local path, product name,
   command, configuration, or policy difference. Keep that edit narrow and
   explain it in the commit or PR.
3. Do not vary structure, terminology, examples, filenames, or metadata merely
   because a peer is a different repository.
4. Compare each peer destination with the primary after editing. Every remaining
   difference must have a documented local reason.

## Validate and publish

1. Run the primary validation and each peer's applicable validation.
2. Check Markdown links, whitespace, generated metadata, and repository status.
3. Commit each peer as a focused rollout. Preserve existing history; do not
   amend or combine unrelated work.
4. In each PR, link the primary change and companion rollouts. State the
   canonical source, any intentional differences, and validation results.

## Handoff checklist

- The primary source and propagated range are explicit.
- Each included peer either matches the primary or has a concrete, documented
  reason for every difference.
- No peer-local behavior was changed only to make files look uniform.
- Validation and PR links make the rollout auditable.
