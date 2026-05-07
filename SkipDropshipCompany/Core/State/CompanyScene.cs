#nullable enable

namespace SkipDropshipCompany.Core.State;

/// <summary>
/// Provides the stable base-game scene identity used by company-routing policy.
/// </summary>
internal static class CompanyScene
{
    private const string CompanySceneName = "CompanyBuilding";

    public static bool IsCompanyScene(string sceneName)
    {
        return sceneName == CompanySceneName;
    }
}
