// PlayerInputHandler.cs
using UnityEngine;
using UnityEngine.InputSystem;

// Реализует ваш универсальный интерфейс
public class PlayerInputHandler : MonoBehaviour, IPlayerInput
{
    private PlayerControls _controls;

    // Реализация событий интерфейса
    public event System.Action<Vector2> OnMove;
    public event System.Action<bool> OnShoot;

    private void Awake()
    {
        // Создаем экземпляр сгенерированного класса
        _controls = new PlayerControls();

        // ----------------------------------------------------
        // ПОДПИСКА НА ДЕЙСТВИЯ:
        // ----------------------------------------------------

        // ДВИЖЕНИЕ (Move):
        // При срабатывании (performed) считываем Vector2 и вызываем наше событие OnMove
        _controls.Gameplay.Move.performed += ctx => OnMove?.Invoke(ctx.ReadValue<Vector2>());

        // При отмене (canceled) передаем Vector2.zero, чтобы остановить движение
        _controls.Gameplay.Move.canceled += ctx => OnMove?.Invoke(Vector2.zero);

        // СТРЕЛЬБА (Shoot):
        // При нажатии (performed) вызываем OnShoot с параметром true
        _controls.Gameplay.Shoot.performed += ctx => OnShoot?.Invoke(true);

        // При отпускании (canceled) вызываем OnShoot с параметром false
        _controls.Gameplay.Shoot.canceled += ctx => OnShoot?.Invoke(false);
    }

    // Методы для включения/выключения, требуемые интерфейсом
    public void EnableInput() => _controls.Enable();
    public void DisableInput() => _controls.Disable();

    private void OnEnable() => EnableInput();
    private void OnDisable() => DisableInput();

    // Важно: уничтожаем экземпляр InputActions при уничтожении объекта
    private void OnDestroy() => _controls.Dispose();
}