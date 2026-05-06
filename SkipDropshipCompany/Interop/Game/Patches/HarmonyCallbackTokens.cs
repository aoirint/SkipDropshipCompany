// SPDX-License-Identifier: MIT
#nullable enable

namespace SkipDropshipCompany.Interop.Game.Patches;

internal static class HarmonyCallbackTokens
{
    public const string TerminalSyncGroupCreditsClientRpcPrefix =
        "terminal_sync_group_credits_prefix";
    public const string TerminalSyncGroupCreditsClientRpcPostfix =
        "terminal_sync_group_credits_postfix";
    public const string StartOfRoundStartGamePostfix = "start_game_postfix";
    public const string StartOfRoundResetShipPostfix = "reset_ship_postfix";
}
