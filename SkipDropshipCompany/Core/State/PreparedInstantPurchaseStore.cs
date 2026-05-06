// SPDX-License-Identifier: MIT
#nullable enable

using SkipDropshipCompany.Core.UseCases;

namespace SkipDropshipCompany.Core.State;

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
