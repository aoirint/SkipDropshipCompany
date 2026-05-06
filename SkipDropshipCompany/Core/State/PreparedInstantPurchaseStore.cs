// SPDX-License-Identifier: MIT
#nullable enable

using SkipDropshipCompany.Core.UseCases;

namespace SkipDropshipCompany.Core.State;

/// <summary>
/// Stores the Prefix-prepared purchase split until the matching Postfix consumes it.
/// </summary>
internal sealed class PreparedInstantPurchaseStore
{
    private PrepareInstantPurchaseResult? preparedInstantPurchaseResult;

    public PrepareInstantPurchaseResult? GetPreparedInstantPurchaseResult()
    {
        return preparedInstantPurchaseResult;
    }

    public void SetPreparedInstantPurchaseResult(PrepareInstantPurchaseResult result)
    {
        preparedInstantPurchaseResult = result;
    }

    public void ClearPreparedInstantPurchaseResult()
    {
        preparedInstantPurchaseResult = null;
    }
}
