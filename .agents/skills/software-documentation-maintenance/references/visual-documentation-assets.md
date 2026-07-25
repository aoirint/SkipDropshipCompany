# Visual Documentation Assets

Use this reference when a repository adds or reorganizes diagrams, icons, screenshots, or renderer-specific image
fallbacks. Do not make `diagrams/` or `screenshots/` a top-level documentation concern merely because an asset has
that format. Place it by the responsibility it serves and the lifecycle that changes it.

## Place content and procedures separately

- **Domain or architecture content:** owns the game/platform facts or product-design facts a diagram explains. Link
  to the release asset when it is user-facing; the drawing must not become a second source of those facts.
- **Release material:** owns diagrams, icons, screenshots, package-renderer fallbacks, and other presentation assets
  when they are shared by a release README, package page, or release process. Keep the editable source and its focused
  authoring guide together in `docs/release/` so an update does not separate the asset from the procedure that keeps
  its derivatives and consumers correct. Runtime-package files still belong in the package layout required by the
  package host.
- **Operations:** owns a shared procedure that applies across several assets, such as a renderer invocation, image
  conversion policy, capture environment, or validation matrix. A release-asset-specific guide may stay beside its
  source and link to this shared operation instead of duplicating it.

A captured screenshot is release material or a user-facing example, not a domain or architecture fact and not an
editable diagram. Do not give it an authoring guide unless capture conditions, redaction, transformation, or
compatibility validation are recurring maintenance work.

## Create directories only for an owned question

Create `docs/release/` when release-facing assets need shared ownership, discovery, or maintenance guidance beyond
the README that embeds them. It is especially appropriate when an editable source needs a retained fallback for
another renderer. Do not create an empty future category. If one static screenshot has no procedure, it may remain in
a small asset subdirectory owned by the release-facing document or package source without becoming an indexed
documentation section.

Do not create `docs/diagrams/` just to collect files with a common format. It is appropriate only if the repository
has an independent, cross-cutting diagram system whose maintainer question cannot be owned by domain, architecture,
operations, or release. State that exceptional boundary explicitly in `docs/README.md`.

## Package-facing usage

Treat a package host's Markdown renderer as a distinct release consumer. Verify its URL or archive-path behavior and
asset availability separately from the repository README. A package README may reference a stable repository-hosted
fallback when the host supports it; package a copied image only when the host contract requires it. This choice must
not create a second editable source.

## Rollout rule

Use `rollout-workflow` only after the primary repository has validated each asset's responsibility-based placement.
Copy a directory, source/derivative procedure, or package fallback to a peer only when it has the same responsibility
and lifecycle. Do not create empty categories, relocate a peer's existing assets, or duplicate a fallback merely to
look uniform. Record each different host, renderer, package layout, asset lifecycle, or existing repository convention
as the concrete reason for a narrow peer-specific variation.
