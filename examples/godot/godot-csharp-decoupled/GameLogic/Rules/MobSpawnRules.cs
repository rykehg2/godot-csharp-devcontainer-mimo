namespace GameLogic.Rules;

public static class MobSpawnRules
{
    // Aumenta a velocidade mínima e máxima do mob de acordo com o score
    public static (float MinSpeed, float MaxSpeed) GetMobSpeedRange(int score)
    {
        float baseMin = 150.0f;
        float baseMax = 250.0f;
        
        // Aumenta a velocidade em 5% por ponto marcado (dificuldade progressiva)
        float speedMultiplier = 1.0f + (score * 0.05f);
        
        return (baseMin * speedMultiplier, baseMax * speedMultiplier);
    }
}
