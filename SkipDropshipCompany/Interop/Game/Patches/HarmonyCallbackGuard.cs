// SPDX-License-Identifier: MIT
#nullable enable

using System;

namespace SkipDropshipCompany.Interop.Game.Patches;

internal static class HarmonyCallbackGuard
{
    private static HarmonyCallbackDiagnosticReporter? diagnosticReporter;

    public static void Configure(HarmonyCallbackDiagnosticReporter reporter)
    {
        diagnosticReporter = reporter;
    }

    public static bool TryNotifyHarmonyCallback(string callback, Action notify)
    {
        try
        {
            notify();
            return true;
        }
        catch (Exception exception)
        {
            TryRecordCallbackException(callback: callback, exception: exception);
            return false;
        }
    }

    public static bool TryNotifyHarmonyCallback<T>(
        string callback,
        Func<T> notify,
        out T? result
    )
    {
        try
        {
            result = notify();
            return true;
        }
        catch (Exception exception)
        {
            result = default;
            TryRecordCallbackException(callback: callback, exception: exception);
            return false;
        }
    }

    private static void TryRecordCallbackException(string callback, Exception exception)
    {
        try
        {
            diagnosticReporter?.RecordCallbackException(callback: callback, exception: exception);
        }
        catch
        {
            // Diagnostics must not turn a fail-open Harmony callback into a base-game failure.
        }
    }
}
