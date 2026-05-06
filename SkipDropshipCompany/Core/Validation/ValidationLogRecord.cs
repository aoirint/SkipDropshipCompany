// SPDX-License-Identifier: MIT
#nullable enable

using System.Collections.Generic;
using SkipDropshipCompany.Core.State;
using SkipDropshipCompany.Core.UseCases;

namespace SkipDropshipCompany.Core.Validation;

internal enum ValidationLogRole
{
    Server,
    Client
}

internal enum ValidationLogScene
{
    Company,
    Other,
    Unknown
}

internal enum ValidationLogPrepareResult
{
    NoServer,
    NotAllowed,
    Success
}

internal enum ValidationLogSpawnResult
{
    NoServer,
    NoPreparedPurchase,
    SpawnFailed,
    Success
}

internal enum ValidationLogLandingHistoryResult
{
    NoServer,
    NullScene,
    EmptyScene,
    Success
}

internal enum ValidationLogTerminalOrderReadResult
{
    NullOrderedItems
}

internal enum ValidationLogTerminalOrderRestoreResult
{
    Failed,
    Success
}

/// <summary>
/// Immutable validation event description with stable event names and fields.
/// </summary>
internal sealed class ValidationLogRecord
{
    // Call sites choose semantic events through named factories; this type owns
    // the stable field names and token spelling.
    private ValidationLogRecord(string eventName, Dictionary<string, object?>? fields = null)
    {
        EventName = eventName;
        Fields = fields;
    }

    public string EventName { get; }

    public Dictionary<string, object?>? Fields { get; }

    public static ValidationLogRecord PluginLoaded(
        string version,
        bool validationLogging,
        bool enabled,
        bool requireReroutingOnFirstDay
    )
    {
        return new(
            "plugin_loaded",
            new()
            {
                ["version"] = version,
                ["validation_logging"] = validationLogging,
                ["enabled"] = enabled,
                ["require_rerouting_on_first_day"] = requireReroutingOnFirstDay
            }
        );
    }

    public static ValidationLogRecord ControllerCreated()
    {
        return new("controller_created");
    }

    public static ValidationLogRecord CallbackException(string callback, string exceptionType)
    {
        return new(
            "callback_exception",
            new()
            {
                ["callback"] = callback,
                ["exception_type"] = exceptionType
            }
        );
    }

    public static ValidationLogRecord InstantPurchaseEligibilityDecision(
        RoundState roundState,
        bool enabled,
        bool requireReroutingOnFirstDay,
        bool lastLandedOnCompany,
        bool allowed,
        string reason
    )
    {
        return new(
            "instant_purchase_eligibility_decision",
            new()
            {
                ["enabled"] = enabled,
                ["require_rerouting_on_first_day"] = requireReroutingOnFirstDay,
                ["is_in_orbit"] = roundState.IsInOrbit,
                ["is_first_day"] = roundState.IsFirstDay,
                ["is_routing_to_company"] = roundState.IsRoutingToCompany,
                ["last_landed_on_company"] = lastLandedOnCompany,
                ["allowed"] = allowed,
                ["reason"] = reason
            }
        );
    }

    public static ValidationLogRecord InstantPurchaseEligibilityConfigDisabled()
    {
        return new(
            "instant_purchase_eligibility_decision",
            new()
            {
                ["enabled"] = false,
                ["allowed"] = false,
                ["reason"] = "disabled"
            }
        );
    }

    public static ValidationLogRecord PrepareInstantPurchaseResult(
        ValidationLogRole role,
        ValidationLogPrepareResult result,
        int originalItemCount,
        PrepareInstantPurchaseResult? preparedResult
    )
    {
        return new(
            "prepare_instant_purchase_result",
            new()
            {
                ["role"] = ToValidationRoleToken(role),
                ["result"] = ToValidationPrepareResultToken(result),
                ["original_item_count"] = originalItemCount,
                ["dropship_item_count"] = preparedResult?.DropShipBoughtItemIndexes.Count,
                ["instant_item_count"] = preparedResult?.InstantBoughtItemIndexes.Count
            }
        );
    }

    public static ValidationLogRecord SpawnInstantPurchaseResult(
        ValidationLogRole role,
        ValidationLogSpawnResult result,
        int preparedInstantItemCount,
        int preparedDropShipItemCount,
        int spawnedItemCount
    )
    {
        return new(
            "spawn_instant_purchase_result",
            new()
            {
                ["role"] = ToValidationRoleToken(role),
                ["result"] = ToValidationSpawnResultToken(result),
                ["prepared_instant_item_count"] = preparedInstantItemCount,
                ["prepared_dropship_item_count"] = preparedDropShipItemCount,
                ["spawned_item_count"] = spawnedItemCount
            }
        );
    }

    public static ValidationLogRecord LandingHistoryUpdated(
        ValidationLogRole role,
        ValidationLogLandingHistoryResult result,
        ValidationLogScene scene
    )
    {
        return new(
            "landing_history_updated",
            new()
            {
                ["role"] = ToValidationRoleToken(role),
                ["result"] = ToValidationLandingHistoryResultToken(result),
                ["scene"] = ToValidationSceneToken(scene)
            }
        );
    }

    public static ValidationLogRecord LandingHistoryCleared(ValidationLogRole role, bool cleared)
    {
        return new(
            "landing_history_cleared",
            new()
            {
                ["role"] = ToValidationRoleToken(role),
                ["cleared"] = cleared
            }
        );
    }

    public static ValidationLogRecord TerminalOrderReadResult(
        ValidationLogRole role,
        ValidationLogTerminalOrderReadResult result
    )
    {
        return new(
            "terminal_order_read_result",
            new()
            {
                ["role"] = ToValidationRoleToken(role),
                ["result"] = ToValidationTerminalOrderReadResultToken(result)
            }
        );
    }

    public static ValidationLogRecord TerminalOrderRestoreResult(
        ValidationLogRole role,
        ValidationLogTerminalOrderRestoreResult result,
        int dropshipItemCount
    )
    {
        return new(
            "terminal_order_restore_result",
            new()
            {
                ["role"] = ToValidationRoleToken(role),
                ["result"] = ToValidationTerminalOrderRestoreResultToken(result),
                ["dropship_item_count"] = dropshipItemCount
            }
        );
    }

    public static ValidationLogScene ToValidationScene(string? sceneName)
    {
        // Scene names come from the base game, but validation logs keep only
        // closed scene categories so runs can be compared across environments.
        if (sceneName == null)
        {
            return ValidationLogScene.Unknown;
        }

        return CompanyScene.IsCompanyScene(sceneName)
            ? ValidationLogScene.Company
            : ValidationLogScene.Other;
    }

    private static string ToValidationRoleToken(ValidationLogRole role)
    {
        // Unknown enum values fall back to schema-safe tokens instead of
        // leaking numeric values into validation output.
        return role switch
        {
            ValidationLogRole.Server => "server",
            ValidationLogRole.Client => "client",
            _ => "unknown"
        };
    }

    private static string ToValidationSceneToken(ValidationLogScene scene)
    {
        return scene switch
        {
            ValidationLogScene.Company => "company",
            ValidationLogScene.Other => "other",
            ValidationLogScene.Unknown => "unknown",
            _ => "unknown"
        };
    }

    private static string ToValidationPrepareResultToken(ValidationLogPrepareResult result)
    {
        return result switch
        {
            ValidationLogPrepareResult.NoServer => "no_server",
            ValidationLogPrepareResult.NotAllowed => "not_allowed",
            ValidationLogPrepareResult.Success => "success",
            _ => "unknown"
        };
    }

    private static string ToValidationSpawnResultToken(ValidationLogSpawnResult result)
    {
        return result switch
        {
            ValidationLogSpawnResult.NoServer => "no_server",
            ValidationLogSpawnResult.NoPreparedPurchase => "no_prepared_purchase",
            ValidationLogSpawnResult.SpawnFailed => "spawn_failed",
            ValidationLogSpawnResult.Success => "success",
            _ => "unknown"
        };
    }

    private static string ToValidationLandingHistoryResultToken(
        ValidationLogLandingHistoryResult result
    )
    {
        return result switch
        {
            ValidationLogLandingHistoryResult.NoServer => "no_server",
            ValidationLogLandingHistoryResult.NullScene => "null_scene",
            ValidationLogLandingHistoryResult.EmptyScene => "empty_scene",
            ValidationLogLandingHistoryResult.Success => "success",
            _ => "unknown"
        };
    }

    private static string ToValidationTerminalOrderReadResultToken(
        ValidationLogTerminalOrderReadResult result
    )
    {
        return result switch
        {
            ValidationLogTerminalOrderReadResult.NullOrderedItems => "null_ordered_items",
            _ => "unknown"
        };
    }

    private static string ToValidationTerminalOrderRestoreResultToken(
        ValidationLogTerminalOrderRestoreResult result
    )
    {
        return result switch
        {
            ValidationLogTerminalOrderRestoreResult.Failed => "failed",
            ValidationLogTerminalOrderRestoreResult.Success => "success",
            _ => "unknown"
        };
    }
}
