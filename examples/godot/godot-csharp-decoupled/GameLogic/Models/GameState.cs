namespace GameLogic.Models;

public enum GameStatus
{
    Ready,
    Running,
    GameOver
}

public class GameState
{
    public int Score { get; set; }
    public GameStatus Status { get; set; } = GameStatus.Ready;
}
