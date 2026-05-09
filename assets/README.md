# SkipDropshipCompany

A Lethal Company mod that skips the dropship for item purchases on the Company
and in some limited situations.

## Compatibility

- Lethal Company v81.5 (2026-04-17 UTC, Manifest ID:
  `6423525044216269478`)
    - Test environment
        - [BepInExPack][bepinexpack-package] v5.4.2305 (2026-03-17 UTC)

## What it does

- Direct Ship Delivery: Purchased items are delivered directly to your ship instead of being delivered by dropship.

## When it activates

Direct ship delivery feature activates when any of these conditions are met:

- While landed on the Company
- In orbit on the first day (also the day after ejected)
- In orbit on the next day after landing on the Company, still routing to the Company (in the same session only)

You can still use the dropship on the other moons as usual.

Cruiser still uses the dropship for delivery.

## Who needs to install

Host only. Clients do not need to install this mod, but it's safe to install on clients as it will have no effect.

If clients (including non-modded) purchase items, the direct ship delivery feature will also work for them.

## Where the items spawn

Purchased items will spawn inside your ship, near the default item position for the out-of-bounds items.

Each item type will be placed with a small offset to avoid overlapping.

## Configuration

| Name | Type | Default | Description |
| :------ | :--- | :------ | :---------- |
| Enabled | bool | true | Set to false to disable this mod. |
| RequireReroutingOnFirstDay | bool | false | If true, rerouting to the company will be required to skip the dropship on the first day. |

## FAQ

### Can't use the dropship to attract enemies on eclipse day?

You can still use it.

- Purchase items in orbit after rerouting to a moon
- Purchase items after starting landing on a moon

The dropship will arrive as usual on that moon in these cases.

You can change the first day behavior in configuration if your first day is
eclipsed or you are using a mod like
[stormytuna/EclipseOnly][eclipse-only-package].

### Already purchased items before the conditions are met. Will they be delivered directly to the ship?

No. Only items purchased after the conditions are met will be delivered directly to the ship.

However, if you purchase additional items before the dropship arrives, they will
be delivered together with the previous ones to the ship.

This behavior may be changed in the future version since it depends on the base game implementation.

### Is it compatible with FasterItemDropship/LC_FasterRocket/HQoL?

At least the following versions are assumed to be compatible.

- [FlipMods/FasterItemDropship][faster-item-dropship-package] v1.3.1
- [zoomstv/FasterRocket][faster-rocket-package] v1.0.1
- [HQHQTeam/HQoL][hqol-package] v1.0.14

### Does it work for the legacy versions of Lethal Company?

No test has been done for them. It is recommended to use the intended Lethal
Company v81.5 baseline above.

## Differences from [Nexor/InstantBuy][instant-buy-package]

`Nexor/InstantBuy` works only on Company moons by default since v0.0.8 (2026-01-24 UTC).
It can be configured to run on other moons by disabling its `Company Only` option.

This mod has no configuration except for the first-day option to change that
behavior.

`Nexor/InstantBuy` also supports some modded Company moons while `Company Only` is enabled.
`Nexor/InstantBuy` does not document dedicated limited-orbit support.

This mod only supports the vanilla Company moon (Gordion) and limited orbit
cases currently.

`Nexor/InstantBuy` can be configured to exclude specified items from instant purchases.

This mod does not have such a configuration option yet.

## Differences from [befuddled_productions/Quick_Buy_Menu][quick-buy-menu-package]

`befuddled_productions/Quick_Buy_Menu` adds some chat commands to purchase items in a faster way.

This mod uses the terminal as usual to purchase items.

`befuddled_productions/Quick_Buy_Menu` stores the purchased items into the player's inventory directly.

This mod stores the purchased items on the ship directly.

## Differences from [FlipMods/FasterItemDropship][faster-item-dropship-package]

`FlipMods/FasterItemDropship` reduces the waiting time for the dropship to arrive after purchasing items.

This mod just spawns the purchased items directly on your ship instead of the dropship delivery.

## Differences from [zoomstv/FasterRocket][faster-rocket-package]

`zoomstv/FasterRocket` reduces the waiting time for the dropship to arrive after purchasing items.

This mod just spawns the purchased items directly on your ship instead of the dropship delivery.

## Differences from [HQHQTeam/HQoL][hqol-package]

`HQHQTeam/HQoL` is a collection of High Quota quality-of-life changes.

Its faster company dropship feature reduces the waiting time for the dropship to
arrive after item purchases or Cruiser orders.

This mod just spawns the purchased items directly on your ship instead of the dropship delivery.

## Differences from [dozyote/EarlyDropship][early-dropship-package]

`dozyote/EarlyDropship` delivers items purchased in orbit before the ship lands.

This mod just spawns the purchased items directly on your ship instead of the dropship delivery.

## Credits

- Highly inspired by [Nexor/InstantBuy][instant-buy-package].

## AI Disclosure

Some parts of this project were developed with AI tools based on large language
models (LLMs), including agent-based tools.
The project maintainer reviews the code.
This disclosure is made in compliance with Thunderstore and community policies.

[bepinexpack-package]: https://thunderstore.io/c/lethal-company/p/BepInEx/BepInExPack/
[early-dropship-package]: https://thunderstore.io/c/lethal-company/p/dozyote/EarlyDropship/
[eclipse-only-package]: https://thunderstore.io/c/lethal-company/p/stormytuna/EclipseOnly/
[faster-item-dropship-package]: https://thunderstore.io/c/lethal-company/p/FlipMods/FasterItemDropship/
[faster-rocket-package]: https://thunderstore.io/c/lethal-company/p/zoomstv/FasterRocket/
[hqol-package]: https://thunderstore.io/c/lethal-company/p/HQHQTeam/HQoL/
[instant-buy-package]: https://thunderstore.io/c/lethal-company/p/Nexor/InstantBuy/
[quick-buy-menu-package]: https://thunderstore.io/c/lethal-company/p/befuddled_productions/Quick_Buy_Menu/
