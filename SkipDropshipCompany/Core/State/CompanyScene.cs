// SPDX-License-Identifier: MIT
#nullable enable

namespace SkipDropshipCompany.Core.State;

internal static class CompanyScene
{
    private const string CompanySceneName = "CompanyBuilding";

    public static bool IsCompanyScene(string sceneName)
    {
        return sceneName == CompanySceneName;
    }
}
