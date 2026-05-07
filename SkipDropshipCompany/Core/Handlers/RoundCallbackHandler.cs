#nullable enable

using SkipDropshipCompany.Core.Ports;
using SkipDropshipCompany.Core.UseCases;

namespace SkipDropshipCompany.Core.Handlers;

/// <summary>
/// Coordinates round lifecycle callbacks with landing-history use cases.
/// </summary>
/// <remarks>
/// Interop detects base-game timing, while Core snapshots the current game
/// state and updates landing history.
/// </remarks>
internal sealed class RoundCallbackHandler
{
    private readonly IGameInterop gameInterop;
    private readonly RecordLandingUseCase recordLandingUseCase;
    private readonly ClearLandingHistoryUseCase clearLandingHistoryUseCase;

    public RoundCallbackHandler(
        IGameInterop gameInterop,
        RecordLandingUseCase recordLandingUseCase,
        ClearLandingHistoryUseCase clearLandingHistoryUseCase
    )
    {
        this.gameInterop = gameInterop;
        this.recordLandingUseCase = recordLandingUseCase;
        this.clearLandingHistoryUseCase = clearLandingHistoryUseCase;
    }

    public void HandleStartGame()
    {
        recordLandingUseCase.Execute(sceneName: gameInterop.GetCurrentLevelSceneName());
    }

    public void HandleResetShip()
    {
        clearLandingHistoryUseCase.Execute();
    }
}
