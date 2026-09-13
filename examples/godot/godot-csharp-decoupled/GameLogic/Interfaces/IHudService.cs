using GameLogic.Models;

namespace GameLogic.Interfaces;

public interface IHudService
{
    HudMessage CurrentMessage { get; set; }
    int Score { get; set; }
    bool StartButtonVisible { get; set; }
    void ShowMessage(HudMessage message);
    void ShowGameOver();
    void UpdateScore(int score);
    void ResetForNewGame();
}
