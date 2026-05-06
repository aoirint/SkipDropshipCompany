// SPDX-License-Identifier: MIT
#nullable enable

namespace SkipDropshipCompany.Core.State;

/// <summary>
/// Plain snapshot of the round state needed for instant-purchase eligibility.
/// </summary>
internal sealed class RoundState
{
    public RoundState(bool isInOrbit, bool isFirstDay, bool isRoutingToCompany)
    {
        IsInOrbit = isInOrbit;
        IsFirstDay = isFirstDay;
        IsRoutingToCompany = isRoutingToCompany;
    }

    public bool IsInOrbit { get; }

    public bool IsFirstDay { get; }

    public bool IsRoutingToCompany { get; }
}
