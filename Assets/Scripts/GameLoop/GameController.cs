using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public event Action OnShopUpdate;

    [Header("Level Goals")]
    [SerializeField] private int _killsToWin = 1;
    public int CurrentKills { get; private set; } = 0;

    [SerializeField] SpawnManager spawnManager;
    [SerializeField] UIManager UIManager;
    public int CurrentLevel { get; private set; } = 1;

    private IGameState _currentState;
    private Dictionary<GameState, IGameState> _allStates;

    private void Awake()
    {
        // Инициализация всех возможных состояний
        _allStates = new Dictionary<GameState, IGameState>()
        {
            { GameState.LevelStart, new LevelStartState() },
            { GameState.InGame, new InGameState() },
            { GameState.InterLevelMenu, new InterLevelMenuState() }
        };

        // Начинаем с подготовки к первому уровню
        ChangeState(GameState.LevelStart);
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
        PlayersManagerSingletone.Instance.LocalPlayer.ShooterComponent.StartShooting();
    }

    // Метод 2: Открытие магазина (логика, вызываемая из InterLevelMenuState)
    public void OpenInterLevelMenu(bool isOpen)
    {
        UIManager.OnShopOpen(isOpen);

        OnShopUpdate?.Invoke();
    }

    // Метод 3: Конец уровня (Вызывается извне, например, от скрипта триггера победы)
    public void EndLevel()
    {
        if (_currentState.GetType() == typeof(InGameState))
        {
            spawnManager.ClearAllEnemies();
            //spawnManager.isSpawning = false;
            Debug.Log($"Уровень {CurrentLevel} пройден. Готовим магазин...");
            CurrentLevel++; // Подготовка к следующему уровню
            ChangeState(GameState.InterLevelMenu);
        }
    }

    // Метод 4: Закрытие магазина (Вызывается из UI магазина по нажатию кнопки "Продолжить")
    public void CloseInterLevelMenu()
    {
        Debug.Log("Закрытие магазина");
        ChangeState(GameState.LevelStart);
    }

    public void ResetLevelData()
    {
        CurrentKills = 0;
        // Здесь же можно сбрасывать таймеры, ресурсы и т.д.
    }   

    private void CheckLevelCompletion()
    {
        // Условие победы: Убито достаточно врагов
        if (CurrentKills >= _killsToWin)
        {
            // Важно: проверяем, что мы вообще находимся в игровом состоянии
            if (_currentState.GetType() == typeof(InGameState))
            {
                // Вызываем ваш существующий метод EndLevel
                EndLevel();
            }
        }
    }

    public void SubscribeToShape(Shape shape)
    {
        // Подписываем метод-обработчик на событие
        shape.OnShapeDestroyed += OnShapeDestroyedHandler;
    }

    // 2. Метод-обработчик события
    private void OnShapeDestroyedHandler(Shape destroyedShape)
    {      
        destroyedShape.OnShapeDestroyed -= OnShapeDestroyedHandler;
       
        CurrentKills++;
        Debug.Log($"Убито: {CurrentKills}");       
        CheckLevelCompletion();
    }

    public IGameState GetCurrentState()
    {
        return _currentState;
    }

    public void CleanLevel()
    {

    }

    
}