using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public event Action<EnemyData> OnEnemySpawned;

    [SerializeField] GameController controller;
    // --- Параметры для настройки в инспекторе ---

    [Header("Базовые Префабы")]
    [Tooltip("Префаб, используемый как 'заготовка' врага (должен содержать EnemyData).")]
    public GameObject baseEnemyPrefab;

    [Tooltip("Префаб одного Shape (должен содержать Shape и TextMeshPro).")]
    public GameObject baseShapePrefab;

    [Header("Настройки Сложности")]
    [Tooltip("Конфигурации для разных уровней сложности.")]
    public List<LevelConfig> levelConfigs;
    public int currentLevel = 0; // Текущий уровень (начинается с 0)

    [Header("Настройки Генерации")]
    [Tooltip("Позиция по Z, откуда фигуры начинают лететь (за горизонтом).")]
    public float spawnZPosition = 70f;

    [Tooltip("Диапазон генерации по X и Y (Y обычно 0).")]
    public Vector2 spawnRangeXY = new Vector2(13f, 5f);

    [Tooltip("Z-позиция, после которой враги уничтожаются.")]
    public float destroyLimitZ = -25f; // Позиция уничтожения по умолчанию

    // --- Управление ---

    public bool isSpawning = false;
    private float nextSpawnTime;

    // Свойство для получения текущей конфигурации уровня
    private LevelConfig CurrentConfig
    {
        get
        {
            // Возвращаем конфигурацию текущего уровня или последнего, если уровень превышен
            if (levelConfigs == null || levelConfigs.Count == 0) return null;
            if (currentLevel < levelConfigs.Count) return levelConfigs[currentLevel];
            return levelConfigs[levelConfigs.Count - 1];
        }
    }

    void Start()
    {
        // Проверка на корректность настроек
        if (baseEnemyPrefab == null || baseShapePrefab == null || levelConfigs == null || levelConfigs.Count == 0)
        {
            Debug.LogError("SpawnManager не настроен должным образом! Проверьте префабы и конфигурации уровней.");
            return;
        }

        // Установка первого интервала
        if (CurrentConfig != null)
        {
            nextSpawnTime = Time.time + CurrentConfig.spawnInterval;
        }

        //StartSpawning();
    }    

    void Update()
    {
        if (isSpawning && Time.time >= nextSpawnTime)
        {
            SpawnRandomEnemy();
            // Устанавливаем следующий таймер, используя текущий интервал сложности
            nextSpawnTime = Time.time + CurrentConfig.spawnInterval;
        }
    }

    // --- Публичные методы управления ---

    /// <summary>
    /// Запускает генерацию.
    /// </summary>
    public void StartSpawning()
    {
        isSpawning = true;
    }

    /// <summary>
    /// Останавливает генерацию.
    /// </summary>
    private void StopSpawning()
    {
        isSpawning = false;
    }

    /// <summary>
    /// Повышает уровень сложности.
    /// </summary>
    //public void IncreaseLevel()
    //{
    //    if (currentLevel < levelConfigs.Count - 1)
    //    {
    //        currentLevel++;
    //        Debug.Log($"Уровень сложности повышен до: {currentLevel + 1}. Интервал: {CurrentConfig.spawnInterval}с");
    //    }
    //    else
    //    {
    //        Debug.Log("Достигнут максимальный уровень сложности.");
    //    }
    //}

    // --- Логика генерации ---

    private void SpawnRandomEnemy()
    {
        LevelConfig config = CurrentConfig;
        if (config == null) return;

        float randomX = UnityEngine.Random.Range(-spawnRangeXY.x, spawnRangeXY.x);
        Vector3 spawnPosition = new Vector3(randomX, 0, spawnZPosition);
        GameObject newEnemyGO = Instantiate(baseEnemyPrefab, spawnPosition, Quaternion.identity);
        EnemyData enemyData = newEnemyGO.GetComponent<EnemyData>();

        if (enemyData == null)
        {
            Destroy(newEnemyGO);
            return;
        }

        CreateShapesInSquare(enemyData, config);
        enemyData.SetDestroyZPosition(destroyLimitZ);

        // 🔔 Уведомляем GameController, что враг создан
        OnEnemySpawned?.Invoke(enemyData);
    }


    private void CreateShapesInSquare(EnemyData enemyData, LevelConfig config)
    {
        // 1. Вычисляем параметры
        int minShapes = config.minShapes;
        int maxShapes = config.maxShapes;

        // Определяем общее количество Shape, которое нужно разместить
        int shapesToSpawn = UnityEngine.Random.Range(minShapes, maxShapes + 1);

        // Определяем размер сетки (сторона квадрата)
        int sideLength = Mathf.CeilToInt(Mathf.Sqrt(shapesToSpawn));

        // Здоровье для каждого Shape
        int totalHealth = Mathf.RoundToInt(config.baseShapeHealth * config.healthMultiplier);

        // Расстояние между центрами соседних Shape
        float spacing = 1.2f;

        // Начальная позиция для центрирования
        float startX = -(sideLength - 1) * spacing / 2f;
        float startY = -(sideLength - 1) * spacing / 2f;

        int shapeCounter = 0;

        // 2. Генерация и размещение в сетке
        for (int row = 0; row < sideLength; row++)
        {
            for (int col = 0; col < sideLength; col++)
            {
                if (shapeCounter >= shapesToSpawn)
                {
                    // Останавливаемся, если достигнуто необходимое количество Shape
                    return;
                }

                // Создаем Shape как дочерний объект
                GameObject newShapeGO = Instantiate(baseShapePrefab, enemyData.transform);
                Shape shapeComponent = newShapeGO.GetComponent<Shape>();

                // Вычисляем локальную позицию
                float localX = startX + col * spacing;
                float localZ = startY + row * spacing;

                // Устанавливаем локальную позицию (Y и X)
                newShapeGO.transform.localPosition = new Vector3(localX, 0, localZ);

                if (shapeComponent != null)
                {
                    shapeComponent.health = totalHealth;                
                }
                else
                {
                    Debug.LogError("Ошибка: Shape Prefab должен содержать Shape!");
                }

                shapeCounter++;
            }
        }
    }

    public void ClearAllEnemies()
    {
        StopSpawning();

        // 2. Находим все объекты, содержащие EnemyData (ваши основные вражеские контейнеры)
        EnemyData[] activeEnemies = FindObjectsOfType<EnemyData>();

        // 3. Уничтожаем каждый найденный объект
        foreach (EnemyData enemy in activeEnemies)
        {
            // Используем DestroyImmediate для немедленного удаления, 
            // или просто Destroy(enemy.gameObject) для удаления в конце кадра.
            // Для перехода между уровнями достаточно простого Destroy.
            Destroy(enemy.gameObject);
        }

        // Очистка всех пуль
        // Если пули имеют отдельный скрипт (например, Bullet), можно также очистить и их
        Bullet[] bullets = FindObjectsOfType<Bullet>();
        foreach (Bullet bullet in bullets)
        {
            Destroy(bullet.gameObject);
        }

        PlayersManagerSingletone.Instance.LocalPlayer.ShooterComponent.StopShooting();

        Debug.Log($"Уничтожено {activeEnemies.Length} врагов и {bullets.Length} пуль.");
    }
}