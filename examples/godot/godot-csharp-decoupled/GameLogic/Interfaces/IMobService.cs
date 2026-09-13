using System.Numerics;

namespace GameLogic.Interfaces;

public struct MobSpawnData
{
    public Vector2 Position;
    public float Rotation;
    public Vector2 LinearVelocity;
}

public interface IMobService
{
    MobSpawnData CreateSpawnData(float spawnRotation, int currentScore, float angleOffset, Random random);
}