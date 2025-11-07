// --- Состояние Загрузки ---
using UnityEngine;

public class LoadingState : IGameState
{
    public void EnterState(GameController controller)
    {
        // Начало загрузки данных
        //controller.LoadPlayerData();
        // В реальном проекте здесь будет асинхронная загрузка, 
        // а ChangeState будет вызван после ее завершения.
    }

    public void ExitState(GameController controller) { /* Очистка экрана загрузки */ }
}

// --- Состояние Игры ---
public class InGameState : IGameState
{
    public void EnterState(GameController controller)
    {
        Time.timeScale = 1; // Убеждаемся, что игра не на паузе
        // ... (Запуск таймеров, активация игровых систем)
    }
   

    public void ExitState(GameController controller)
    {
        // Отключение ввода игрока, остановка событий уровня
    }
}

// --- Состояние Конец Уровня ---
public class LevelEndState : IGameState
{
    public void EnterState(GameController controller)
    {
       
    }
   
    public void ExitState(GameController controller) { /* Скрытие экрана результатов */ }
}

public class LevelStartState : IGameState
{
    public void EnterState(GameController controller)
    {
        controller.ResetLevelData(); // <-- Сброс счетчика CurrentKills = 0
        controller.StartLevel();
        controller.ChangeState(GameState.InGame);
    }

    public void ExitState(GameController controller) { /* Скрытие экрана результатов */ }
}

// --- Состояние Сохранения ---
public class SavingState : IGameState
{
    public void EnterState(GameController controller)
    {
        
    }

    public void ExitState(GameController controller) { /* Убрать индикатор сохранения */ }
}

// --- Состояние Магазина Между Уровнями ---
public class InterLevelMenuState : IGameState
{
    public void EnterState(GameController controller)
    {
        
        controller.OpenInterLevelMenu(true);
    }
   

    public void ExitState(GameController controller)
    {
        Debug.Log("ExitStateofInterLevel");
        
        controller.OpenInterLevelMenu(false);       
       
    }
}

