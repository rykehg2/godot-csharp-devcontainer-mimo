using System.Numerics;
using GameLogic.Interfaces;

namespace GameLogic.Services;

public class PlayerMovementService : IPlayerMovementService
{
    public Vector2 CalculateVelocity(Vector2 direction, float speed)
    {
        if (direction.LengthSquared() > 0)
        {
            return Vector2.Normalize(direction) * speed;
        }
        return Vector2.Zero;
    }

    public Vector2 ClampPosition(Vector2 position, Vector2 screenSize)
    {
        return Vector2.Clamp(position, Vector2.Zero, screenSize);
    }

    public PlayerAnimation GetAnimationState(Vector2 velocity)
    {
        if (velocity.LengthSquared() == 0)
            return PlayerAnimation.Idle;
        if (velocity.X != 0)
            return PlayerAnimation.Walk;
        return PlayerAnimation.Up;
    }

    public bool GetFlipH(Vector2 velocity) => velocity.X < 0;
    public bool GetFlipV(Vector2 velocity) => velocity.Y > 0;
}