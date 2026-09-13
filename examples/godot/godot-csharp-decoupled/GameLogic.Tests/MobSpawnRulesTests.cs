using Xunit;
using GameLogic.Rules;

namespace GameLogic.Tests;

public class MobSpawnRulesTests
{
    [Fact]
    public void GetMobSpeedRange_WithZeroScore_ShouldReturnBaseSpeeds()
    {
        // Act
        var (minSpeed, maxSpeed) = MobSpawnRules.GetMobSpeedRange(0);

        // Assert
        Assert.Equal(150.0f, minSpeed);
        Assert.Equal(250.0f, maxSpeed);
    }

    [Theory]
    [InlineData(10, 225.0f, 375.0f)]  // +50% speed increase (10 * 0.05)
    [InlineData(20, 300.0f, 500.0f)]  // +100% speed increase (20 * 0.05)
    public void GetMobSpeedRange_WithVariousScores_ShouldScaleSpeedProgressively(int score, float expectedMin, float expectedMax)
    {
        // Act
        var (minSpeed, maxSpeed) = MobSpawnRules.GetMobSpeedRange(score);

        // Assert
        Assert.Equal(expectedMin, minSpeed);
        Assert.Equal(expectedMax, maxSpeed);
    }
}
