using Xunit;
using GameLogic.Services;
using GameLogic.Models;

namespace GameLogic.Tests;

public class HudServiceTests
{
    [Fact]
    public void ShowMessage_ShouldSetCurrentMessage()
    {
        // Arrange
        var service = new HudService();

        // Act
        service.ShowMessage(HudMessage.GetReady);

        // Assert
        Assert.Equal(HudMessage.GetReady, service.CurrentMessage);
    }

    [Fact]
    public void ShowGameOver_ShouldSetMessageAndHideButton()
    {
        // Arrange
        var service = new HudService();

        // Act
        service.ShowGameOver();

        // Assert
        Assert.Equal(HudMessage.GameOver, service.CurrentMessage);
        Assert.False(service.StartButtonVisible);
    }

    [Fact]
    public void UpdateScore_ShouldSetScore()
    {
        // Arrange
        var service = new HudService();

        // Act
        service.UpdateScore(42);

        // Assert
        Assert.Equal(42, service.Score);
    }

    [Fact]
    public void ResetForNewGame_ShouldResetState()
    {
        // Arrange
        var service = new HudService();
        service.UpdateScore(50);
        service.ShowMessage(HudMessage.GameOver);
        service.StartButtonVisible = false;

        // Act
        service.ResetForNewGame();

        // Assert
        Assert.Equal(0, service.Score);
        Assert.Equal(HudMessage.GetReady, service.CurrentMessage);
        Assert.False(service.StartButtonVisible);
    }

    [Theory]
    [InlineData(HudMessage.GetReady, "Get Ready!")]
    [InlineData(HudMessage.GameOver, "Game Over")]
    [InlineData(HudMessage.Title, "Dodge the Creeps!")]
    [InlineData(HudMessage.None, "")]
    public void GetDisplayText_ShouldReturnCorrectText(HudMessage message, string expected)
    {
        var result = HudService.GetDisplayText(message);
        Assert.Equal(expected, result);
    }
}