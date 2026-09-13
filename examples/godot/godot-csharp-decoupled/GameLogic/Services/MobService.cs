using System.Numerics;
using GameLogic.Interfaces;
using GameLogic.Rules;

namespace GameLogic.Services;

public class MobService : IMobService
{
    public MobSpawnData CreateSpawnData(float spawnRotation, int currentScore, float angleOffset, Random random)
    {
        float direction = spawnRotation + MathF.PI / 2 + angleOffset;

        var (minSpeed, maxSpeed) = MobSpawnRules.GetMobSpeedRange(currentScore);

        // Random speed within the range
        float speed = minSpeed + (float)random.NextDouble() * (maxSpeed - minSpeed);

        var velocity = new Vector2(speed, 0);
        var rotatedVelocity = RotateVector2(velocity, direction);

        return new MobSpawnData
        {
            Position = Vector2.Zero, // Caller sets position from PathFollow2D
            Rotation = direction,
            LinearVelocity = rotatedVelocity
        };
    }

    public static Vector2 RotateVector2(Vector2 vector, float radians)
    {
        float cos = MathF.Cos(radians);
        float sin = MathF.Sin(radians);
        return new Vector2(
            vector.X * cos - vector.Y * sin,
            vector.X * sin + vector.Y * cos
        );
    }
}