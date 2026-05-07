#nullable enable

using System.Collections.Generic;
using System.Linq;
using SkipDropshipCompany.Core.Ports;

namespace SkipDropshipCompany.Core.State;

/// <summary>
/// One-entry rolling history of the latest landed scene.
/// </summary>
/// <remarks>
/// Only the most recent landing matters for the company reroute rule.
/// </remarks>
internal sealed class LandingHistoryStore
{
    private const int LandingHistorySize = 1;
    private readonly IPluginLogger logger;
    private List<string> landingEntries = [];

    public LandingHistoryStore(IPluginLogger logger)
    {
        this.logger = logger;
    }

    public bool AddLandingHistory(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            logger.LogError("Scene name is null or empty. Cannot add to landing history.");
            return false;
        }

        landingEntries.Add(sceneName);
        landingEntries = landingEntries.TakeLast(LandingHistorySize).ToList();

        logger.LogDebug($"Updated landing history. landingEntries={string.Join(", ", landingEntries)}");
        return true;
    }

    public bool IsLastLandedOnCompany()
    {
        var lastLandedSceneName = landingEntries.LastOrDefault();
        if (lastLandedSceneName == null)
        {
            logger.LogDebug("Last landed scene name is null.");
            return false;
        }

        if (!CompanyScene.IsCompanyScene(lastLandedSceneName))
        {
            logger.LogDebug("Last landed scene is not company.");
            return false;
        }

        return true;
    }

    public void ClearLandingHistory()
    {
        landingEntries.Clear();
    }
}
