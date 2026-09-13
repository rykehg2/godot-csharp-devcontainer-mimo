using GameLogic.Models;

namespace GameLogic.Services;

public class GameSession
{
    private readonly GameState _state = new();
    
    public event Action<int>? OnScoreChanged;
    public event Action<GameStatus>? OnStatusChanged;

    public int CurrentScore => _state.Score;
    public GameStatus CurrentStatus => _state.Status;

    public void StartNewGame()
    {
        _state.Score = 0;
        _state.Status = GameStatus.Running;
        OnScoreChanged?.Invoke(_state.Score);
        OnStatusChanged?.Invoke(_state.Status);
    }

    public void IncrementScore()
    {
        if (_state.Status != GameStatus.Running) return;
        
        _state.Score++;
        OnScoreChanged?.Invoke(_state.Score);
    }

    public void GameOver()
    {
        _state.Status = GameStatus.GameOver;
        OnStatusChanged?.Invoke(_state.Status);
    }
}
