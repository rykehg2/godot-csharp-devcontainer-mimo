using System.Numerics;
using GameLogic.Services;
using GameLogic.Interfaces;
using Xunit;

namespace GameLogic.Tests;

public class MobServiceTests
{
    private readonly MobService _service = new();
    private static readonly Random _seed = new(42);

    [Fact]
    public void RotateVector2_WithZeroRadians_ShouldReturnSameVector()
    {
        var v = new Vector2(100, 0);
        var result = MobService.RotateVector2(v, 0);
        Assert.InRange(result.X, 99.9f, 100.1f);
        Assert.InRange(result.Y, -0.01f, 0.01f);
    }

    [Fact]
    public void RotateVector2_WithPiOver2_ShouldRotate90Degrees()
    {
        var v = new Vector2(100, 0);
        var result = MobService.RotateVector2(v, MathF.PI / 2);
        Assert.InRange(result.X, -0.01f, 0.01f);
        Assert.InRange(result.Y, 99.9f, 100.1f);
    }

    [Fact]
    public void CreateSpawnData_ShouldReturnNonZeroVelocity()
    {
        var data = _service.CreateSpawnData(0f, 0, 0f, _seed);
        Assert.True(data.LinearVelocity.Length() > 0);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(10)]
    public void CreateSpawnData_VelocityScalesWithScore(int score)
    {
        var dataLow = _service.CreateSpawnData(0f, 0, 0f, _seed);
        var dataHigh = _service.CreateSpawnData(0f, score, 0f, _seed);
        Assert.True(dataHigh.LinearVelocity.Length() >= dataLow.LinearVelocity.Length());
    }

    [Fact]
    public void CreateSpawnData_VelocityWithinExpectedRange()
    {
        var (min, max) = GameLogic.Rules.MobSpawnRules.GetMobSpeedRange(0);
        var data = _service.CreateSpawnData(0f, 0, 0f, _seed);
        var speed = data.LinearVelocity.Length();
        Assert.InRange(speed, min, max);
    }

    [Fact]
    public void CreateSpawnData_DifferentRandomSeeds_ProduceDifferentSpeeds()
    {
        var data1 = _service.CreateSpawnData(0f, 0, 0f, new Random(1));
        var data2 = _service.CreateSpawnData(0f, 0, 0f, new Random(2));
        var speed1 = data1.LinearVelocity.Length();
        var speed2 = data2.LinearVelocity.Length();

        // With different seeds and a range of speeds, they should differ
        // (very unlikely to be equal by chance)
        // We allow some tolerance for the edge case
        bool different = Math.Abs(speed1 - speed2) > 0.1f;
        Assert.True(different, "Different random seeds should produce different velocities");
    }

    [Fact]
    public void CreateSpawnData_AngleOffset_AffectsRotation()
    {
        var dataNoOffset = _service.CreateSpawnData(0f, 0, 0f, _seed);
        var dataWithOffset = _service.CreateSpawnData(0f, 0, MathF.PI / 4, _seed);

        // The velocities should have different directions
        float dot = Vector2.Dot(
            Vector2.Normalize(dataNoOffset.LinearVelocity),
            Vector2.Normalize(dataWithOffset.LinearVelocity)
        );
        // cos(PI/4) ≈ 0.707, so dot should be less than 1 indicating different directions
        Assert.True(dot < 0.9f, "Angle offset should change the velocity direction");
    }
}