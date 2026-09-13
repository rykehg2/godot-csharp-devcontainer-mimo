using System.Numerics;
using GameLogic.Services;
using GameLogic.Interfaces;
using Xunit;

namespace GameLogic.Tests;

public class PlayerMovementServiceTests
{
    private readonly PlayerMovementService _service = new();

    [Fact]
    public void CalculateVelocity_WithNonZeroDirection_ShouldReturnNormalizedAndScaled()
    {
        var direction = new Vector2(1, 0);
        var result = _service.CalculateVelocity(direction, 400);
        Assert.Equal(new Vector2(400, 0), result);
    }

    [Fact]
    public void CalculateVelocity_WithDiagonalDirection_ShouldReturnNormalized()
    {
        var direction = new Vector2(1, 1);
        var result = _service.CalculateVelocity(direction, 400);
        var length = result.Length();
        Assert.InRange(length, 399.9f, 400.1f);
    }

    [Fact]
    public void CalculateVelocity_WithZeroDirection_ShouldReturnZero()
    {
        var direction = Vector2.Zero;
        var result = _service.CalculateVelocity(direction, 400);
        Assert.Equal(Vector2.Zero, result);
    }

    [Theory]
    [InlineData(50, 0, 100, 100)]
    [InlineData(0, 50, 100, 100)]
    [InlineData(-10, -10, 100, 100)]
    public void ClampPosition_ShouldClampToScreenBounds(float px, float py, float sw, float sh)
    {
        var pos = new Vector2(px, py);
        var screen = new Vector2(sw, sh);
        var result = _service.ClampPosition(pos, screen);
        Assert.True(result.X >= 0 && result.X <= sw);
        Assert.True(result.Y >= 0 && result.Y <= sh);
    }

    [Fact]
    public void GetAnimationState_WhenZero_ShouldReturnIdle()
    {
        Assert.Equal(PlayerAnimation.Idle, _service.GetAnimationState(Vector2.Zero));
    }

    [Fact]
    public void GetAnimationState_WhenHorizontal_ShouldReturnWalk()
    {
        Assert.Equal(PlayerAnimation.Walk, _service.GetAnimationState(new Vector2(10, 0)));
    }

    [Fact]
    public void GetAnimationState_WhenVertical_ShouldReturnUp()
    {
        Assert.Equal(PlayerAnimation.Up, _service.GetAnimationState(new Vector2(0, 10)));
    }

    [Theory]
    [InlineData(1, false)]
    [InlineData(-1, true)]
    public void GetFlipH_ShouldFlipWhenNegative(float x, bool expected)
    {
        Assert.Equal(expected, _service.GetFlipH(new Vector2(x, 0)));
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(-1, false)]
    public void GetFlipV_ShouldFlipWhenPositive(float y, bool expected)
    {
        Assert.Equal(expected, _service.GetFlipV(new Vector2(0, y)));
    }
}