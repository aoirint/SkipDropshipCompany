// SPDX-License-Identifier: MIT
#nullable enable

namespace SkipDropshipCompany.Interop.Game.Patches;

internal static class HarmonyCallbackTokens
{
    // Tokens are stable validation identifiers, not display text. Use
    // class_method_patchkind naming so SDC and CJP diagnostics stay comparable.
    public const string TerminalSyncGroupCreditsClientRpcPrefix =
        "terminal_sync_group_credits_client_rpc_prefix";
    public const string TerminalSyncGroupCreditsClientRpcPostfix =
        "terminal_sync_group_credits_client_rpc_postfix";
    public const string StartOfRoundStartGamePostfix = "start_of_round_start_game_postfix";
    public const string StartOfRoundResetShipPostfix = "start_of_round_reset_ship_postfix";
}
