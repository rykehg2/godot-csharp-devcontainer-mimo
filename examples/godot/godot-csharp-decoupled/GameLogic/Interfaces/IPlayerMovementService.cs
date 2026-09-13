using System.Numerics;

namespace GameLogic.Interfaces;

public enum PlayerAnimation
{
    Idle,
    Walk,
    Up
}

public interface IPlayerMovementService
{
    Vector2 CalculateVelocity(Vector2 direction, float speed);
    Vector2 ClampPosition(Vector2 position, Vector2 screenSize);
    PlayerAnimation GetAnimationState(Vector2 velocity);
    bool GetFlipH(Vector2 velocity);
    bool GetFlipV(Vector2 velocity);
}