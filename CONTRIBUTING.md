<!-- SPDX-License-Identifier: MIT -->

# Contributing

Thank you for your interest in improving SkipDropshipCompany. This project
welcomes focused bug reports, documentation improvements, compatibility notes,
and small code changes that are easy to review.
The project maintainer is listed in [CODEOWNERS](./.github/CODEOWNERS).

## Before you start

- Check the existing [issues](https://github.com/aoirint/SkipDropshipCompany/issues)
  and [pull requests](https://github.com/aoirint/SkipDropshipCompany/pulls)
  to avoid duplicate work.
- Open an issue first for larger behavior changes, compatibility changes, or
  anything that may affect release packaging.
- Keep changes focused. Separate unrelated fixes, refactors, and documentation
  updates into separate pull requests when practical.

## Reporting issues

- Use [GitHub Issues](https://github.com/aoirint/SkipDropshipCompany/issues)
  for ordinary bug reports, feature requests, compatibility notes, and
  documentation improvement requests.
    - To minimize the personal burden this project places on maintainers,
      maintainers try to keep the project's responsibilities as small as
      practical. However, issues or requests that seem relevant can still be
      useful as related information for other users even if they are closed as
      not planned. This is likely for requests outside the project's scope,
      requests for large or complex features, or compatibility requests for base
      game versions that are no longer current.
- Before opening a new issue, check the existing
  [issues](https://github.com/aoirint/SkipDropshipCompany/issues) and
  [pull requests](https://github.com/aoirint/SkipDropshipCompany/pulls) to avoid
  duplicates.
- When you share logs, screenshots, or other supporting material in a public
  issue, expect maintainers to use that material within a reasonable scope
  related to the reported issue, including for understanding, reproducing,
  discussing, fixing, and communicating with end users about it.
- Only share material that you have the right to share, and do not include
  secrets, personal information, private data, or content that should not be made
  public.
- If AI assistance significantly shapes an issue report or issue comment,
  disclose that assistance in the issue or comment. This helps maintainers judge
  what context may need closer checking, such as reproduction steps,
  compatibility claims, log interpretation, or documentation suggestions. See
  [AI assistance](#ai-assistance).
- Do not submit sample code, documentation text, patches, or other material that
  could be included or adapted into the project unless you provide it under the
  [Contribution License Agreement](#contribution-license-agreement). Clearly
  state the same confirmation used for pull requests when you submit that
  material: `I have read CONTRIBUTING.md and agree to the Contribution License
  Agreement.`
- Do not report security issues in public GitHub Issues. See
  [Reporting security issues](#reporting-security-issues) instead.

## Stalled Issues

- Respond to maintainer questions as much as you reasonably can. If you need
  more time, are blocked from providing the requested information, or find that
  the issue is no longer reproducible, leave a short comment so maintainers know
  what to expect. Even if a long time has passed, it is always fine to reply
  with an update.
- To keep maintainer work manageable and the issue list current, issues that
  cannot move forward because needed information is missing, the issue is no
  longer reproducible, the reported behavior no longer matches the current
  project, or the discussion has been inactive for a reasonable period may be
  closed. This is not a judgment on the reporter or the report, and it does not
  prevent you from reopening the issue, or asking maintainers to reopen it, if
  the issue is still relevant and it is reasonable to do so.

## Development setup

Follow the setup, formatting, build, package management, debugging, and release
notes in [README.md](./README.md).
At minimum, install the documented .NET SDK and restore packages before building:

```powershell
dotnet restore --locked-mode
```

## Making changes

- Prefer the existing project structure and naming conventions.
- Keep user-facing behavior explicit in code, documentation, or changelog
  entries when the behavior changes.
- Update [CHANGELOG.md](./CHANGELOG.md) for developer-facing changes that should
  appear in release history.
- Update files under [assets/](./assets/) when the Thunderstore package
  metadata, icon, README, or release notes change.
- Do not commit build output, downloaded game files, local mod manager profiles, or local machine configuration.

## Verification

Run the checks that match your change before opening a pull request:

```powershell
dotnet format
docker run --rm --network none --user 1000:1000 -v ".:/workdir" davidanson/markdownlint-cli2:v0.22.1@sha256:0ed9a5f4c77ef447da2a2ac6e67caf74b214a7f80288819565e8b7d2ac148fe5
shellcheck .github/actions/publish-thunderstore/publish-thunderstore.sh
actionlint -pyflakes=
pinact run --check --min-age 7
DOTNET_CLI_UI_LANGUAGE=en dotnet build
```

Use the commands as follows:

- Run `dotnet format` and `DOTNET_CLI_UI_LANGUAGE=en dotnet build` for source
  changes.
- Run Markdown lint for documentation changes.
  The Docker command is the documented pinned path, but Docker is not required.
  Another `markdownlint-cli2` installation method is acceptable when it uses the
  repository configuration.
- Run `shellcheck`, `actionlint`, and `pinact` when changing GitHub Actions
  workflows, composite actions, shell scripts, or related repository automation.
  ShellCheck should run before actionlint so actionlint can use its ShellCheck
  integration for inline workflow shell scripts.

On Linux, run the Markdown lint command with `sudo docker` and use
`--user "$(id -u):$(id -g)"`.

For package or release changes, also verify the release documentation in
[README.md](./README.md) and confirm that the Thunderstore-facing files under
`assets/` are still correct.

## Pull requests

- Use a clear title that summarizes the change.
- Describe what changed and how you verified it.
- Link related issues when applicable.
- Keep the pull request small enough for maintainers to review without guessing
  at unrelated intent.
- If AI assistance significantly shapes the pull request, disclose that
  assistance in the pull request description. See
  [AI assistance](#ai-assistance).
- Pull requests must include the
  [pull request template](./.github/pull_request_template.md) checkbox
  confirmation for the
  [Contribution License Agreement](#contribution-license-agreement) before they
  can be merged. Pull requests without that confirmation may be closed without
  further notice.

Use pull request template sections this way:

- `Summary`: user-facing or maintainer-facing changes, grouped by behavior or
  area.
- `Related Issues`: GitHub issues, pull requests, external references, or
  `None`. When linking related work, state how it is related.
- `Notes for reviewers`: limitations, skipped checks, migration notes, reviewer
  attention points, or review focus.
- `AI disclosure`: significant AI assistance details, or `None` when no
  significant AI assistance was used.
- `Testing`: automated commands, CI results, AI-assisted inspections, manual
  checks, screenshots, or videos.
- `Breaking Changes`: required when the title or commits include `!` or
  `BREAKING CHANGE`. Treat backward-incompatible changes to public behavior,
  documented workflows, configuration, package or release behavior,
  compatibility guarantees, or other project-facing contracts as breaking.

## Stalled Pull Requests

- Respond to maintainer feedback as much as you reasonably can. If you need
  more time, are blocked, or no longer plan to continue the pull request, leave
  a short comment so maintainers know what to expect. Even if a long time has
  passed, it is always fine to reply with an update.
- If you want to continue work from a stalled pull request, leave a short
  comment for the maintainer and the original contributor before opening a new
  pull request. The original contributor may not be available to respond, but
  the maintainer can confirm whether the change is still wanted and coordinate
  attribution or next steps.
- To keep work moving, maintainers may accept another contribution for the same
  issue without first rejecting an inactive pull request.
- If a pull request stalls, maintainers or another contributor may continue the
  work in a separate pull request. This may include reusing or adapting the
  stalled pull request's commits, patch, tests, documentation, or ideas under
  the Contribution License Agreement.
- If your pull request reuses substantial work from another pull request, credit
  the original pull request and contributor in your pull request description.
- To keep maintainer work manageable and the review queue current, pull requests
  that remain inactive for a reasonable period may be closed. This is not a
  judgment on the contributor, and it does not prevent you from opening a new
  pull request later if the change is still useful and it is reasonable to do
  so.

## Contribution License Agreement

By submitting a contribution to this project, you agree to this Contribution
License Agreement.
If this agreement changes, new pull requests must use the current agreement.

For this agreement, "you" means the person or organization submitting the
contribution, and "contribution" means code, documentation, assets, patches,
generated output, or other material that you intentionally submit for inclusion
in this project. Ordinary issue reports, pull request discussion, questions, and
suggestions are not contributions under this agreement unless you clearly submit
them for inclusion in the project.

For disclosure expectations when AI assistance affects contributions, issue
reports, comments, or other project-facing material, see
[AI assistance](#ai-assistance).

By submitting a contribution, you represent and agree that:

- You have the legal right to submit the contribution and to grant the rights
  described in this agreement.
- Your contribution may be distributed under the same license as this project,
  without additional terms or conditions.
- You grant the maintainer and downstream recipients a perpetual, worldwide,
  non-exclusive, no-charge, royalty-free, irrevocable copyright license to use,
  copy, modify, merge, publish, distribute, sublicense, and otherwise use your
  contribution as part of this project.
- You grant the maintainer and downstream recipients a perpetual, worldwide,
  non-exclusive, no-charge, royalty-free, irrevocable patent license to make,
  have made, use, offer to sell, sell, import, and otherwise transfer your
  contribution as part of this project. This patent license applies only to
  patent claims that you can license and that are necessarily infringed by your
  contribution alone or by combining your contribution with the project.
- You keep any copyright you hold in your contribution. This agreement is a
  license grant, not a copyright assignment.
- The maintainer is not required to accept, publish, retain, or distribute any
  contribution.
- Do not submit code, documentation, assets, generated output, or other
  materials if you do not have the right to contribute them under this
  agreement.

## AI assistance

AI tools may be used as aids, but the human submitter remains responsible for
the material they submit.

When AI assistance is significant, disclose it where maintainers and reviewers
will see the relevant context:

- For a pull request, disclose it in the pull request description.
- For an issue report, issue comment, or other project-facing material,
  disclose it where you submit that material.

The purpose of disclosure is to give reviewers and maintainers enough context to
understand where to focus, including areas you may not have reviewed closely.
There is no exact percentage or universal rule for when assistance is
"significant", so use the following common workflows as practical examples, not
as an exhaustive list.

Examples that should be disclosed:

- AI-generated or substantially AI-rewritten code, documentation, assets, tests,
  patches, release notes, issue reports, comments, or maintainer-facing text.
- Agent-made changes to a pull request, including changes made after automated
  review.
- AI assistance that significantly shaped reproduction notes, compatibility
  notes, log interpretation, documentation improvement requests, or other triage
  information.
- When possible, mention:
    - The rough prompt you gave an agent.
    - The changes you asked it to make.
    - For pull requests, what you focused on when reviewing or adapting the
      result.
- If there are areas you did not review closely or are less confident about,
  mention them so reviewers can check those areas more carefully.

Examples that normally do not need disclosure:

- Ordinary spell-checking, formatting, search, translation used only for your own
  understanding, or small completion suggestions normally do not need disclosure
  unless they make up a main part of the change itself.

When in doubt:

- If you are unsure whether assistance was "significant", treat it as
  significant and describe what you did.

Submitter responsibilities:

- Review AI-assisted material yourself. Do not assume generated code,
  documentation, tests, explanations, summaries, or issue details are correct.
  Understand and be ready to explain the material as much as you reasonably can
  instead of leaving that work to maintainers.
- For pull requests, provide verification evidence that matches the change, such
  as automated test output, build output, screenshots, or a clear description of
  any hands-on checks that could not reasonably be automated.
- Do not present AI-performed review, inspection, editing, verification, or
  other work as "manual". For example, if you include AI-assisted inspection,
  describe the request first, nest the AI result under it, and clearly label the
  result as AI-assisted.
- Keep pull requests and other project-facing material reviewable. Do not submit
  either large, hard-to-review changes or high volumes of issues, comments, or
  pull requests that a maintainer cannot reasonably check, whether they were
  generated by AI tools or written manually.
- Agent-generated pull requests are allowed only when a human contributor
  understands the change, adapts it to this codebase, verifies it, discloses the
  assistance, and can personally explain and maintain it.
- Do not submit low-effort AI-generated or "vibe-coded" pull requests that do
  not meet those requirements.
- Write pull request descriptions and review replies in your own words. Use AI
  for editing help only if you still review and stand behind the final text.
- Maintainers may close undisclosed, unverified, low-quality, or spam-like
  AI-assisted pull requests, issues, comments, or other project-facing material.

## Reporting security issues

If you suspect you found a security issue, do not share exploit details publicly
or with untrusted recipients.

This includes, but is not limited to:

- Public issues.
- Social media.
- Blog posts.
- Livestreams.
- Video uploads.
- Similar public channels.

Report the issue to the maintainer through a private and secure channel when
possible, or to a trusted security organization.
