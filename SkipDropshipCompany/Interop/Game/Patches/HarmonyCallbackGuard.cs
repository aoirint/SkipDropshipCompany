#nullable enable

using System;

namespace SkipDropshipCompany.Interop.Game.Patches;

internal static class HarmonyCallbackGuard
{
    private static HarmonyCallbackDiagnosticReporter? diagnosticReporter;

    /// <summary>
    /// Configures diagnostics before Harmony patches are installed.
    /// </summary>
    public static void Configure(HarmonyCallbackDiagnosticReporter reporter)
    {
        diagnosticReporter = reporter;
    }

    /// <summary>
    /// Runs a patch notification and records diagnostics if the callback throws.
    /// </summary>
    /// <returns>
    /// Whether the controller notification completed without throwing.
    /// </returns>
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

    /// <summary>
    /// Runs a patch notification that returns a value needed for a patch side effect.
    /// </summary>
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
