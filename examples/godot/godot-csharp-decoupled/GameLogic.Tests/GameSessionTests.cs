using Xunit;
using GameLogic.Services;
using GameLogic.Models;

namespace GameLogic.Tests;

public class GameSessionTests
{
    [Fact]
    public void StartNewGame_ShouldResetScoreAndSetStatusToRunning()
    {
        // Arrange
        var session = new GameSession();

        // Act
        session.StartNewGame();

        // Assert
        Assert.Equal(0, session.CurrentScore);
        Assert.Equal(GameStatus.Running, session.CurrentStatus);
    }

    [Fact]
    public void StartNewGame_ShouldTriggerEvents()
    {
        // Arrange
        var session = new GameSession();
        int? triggeredScore = null;
        GameStatus? triggeredStatus = null;

        session.OnScoreChanged += (score) => triggeredScore = score;
        session.OnStatusChanged += (status) => triggeredStatus = status;

        // Act
        session.StartNewGame();

        // Assert
        Assert.Equal(0, triggeredScore);
        Assert.Equal(GameStatus.Running, triggeredStatus);
    }

    [Fact]
    public void IncrementScore_WhenRunning_ShouldIncreaseScoreAndTriggerEvent()
    {
        // Arrange
        var session = new GameSession();
        session.StartNewGame();
        int? triggeredScore = null;
        session.OnScoreChanged += (score) => triggeredScore = score;

        // Act
        session.IncrementScore();

        // Assert
        Assert.Equal(1, session.CurrentScore);
        Assert.Equal(1, triggeredScore);
    }

    [Fact]
    public void IncrementScore_WhenNotRunning_ShouldNotIncreaseScore()
    {
        // Arrange
        var session = new GameSession(); // Starts in Ready status
        
        // Act
        session.IncrementScore();

        // Assert
        Assert.Equal(0, session.CurrentScore);
        Assert.Equal(GameStatus.Ready, session.CurrentStatus);
    }

    [Fact]
    public void GameOver_ShouldSetStatusToGameOverAndTriggerEvent()
    {
        // Arrange
        var session = new GameSession();
        session.StartNewGame();
        GameStatus? triggeredStatus = null;
        session.OnStatusChanged += (status) => triggeredStatus = status;

        // Act
        session.GameOver();

        // Assert
        Assert.Equal(GameStatus.GameOver, session.CurrentStatus);
        Assert.Equal(GameStatus.GameOver, triggeredStatus);
    }
}
