# Guest Feature Authorization and Global Gates

Use this reference when a BepInEx mod exposes a practice, debug, cheat-like,
or other feature that a non-host player could use to materially change shared
gameplay, game balance, progression, challenge, or another player's session.

## Contents

- [1. Classify the feature](#1-classify-the-feature)
- [2. Set a practical boundary for the control](#2-set-a-practical-boundary-for-the-control)
- [3. State the policy before choosing transport](#3-state-the-policy-before-choosing-transport)
- [4. Implement an authoritative authorization path](#4-implement-an-authoritative-authorization-path)
- [5. Provide a coherent global gate](#5-provide-a-coherent-global-gate)
- [6. Verify policy, not only RPC reachability](#6-verify-policy-not-only-rpc-reachability)

## 1. Classify the feature

Treat a feature as host-authorized by default when a guest can use it to:

- mutate shared game state, spawn or remove resources, bypass progression, or
  change movement, damage, inventory, physics, AI, objectives, or saves;
- reveal information that normally gates play or materially changes challenge;
- run a practice, rollback, rewind, teleport, debug, or test action that can
  affect a shared session; or
- trigger a server RPC or equivalent authoritative action whose effect is not
  purely local and cosmetic.

Do not use the feature's label as the classification. A feature called
"diagnostic" can still require authorization when its information or controls
materially change play. A purely local, cosmetic presentation that cannot affect
or reveal balance-relevant state may use a documented client-only policy.

When classification is uncertain, use host authorization until game evidence
shows the feature is local and non-material.

## 2. Set a practical boundary for the control

Host authorization is a consent and friction mechanism, not a security
boundary. It makes ordinary guest-side misuse less convenient and lets the
host explicitly accept balance-changing features in a shared session. It does
not prevent a user from modifying their local mod, bypassing its checks, or
using a different cheat. Do not represent it as anti-cheat, tamper resistance,
authentication, or a guarantee against abuse.

Use this limited model when its benefits fit the feature:

- deny by default so a guest does not silently impose a balance-changing tool
  on a host or other players;
- make the host's consent signal authoritative, so responsibility for allowing
  the capability is a host decision rather than an accidental guest-local
  toggle; and
- keep the protocol proportionate. A small explicit allow/deny exchange is
  usually sufficient; do not build identity, cryptographic attestation,
  mod-integrity, version-negotiation, or a general anti-cheat system unless the
  mod has a separate, evidenced requirement for one.

### Treat host installation as consent when it is safe to do so

Host installation of the same mod may itself be the consent signal when the
protected guest capability does not affect the host's security. In this model,
the host chose to install the mod and thereby accepts its declared ordinary
guest-facing practice or information feature, while an unmodded host does not
silently receive that behavior.

Make this choice only after recording why a guest's use cannot compromise host
security. In particular, it must not grant host process, file, configuration,
save, network, moderation, identity, privilege, or arbitrary-RPC authority; it
must not make the host execute an untrusted guest-controlled action; and it
must not weaken another independently protected trust boundary. Shared-game
balance effects alone do not require an additional host toggle when the host's
installation is an informed acceptance of that mod's declared scope.

For a capability that can affect host security, authority, or another trust
boundary, host presence is insufficient. Require an explicit host-controlled
allow/deny policy, and document why that stronger consent is needed.

State the accepted limitation in architecture documentation whenever this
model protects a materially balance-changing feature. The documented claim
should be no stronger than: normal mod clients require host consent; deliberately
modified or independently malicious clients are outside this mechanism's
protection.

## 3. State the policy before choosing transport

Document these facts in the mod architecture before implementing a handshake:

| Question | Required answer |
| --- | --- |
| Protected capability | Which guest action or information is gated? |
| Host consent signal | Is host installation sufficient, or which explicit host-controlled setting grants it? Why is that strength appropriate? |
| Default | Is guest use denied or allowed before authorization, and why? |
| Failure behavior | What happens on no host mod, no reply, deny, disconnect, or reset? |
| Lifetime | When does authorization expire and reset? |
| Scope | Which feature paths does the decision gate? |

For balance-changing guest features, use fail-closed behavior: no response,
denial, unavailable network state, or expired session authorization disables the
guest capability.

## 4. Implement an authoritative authorization path

Use an authoritative host-to-client path, normally a targeted response to a
guest request, when the game has no verified general mod-list or authorization
protocol.

- Validate the execution role before sending, receiving, or applying a request.
- Choose the response from the documented consent signal. When installation is
  sufficient under section 2, a host response confirming this mod's presence
  grants the capability. Otherwise, let the host read its current explicit
  policy and return an allow/deny value. Do not infer consent from a different
  mod's presence.
- Target the response to the requesting client unless every recipient needs the
  same result.
- Keep the client-side result connection-scoped. Clear it on disconnect,
  network despawn, lobby replacement, or another proven session boundary.
- Bound requests and retries. Do not send authorization traffic from a per-frame
  update loop. State the initial delay, retry interval, maximum attempts, and
  reason for each timing choice.
- Treat an unmodded host, an unknown RPC, a timeout, and a denied response as
  unavailable authorization. The guest feature must remain disabled without
  breaking normal game callbacks.
- Do not describe a presence handshake as authentication, anti-cheat, exact
  version negotiation, or a general mod-list unless separately implemented and
  verified.

Keep Unity, Netcode, BepInEx configuration, and RPC code in Interop. Pass Core
only the plain authorization result or an explicit policy value.

## 5. Provide a coherent global gate

Give the mod a global `Enabled` configuration setting by default. It controls
the mod's declared functionality; it does not unload the BepInEx plugin.

- Apply the gate before expensive observation, mutation, rendering, and network
  sends. Hide or clean up mod-owned presentation when disabling it.
- Keep lifecycle cleanup, error containment, and required passive callbacks
  safe while the gate is off.
- Combine the global gate with narrower feature settings rather than replacing
  them. A useful order is global enablement, host/guest authorization, then
  presentation or feature-specific controls.
- Describe settings by responsibility. For example, put plugin-wide operation
  and host guest-policy settings in `General`, and local presentation choices in
  an `Overlay` or feature-specific category when that matches the repository.

An exception is valid only when a coherent gate cannot be applied because of a
specific scale, lifecycle, or implementation constraint. Document the affected
paths, why a gate would be unsafe or misleading, and the narrower controls that
remain available. "This mod is large" alone is not enough.

## 6. Verify policy, not only RPC reachability

Cover the applicable rows below with the real Interop boundary or a faithful
harness. Use distinct values so an incorrect role, stale authorization, or
wrong recipient cannot pass by coincidence.

| Host state | Guest state | Expected result |
| --- | --- | --- |
| Mod absent or no response | Feature requested | Guest feature stays disabled. |
| Same mod present; installation is documented as sufficient consent | Feature requested | Only the documented guest capability enables. |
| Mod present, explicit host policy denies | Feature requested | Guest feature stays disabled. |
| Mod present, explicit host policy allows | Feature requested | Only the authorized guest capability enables. |
| Host policy changes or session ends | Previously allowed guest | Authorization resets at the documented boundary. |
| Global gate disabled | Host or guest | Declared mod functionality is disabled without plugin unload. |
| Guest lacks the mod | Shared session | Normal game callbacks and other clients remain safe. |

Also verify that requests are bounded, the documented host consent signal is
authoritative for unmodified clients, and editing only guest-local configuration
does not enable the protected path against an unmodified host.
