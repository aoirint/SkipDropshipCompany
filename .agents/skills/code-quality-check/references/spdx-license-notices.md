<!-- SPDX-License-Identifier: MIT -->

# SPDX License Notice Decision Guide

## Purpose

Use this reference when a code-quality review needs concrete SPDX guidance for copied, adapted,
generated, vendored, or externally influenced files. It is practical engineering guidance, not legal
advice. When the license impact is unclear or high-risk, record the uncertainty and ask a maintainer
or legal reviewer before changing notices.

## Contents

- Basic principles
- Straightforward project-local files
- Reuse type decisions
- License structure decisions
- SPDX header formats
- Practical notes
- Decision flow
- Essential summary

## Basic Principles

- Do not remove original copyright notices.
- Do not change a license unless the license terms and project maintainer explicitly allow it.
- Add your own copyright for substantial new authorship by stacking notices instead of overwriting
  existing ones.
- Copy upstream copyright holders and years from the exact source version you used. Do not invent
  placeholder holders or normalize away contributor wording.
- Check companion obligations such as `NOTICE`, attribution files, or license exceptions before
  declaring the SPDX header complete.

Example:

```python
# SPDX-FileCopyrightText: 2023 Upstream Project Contributors
# SPDX-FileCopyrightText: 2026 Example Maintainer
# SPDX-License-Identifier: MIT
```

## Straightforward Project-Local Files

For files created wholly for this repository, use the repository's existing notice convention. Check
nearby files, `LICENSE*`, package metadata, repository docs, templates, generation scripts, and task
instructions before adding a new header style.

Default to [`MIT`](https://spdx.org/licenses/MIT.html) only when both checks are true:

1. No project-specific instruction overrides it.
2. The file is not copied, translated, generated from, adapted from, vendored from, or substantially
   derived from third-party material, and is not constrained by an upstream template, dependency
   license, code generator, protocol specification, sample, or asset with its own terms.

If either check is uncertain, do not claim a new license. Keep the existing project convention when
available, record the uncertainty in the final summary or PR notes, and ask for maintainer input.

## Reuse Type Decisions

### Near-Verbatim Copy

Treat a near-verbatim copy as a derivative of the original file. Preserve the upstream notices and
license. Add source context when the file is copied into the repository.

```python
# SPDX-FileCopyrightText: 2023 Upstream Project Contributors
# SPDX-License-Identifier: Apache-2.0
# Source: https://example.com/upstream/project/file.py at commit 0123456
```

If you add substantial project-specific changes, stack your own copyright notice too:

```python
# SPDX-FileCopyrightText: 2023 Upstream Project Contributors
# SPDX-FileCopyrightText: 2026 Example Maintainer
# SPDX-License-Identifier: Apache-2.0
# Source: https://example.com/upstream/project/file.py at commit 0123456
```

### Partial Reuse or Adaptation

Treat copied structure, non-trivial code fragments, translated code, or adapted algorithms expressed
from source code as likely derivative. Preserve the upstream license and add your own copyright only
for your changes.

```javascript
// SPDX-FileCopyrightText: 2021 Upstream Widget Authors
// SPDX-FileCopyrightText: 2026 Example Maintainer
// SPDX-License-Identifier: BSD-3-Clause
// Adapted from: https://example.com/widget/parser.js at v1.4.2
```

When only a small excerpt is embedded in an otherwise original file, prefer isolating the excerpt or
recording both licenses with `AND` if both sets of terms apply to the combined file:

```python
# SPDX-FileCopyrightText: 2021 Upstream Widget Authors
# SPDX-FileCopyrightText: 2026 Example Maintainer
# SPDX-License-Identifier: MIT AND BSD-3-Clause
# Contains adapted parsing table from Upstream Widget v1.4.2.
```

### Idea-Only Reference

Ideas, behavior, APIs, and facts are usually not licensed like copied expression. If you wrote fresh
code without copying expressive source text or structure, use the repository's license convention
for the new file. Cite the source only when it materially helps reviewers understand provenance.

```python
# SPDX-FileCopyrightText: 2026 Example Maintainer
# SPDX-License-Identifier: MIT
# Design note: behavior cross-checked against Example Tool v2.0 documentation.
```

Do not claim upstream copyright or license merely because you learned the idea from a project.

## License Structure Decisions

### Single License

When a file is wholly original to this repository or derived from a single upstream license, use
that one license identifier.

```shell
# SPDX-FileCopyrightText: 2026 Example Maintainer
# SPDX-License-Identifier: MIT
```

For a copied file:

```shell
# SPDX-FileCopyrightText: 2022 Upstream Script Authors
# SPDX-License-Identifier: Apache-2.0
# Source: https://example.com/upstream/scripts/install.sh at v0.9.0
```

### Multiple Licenses Offered With `OR`

Use `OR` only when the rightsholder offers a choice of licenses and the file may be used under any
one of them. Preserve the offered choices or explicitly record the selected compatible choice when
project policy requires one.

```c
/* SPDX-FileCopyrightText: 2020 Upstream Library Authors
 * SPDX-License-Identifier: MIT OR Apache-2.0
 */
```

If the project intentionally chooses one offered license for the imported copy, make that choice
reviewable in a nearby source comment or PR note:

```c
/* SPDX-FileCopyrightText: 2020 Upstream Library Authors
 * SPDX-License-Identifier: Apache-2.0
 * Source: Upstream Library v3.1, used under its Apache-2.0 option.
 */
```

Do not rewrite `MIT OR Apache-2.0` to `MIT` just because the repository default is MIT
unless the upstream grant allows that choice and the project intentionally makes it.

### Mixed Original and Copyleft Code

When original project code is combined with copyleft-licensed code, decide whether the combined file
or distributed work must follow the copyleft license. GPL-family code often requires the combined
derivative work to be distributed under GPL-compatible terms.

Prefer splitting copied or adapted copyleft material into a separate file with its own clear notice
when that preserves provenance and reduces licensing ambiguity.

For an adapted GPL file:

```python
# SPDX-FileCopyrightText: 2019 Upstream GPL Project Contributors
# SPDX-FileCopyrightText: 2026 Example Maintainer
# SPDX-License-Identifier: GPL-3.0-or-later
# Adapted from: https://example.com/gpl-project/module.py at commit 89abcde
```

For a file with separable sections under different terms, prefer splitting files. If splitting is
not practical and both terms apply, use an expression that represents the combined obligations:

```python
# SPDX-FileCopyrightText: 2019 Upstream GPL Project Contributors
# SPDX-FileCopyrightText: 2026 Example Maintainer
# SPDX-License-Identifier: GPL-3.0-or-later AND MIT
```

Do not add a project-default permissive header to a derivative of GPL code unless a maintainer has
confirmed the resulting distribution terms.

## SPDX Header Formats

Use a valid [SPDX License List](https://spdx.org/licenses/) identifier for a single license, or an
[SPDX expression](https://spdx.github.io/spdx-spec/v2.2.2/SPDX-license-expressions/) with `AND`,
`OR`, `WITH`, and parentheses when multiple licenses or exceptions apply.

If a required license cannot be represented by a standard SPDX identifier, use the repository's
existing `LicenseRef-*` convention or document the exact license text/location before adding the
file.

### Minimal Header

Use this when neighboring files use license-only headers and provenance is fully project-local:

```python
# SPDX-License-Identifier: MIT
```

### Recommended Header

Use this for most new reusable source, scripts, templates, and examples:

```python
# SPDX-FileCopyrightText: 2026 Example Maintainer
# SPDX-License-Identifier: MIT
```

### Derivative Header

Use stacked copyright notices plus source context:

```python
# SPDX-FileCopyrightText: 2021 Upstream Project Contributors
# SPDX-FileCopyrightText: 2026 Example Maintainer
# SPDX-License-Identifier: Apache-2.0
# Source: https://example.com/upstream/project/file.py at commit 0123456
```

### Multiple-License Expressions

Use parentheses when they remove ambiguity:

```python
# SPDX-License-Identifier: (MIT OR Apache-2.0) AND BSD-3-Clause
```

## Practical Notes

### Source Comments

Keep source comments short, stable, and reviewable. Include the upstream project, URL, version or
commit, and whether the file was copied or adapted.

```python
# Source: Example Parser v1.2.0, adapted from parser/table.py at commit 0123456.
```

For generated files, prefer generator metadata when available:

```typescript
// SPDX-FileCopyrightText: 2026 Example Maintainer
// SPDX-License-Identifier: MIT
// Generated from schemas/widget.schema.json by scripts/generate-widget-types.ts.
```

For Apache-2.0 material, record whether a `NOTICE` file or equivalent attribution file was checked:

```python
# Source: Example Parser v1.2.0, adapted from parser/table.py at commit 0123456.
# NOTICE: Upstream NOTICE checked; no additional file-level attribution required.
```

### Common Bad Examples

Do not add a misleading SPDX header to files that quote command output, vendored material, or
documentation examples unless the header describes the file itself. If needed, isolate literal
examples so license scanners do not confuse illustrative text with the file's own notice.

Do not remove upstream notices:

```python
# Bad: replaced the upstream copyright holder.
# SPDX-FileCopyrightText: 2026 Example Maintainer
# SPDX-License-Identifier: MIT
```

Do not change licenses casually:

```python
# Bad: file was copied from Apache-2.0 upstream but relicensed without permission.
# SPDX-License-Identifier: MIT
```

Do not erase dual-license choices without a recorded decision:

```python
# Bad: upstream said MIT OR Apache-2.0, but the choice is not documented.
# SPDX-License-Identifier: MIT
```

Do not label a file as original when it contains adapted copyleft code:

```python
# Bad: adapted GPL logic cannot be hidden under a project-default permissive header.
# SPDX-License-Identifier: MIT
```

## Decision Flow

1. Identify whether the file is new, copied, generated, vendored, downloaded, or adapted.
2. Check nearby files, repository metadata, templates, and task instructions for the local notice
   convention.
3. If external material influenced the file, identify the source URL, version or commit, and
   upstream license.
4. Decide the reuse type:
   - Near-verbatim copy: preserve upstream notice and license.
   - Partial reuse or adaptation: preserve upstream notice, add your own notice for substantial
     changes, and keep applicable upstream terms.
   - Idea-only reference: use the repository convention and add only a brief source note if useful.
5. Decide the license structure:
   - Single license: use one SPDX identifier.
   - Offered alternatives: keep `OR` or record the selected allowed option.
   - Mixed or copyleft-sensitive code: split files when possible; otherwise use an expression that
     reflects all applicable terms and ask for review when uncertain.
6. Write the header with the file's comment syntax.
7. Re-read the result and confirm it did not remove upstream rights, invent relicensing authority,
   or hide copyleft obligations.
8. Record source, version, license validation method, and any uncertainty in the PR notes.

## Essential Summary

SPDX notices make rights and conditions machine-readable. They do not grant permission that the
project does not already have.

Think in layers: preserve upstream rights and obligations, add your own authorship when you add
substantial expression, and describe the resulting stack with an SPDX identifier or expression that
matches the file's real provenance.
