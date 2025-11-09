using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public event Action OnShopUpdate;

    [Header("Level Goals")]
    [SerializeField] private int _killsToWin = 1;
    public int CurrentKills { get; private set; } = 0;

    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private LevelInfo levelInfo;  

    private IGameState _currentState;
    private Dictionary<GameState, IGameState> _allStates;

    private void Awake()
    {
        _allStates = new Dictionary<GameState, IGameState>()
        {
            { GameState.LevelStart, new LevelStartState() },
            { GameState.InGame, new InGameState() },
            { GameState.InterLevelMenu, new InterLevelMenuState() }
        };

        ChangeState(GameState.LevelStart);
      

        // Подписываемся на событие спауна новых врагов
        spawnManager.OnEnemySpawned += OnEnemySpawnedHandler;
    }

    private void Start()
    {
        uiManager.InitializePlayerUI();
    }

    private void OnDestroy()
    {
        spawnManager.OnEnemySpawned -= OnEnemySpawnedHandler;
    }

    // 🚀 Когда появляется новый враг
    private void OnEnemySpawnedHandler(EnemyData newEnemy)
    {
        // Подписываемся на события частей врага
        foreach (var shape in newEnemy.GetComponentsInChildren<Shape>())
        {
            shape.OnShapeDestroyed += OnShapeDestroyedHandler;
        }

        // Можно подписаться и на бонусное событие врага, если нужно
        newEnemy.OnEnemyCompletelyDestroyed += OnEnemyBonusHandler;
    }

    private void OnEnemyBonusHandler(int bonus)
    {
        var player = PlayersManagerSingletone.Instance.LocalPlayer;
        player.Stats.score += bonus;
        player.Stats.scoreInCurrentLevel += bonus;

        Debug.Log($"💰 Бонусные очки за уничтожение врага: {bonus}");
    }

    private void OnShapeDestroyedHandler(Shape destroyedShape)
    {
        // Отписываемся, чтобы не ловить повторно
        destroyedShape.OnShapeDestroyed -= OnShapeDestroyedHandler;

        int points = destroyedShape.GetScore();
        var player = PlayersManagerSingletone.Instance.LocalPlayer;

        player.Stats.score += points;
        player.Stats.scoreInCurrentLevel += points;
        CurrentKills++;

        Debug.Log($"🧩 Shape уничтожен! +{points} очков. Всего убийств: {CurrentKills}");
        uiManager.UpdatePlayerUI();
        CheckLevelCompletion();
    }

    private void CheckLevelCompletion()
    {
        if (PlayersManagerSingletone.Instance.LocalPlayer.Stats.scoreInCurrentLevel >= levelInfo.GetScoreToWin())
        {
            if (_currentState is InGameState)
            {
                EndLevel();
            }
        }
    }

    public void ChangeState(GameState newStateKey)
    {
        _currentState?.ExitState(this);

        if (_allStates.TryGetValue(newStateKey, out var newState))
        {
            _currentState = newState;
            Debug.Log($"<color=yellow>Смена состояния:</color> {newStateKey}");
            _currentState.EnterState(this);
        }
    }

    public void StartLevel()
    {
        spawnManager.isSpawning = true;
        spawnManager.StartSpawning();

        var player = PlayersManagerSingletone.Instance.LocalPlayer;
        player.MoveComponent.StopMovement(false);
        player.ShooterComponent.StartShooting();
    }

    public void OpenInterLevelMenu(bool isOpen)
    {
        uiManager.OnShopOpen(isOpen);
        OnShopUpdate?.Invoke();
    }

    public void EndLevel()
    {
        if (_currentState is InGameState)
        {
            levelInfo.IncrementScoreToWin();
            PlayersManagerSingletone.Instance.LocalPlayer.Stats.ResetScore();
            PlayersManagerSingletone.Instance.LocalPlayer.Stats.currentLevel++;
            uiManager.UpdatePlayerUI();
            PlayersManagerSingletone.Instance.LocalPlayer.MoveComponent.StopMovement(true);
            spawnManager.ClearAllEnemies();
          
            ChangeState(GameState.InterLevelMenu);
        }
    }

    public void CloseInterLevelMenu()
    {
        Debug.Log("Закрытие магазина");
        ChangeState(GameState.LevelStart);
    }
}
