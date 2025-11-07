using UnityEngine;

// 1. Перечисление состояний
public enum GameState
{
    LevelStart,        // Перед запуском уровня
    InGame,            // Идет игра
    InterLevelMenu     // Магазин / Меню между уровнями
}

// 2. Интерфейс состояния (IGameState)
public interface IGameState
{
    void EnterState(GameController controller);
    void ExitState(GameController controller);
}