using System; // Ќужен дл€ [Serializable]
using UnityEngine;

// ѕозвол€ет редактировать класс в инспекторе
[Serializable]
public class LevelConfig
{
    [Tooltip("ћинимальное количество Shape дл€ этого уровн€ сложности.")]
    public int minShapes = 3;

    [Tooltip("ћаксимальное количество Shape дл€ этого уровн€ сложности.")]
    public int maxShapes = 5;

    [Tooltip("Ѕазовое здоровье дл€ каждого Shape.")]
    public int baseShapeHealth = 10;

    [Tooltip("ћножитель дл€ здоровь€ Shape (здоровье = baseHealth * healthMultiplier).")]
    public float healthMultiplier = 1.0f;

    [Tooltip("—корость, с которой враг летит к игроку.")]
    public float enemySpeed = 10f;

    [Tooltip("»нтервал между спавнами на этом уровне.")]
    public float spawnInterval = 1.5f;
}