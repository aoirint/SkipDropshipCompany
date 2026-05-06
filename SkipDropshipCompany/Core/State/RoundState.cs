// SPDX-License-Identifier: MIT
#nullable enable

namespace SkipDropshipCompany.Core.State;

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
