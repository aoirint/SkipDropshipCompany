<!-- SPDX-License-Identifier: MIT -->

# Release Validation

This runbook is for human-operated validation of SkipDropshipCompany release
candidates. It focuses on observable game behavior and concise evidence that can
be linked from release-preparation issues or pull requests.

## Scope

Validate the release candidate against:

- Lethal Company v81.5.
- BepInExPack v5.4.2305.
- The packaged Thunderstore zip produced by CI.
- Host-only installation, with optional client installation smoke coverage.

## Evidence To Record

- Artifact under test:
    - GitHub Actions run URL.
    - Package zip name.
    - Package digest from the build summary.
    - `manifest.json` `name`, `version_number`, and dependency list.
- Environment:
    - Lethal Company version.
    - BepInExPack version.
    - Other installed mods that can affect terminal purchases, routing, moons,
      dropship timing, or item spawning.
- Result summary:
    - Passed checks.
    - Failed checks.
    - Missing coverage.
    - Accepted residual risks.
    - Follow-up issues.

Do not post player names, lobby identifiers, account identifiers, machine names,
tokens, profile paths, or full logs that include unrelated private data.

## Checklist

### Package Startup

1. Install the release-candidate zip in a clean mod profile.
2. Start Lethal Company through the mod manager or BepInEx profile.
3. Confirm the BepInEx log shows SkipDropshipCompany loading with the expected
   version.
4. Confirm no startup errors mention `SkipDropshipCompany`,
   `HarmonyPatchException`, missing dependencies, or package metadata issues.

### Default Config

1. Open the generated BepInEx config after first startup.
2. Confirm `General.Enabled` defaults to `true`.
3. Confirm `General.RequireReroutingOnFirstDay` defaults to `false`.
4. Confirm disabling `General.Enabled` prevents direct ship delivery.

### First-Day Orbit

1. Start a new save.
2. Stay in orbit on day one.
3. Buy a low-cost item from the terminal.
4. Confirm the item appears on the ship without waiting for the dropship.
5. Confirm terminal help or purchase state does not leave an item queued for the
   dropship.

### First-Day Orbit With Rerouting Required

1. Set `General.RequireReroutingOnFirstDay` to `true`.
2. Start a new save.
3. Buy an item before routing to the Company.
4. Confirm normal dropship behavior is preserved.
5. Route to the Company and buy another item.
6. Confirm direct ship delivery is enabled only after the reroute condition is
   met.

### Company Landing

1. Route to the Company.
2. Land on Gordion.
3. Buy a low-cost item.
4. Confirm direct ship delivery while landed on the Company.
5. Confirm repeated purchases avoid overlapping item placement enough for pickup.

### Moon Landing No-Op

1. Route to a normal moon.
2. Land on the moon.
3. Buy an item.
4. Confirm normal dropship behavior is preserved.

### Previous Company Landing

1. Land on the Company.
2. Take off.
3. While still routing to the Company, buy an item in orbit.
4. Confirm direct ship delivery.
5. Route to a normal moon and buy another item in orbit.
6. Confirm normal dropship behavior is preserved.

### Host And Client Assumptions

1. Validate host-only installation with at least one client connected when
   multiplayer coverage is available.
2. Let a client buy an item while the host has the mod installed.
3. Confirm the host-side direct delivery decision applies without requiring the
   client to install the mod.
4. Optionally install the mod on a client and confirm it does not change
   host-owned behavior.

## Structured Validation Logging Decision

Structured validation logging is not implemented in this migration stage.

Reason:

- SkipDropshipCompany currently has sparse, direct decision points and no
  existing validation-log parser or issue workflow that consumes JSONL records.
- The release candidate can be validated with the runbook above without adding
  runtime logging behavior before the first v0.2.0 port review.
- If repeated release validation exposes ambiguous failures, add an opt-in
  `Debug.ValidationLogging` config later with one JSON object per event line and
  a stable prefix such as `[SDC_VALIDATION]`.

Future structured log events should stay disabled by default and cover only:

- Plugin load and config state.
- Instant-purchase allow or deny decisions.
- Prepared purchase counts.
- Spawn success or failure counts.
- Landing-history changes.

They must not include local paths, player names, lobby identifiers, account
identifiers, tokens, or machine names.
