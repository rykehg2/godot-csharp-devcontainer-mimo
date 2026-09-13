using GameLogic.Interfaces;
using GameLogic.Models;

namespace GameLogic.Services;

public class HudService : IHudService
{
    public HudMessage CurrentMessage { get; set; } = HudMessage.Title;
    public int Score { get; set; }
    public bool StartButtonVisible { get; set; } = true;

    public void ShowMessage(HudMessage message)
    {
        CurrentMessage = message;
    }

    public void ShowGameOver()
    {
        CurrentMessage = HudMessage.GameOver;
        StartButtonVisible = false;
    }

    public void UpdateScore(int score)
    {
        Score = score;
    }

    /// <summary>
    /// Resets the HUD state for a new game.
    /// </summary>
    public void ResetForNewGame()
    {
        Score = 0;
        CurrentMessage = HudMessage.GetReady;
        StartButtonVisible = false;
    }

    /// <summary>
    /// Maps a HudMessage enum to the display text shown on screen.
    /// </summary>
    public static string GetDisplayText(HudMessage message) => message switch
    {
        HudMessage.GetReady => "Get Ready!",
        HudMessage.GameOver => "Game Over",
        HudMessage.Title => "Dodge the Creeps!",
        _ => ""
    };
}