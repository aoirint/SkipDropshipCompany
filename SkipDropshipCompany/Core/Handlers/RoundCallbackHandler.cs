// SPDX-License-Identifier: MIT
#nullable enable

using SkipDropshipCompany.Core.Ports;
using SkipDropshipCompany.Core.UseCases;

namespace SkipDropshipCompany.Core.Handlers;

// Round callbacks are coordination points. Interop detects base-game timing,
// while Core snapshots the current game state and updates landing history.
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
