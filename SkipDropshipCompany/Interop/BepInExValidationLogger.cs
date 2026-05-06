// SPDX-License-Identifier: MIT
#nullable enable

using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using SkipDropshipCompany.Core.Ports;
using SkipDropshipCompany.Core.Validation;

namespace SkipDropshipCompany.Interop;

/// <summary>
/// Writes structured validation events into the normal BepInEx log stream.
/// </summary>
internal sealed class BepInExValidationLogger : IValidationLogger
{
    private const int SchemaVersion = 1;
    private const string Prefix = "[SDC_VALIDATION] ";

    private readonly IPluginLogger logger;
    private readonly string runId;
    private int sequence;

    public BepInExValidationLogger(IPluginLogger logger, DateTime startupTimeUtc)
    {
        this.logger = logger;
        runId = CreateRunId(startupTimeUtc);
    }

    /// <summary>
    /// Records one validation event as a prefixed, line-oriented JSON payload.
    /// </summary>
    public void Record(ValidationLogRecord record)
    {
        // Validation logs are line-oriented JSON embedded in the normal BepInEx
        // log stream. Keep the envelope stable so external scripts can filter
        // by prefix, then compare schema/run/seq/event across mod versions.
        var payload = new Dictionary<string, object?>
        {
            ["schema"] = SchemaVersion,
            ["ts"] = FormatTimestamp(DateTime.UtcNow),
            ["run"] = runId,
            ["seq"] = ++sequence,
            ["event"] = record.EventName
        };

        if (record.Fields != null)
        {
            foreach (var field in record.Fields)
            {
                payload[field.Key] = field.Value;
            }
        }

        logger.LogInfo(Prefix + JsonConvert.SerializeObject(payload, Formatting.None));
    }

    private static string CreateRunId(DateTime startupTimeUtc)
    {
        // The timestamp makes a run readable in raw logs, while the short
        // random suffix keeps two launches in the same second distinct.
        var timestamp = startupTimeUtc.ToString("yyyyMMdd'T'HHmmss'Z'", CultureInfo.InvariantCulture);
        var suffix = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture).Substring(0, 6);
        return timestamp + "-" + suffix;
    }

    private static string FormatTimestamp(DateTime timestampUtc)
    {
        return timestampUtc.ToUniversalTime().ToString(
            "yyyy-MM-dd'T'HH:mm:ss.fff'Z'",
            CultureInfo.InvariantCulture
        );
    }
}
