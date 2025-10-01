// PlayerCharacter.cs (или PlayerMovementLogic.cs)
using UnityEngine;

// Требует наличия PlayerInputHandler на том же объекте для получения ввода
[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerCharacter : MonoBehaviour
{
    // Зависимость (Dependency Injection) через интерфейс
    private IPlayerInput _input;
    private Rigidbody _rb;
    private Vector2 _currentMoveInput;

    public float MoveSpeed = 5f;

    void Start()
    {
        // Получаем ссылку на наш обработчик через интерфейс
        _input = GetComponent<IPlayerInput>();
        _rb = GetComponent<Rigidbody>();

        if (_input == null)
        {
            Debug.LogError("PlayerCharacter требует PlayerInputHandler.");
            return;
        }

        // Подписываемся на события ввода
        _input.OnMove += HandleMoveInput;
        _input.OnShoot += HandleShootInput;
    }

    private void HandleMoveInput(Vector2 moveVector)
    {
        // Сохраняем полученный вектор движения (x и y)
        _currentMoveInput = moveVector;
    }

    private void HandleShootInput(bool isShooting)
    {
        if (isShooting)
        {
            // Здесь может быть вызов метода спавна пули
            Debug.Log(">> БАХ! (Игрок стреляет!)");
        }
    }

    void FixedUpdate()
    {
        // Движение (используем только X-компоненту для лево/право)
        float horizontalMovement = _currentMoveInput.x;

        Vector3 movement = new Vector3(horizontalMovement, 0, 0) * MoveSpeed * Time.fixedDeltaTime;

        _rb.MovePosition(_rb.position + movement);
    }

    void OnDestroy()
    {
        // Важно: отписываемся от событий
        if (_input != null)
        {
            _input.OnMove -= HandleMoveInput;
            _input.OnShoot -= HandleShootInput;
        }
    }
}