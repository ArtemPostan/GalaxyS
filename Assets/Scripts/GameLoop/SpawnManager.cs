using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    // --- ��������� ��� ��������� � ���������� ---

    [Header("������� �������")]
    [Tooltip("������, ������������ ��� '���������' ����� (������ ��������� EnemyData).")]
    public GameObject baseEnemyPrefab;

    [Tooltip("������ ������ Shape (������ ��������� Shape � TextMeshPro).")]
    public GameObject baseShapePrefab;

    [Header("��������� ���������")]
    [Tooltip("������������ ��� ������ ������� ���������.")]
    public List<LevelConfig> levelConfigs;
    public int currentLevel = 0; // ������� ������� (���������� � 0)

    [Header("��������� ���������")]
    [Tooltip("������� �� Z, ������ ������ �������� ������ (�� ����������).")]
    public float spawnZPosition = 70f;

    [Tooltip("�������� ��������� �� X � Y (Y ������ 0).")]
    public Vector2 spawnRangeXY = new Vector2(13f, 5f);

    [Tooltip("Z-�������, ����� ������� ����� ������������.")]
    public float destroyLimitZ = -25f; // ������� ����������� �� ���������

    // --- ���������� ---

    private bool isSpawning = false;
    private float nextSpawnTime;

    // �������� ��� ��������� ������� ������������ ������
    private LevelConfig CurrentConfig
    {
        get
        {
            // ���������� ������������ �������� ������ ��� ����������, ���� ������� ��������
            if (levelConfigs == null || levelConfigs.Count == 0) return null;
            if (currentLevel < levelConfigs.Count) return levelConfigs[currentLevel];
            return levelConfigs[levelConfigs.Count - 1];
        }
    }

    void Start()
    {
        // �������� �� ������������ ��������
        if (baseEnemyPrefab == null || baseShapePrefab == null || levelConfigs == null || levelConfigs.Count == 0)
        {
            Debug.LogError("SpawnManager �� �������� ������� �������! ��������� ������� � ������������ �������.");
            return;
        }

        // ��������� ������� ���������
        if (CurrentConfig != null)
        {
            nextSpawnTime = Time.time + CurrentConfig.spawnInterval;
        }

        StartSpawning();
    }

    void Update()
    {
        if (isSpawning && Time.time >= nextSpawnTime)
        {
            SpawnRandomEnemy();
            // ������������� ��������� ������, ��������� ������� �������� ���������
            nextSpawnTime = Time.time + CurrentConfig.spawnInterval;
        }
    }

    // --- ��������� ������ ���������� ---

    /// <summary>
    /// ��������� ���������.
    /// </summary>
    public void StartSpawning()
    {
        isSpawning = true;
    }

    /// <summary>
    /// ������������� ���������.
    /// </summary>
    public void StopSpawning()
    {
        isSpawning = false;
    }

    /// <summary>
    /// �������� ������� ���������.
    /// </summary>
    public void IncreaseLevel()
    {
        if (currentLevel < levelConfigs.Count - 1)
        {
            currentLevel++;
            Debug.Log($"������� ��������� ������� ��: {currentLevel + 1}. ��������: {CurrentConfig.spawnInterval}�");
        }
        else
        {
            Debug.Log("��������� ������������ ������� ���������.");
        }
    }

    // --- ������ ��������� ---

    private void SpawnRandomEnemy()
    {
        LevelConfig config = CurrentConfig;
        if (config == null) return;

        // 1. ������� EnemyData (������ ���������������� �������� �������)
        float randomX = Random.Range(-spawnRangeXY.x, spawnRangeXY.x);
        Vector3 spawnPosition = new Vector3(randomX, 0, spawnZPosition);
        GameObject newEnemyGO = Instantiate(baseEnemyPrefab, spawnPosition, Quaternion.identity);
        EnemyData enemyData = newEnemyGO.GetComponent<EnemyData>();

        if (enemyData == null)
        {
            Destroy(newEnemyGO);
            return;
        }

        // 2. �������� ����� ����� ��� �������� ������
        CreateShapesInSquare(enemyData, config);

        // 3. ����������� �������� � ����� �����������
        // enemyData.speed = config.enemySpeed;
        enemyData.SetDestroyZPosition(destroyLimitZ);
    }

    // 5. ����������� �������� � ����� ����������� � EnemyData
    // ����������: ���������, ��� � EnemyData ���� ����� SetDestroyZPosition(float z)
    // enemyData.speed = config.enemySpeed; // ���� EnemyData ��������� ���������
    // enemyData.SetDestroyZPosition(destroyLimitZ);


    private void CreateShapesInSquare(EnemyData enemyData, LevelConfig config)
    {
        // 1. ��������� ���������
        int minShapes = config.minShapes;
        int maxShapes = config.maxShapes;

        // ���������� ����� ���������� Shape, ������� ����� ����������
        int shapesToSpawn = Random.Range(minShapes, maxShapes + 1);

        // ���������� ������ ����� (������� ��������)
        int sideLength = Mathf.CeilToInt(Mathf.Sqrt(shapesToSpawn));

        // �������� ��� ������� Shape
        int totalHealth = Mathf.RoundToInt(config.baseShapeHealth * config.healthMultiplier);

        // ���������� ����� �������� �������� Shape
        float spacing = 1.2f;

        // ��������� ������� ��� �������������
        float startX = -(sideLength - 1) * spacing / 2f;
        float startY = -(sideLength - 1) * spacing / 2f;

        int shapeCounter = 0;

        // 2. ��������� � ���������� � �����
        for (int row = 0; row < sideLength; row++)
        {
            for (int col = 0; col < sideLength; col++)
            {
                if (shapeCounter >= shapesToSpawn)
                {
                    // ���������������, ���� ���������� ����������� ���������� Shape
                    return;
                }

                // ������� Shape ��� �������� ������
                GameObject newShapeGO = Instantiate(baseShapePrefab, enemyData.transform);
                Shape shapeComponent = newShapeGO.GetComponent<Shape>();

                // ��������� ��������� �������
                float localX = startX + col * spacing;
                float localZ = startY + row * spacing;

                // ������������� ��������� ������� (Y � X)
                newShapeGO.transform.localPosition = new Vector3(localX, 0, localZ);

                if (shapeComponent != null)
                {
                    shapeComponent.health = totalHealth;
                    // �������� ������ ����������� � EnemyData.Start/Awake
                }
                else
                {
                    Debug.LogError("������: Shape Prefab ������ ��������� Shape!");
                }

                shapeCounter++;
            }
        }
    }
}