using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveComponent : MonoBehaviour
{
    // Зависимость (Dependency Injection) через интерфейс
    private IPlayerInput _input;
    private Rigidbody _rb;
    public Vector2 _currentMoveInput;


    public float currentSpeed;
    private float previousSpeed;
    // Start is called before the first frame update
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
        currentSpeed = PlayersManagerSingletone.Instance.LocalPlayer.Stats.movementSpeed;
        previousSpeed = currentSpeed;
    }

    void FixedUpdate()
    {
        // Движение (используем только X-компоненту для лево/право)
        float horizontalMovement = _currentMoveInput.x;

        Vector3 movement = new Vector3(horizontalMovement, 0, 0) * currentSpeed * Time.fixedDeltaTime;

        _rb.MovePosition(_rb.position + movement);
    }
    void OnDestroy()
    {
        // Важно: отписываемся от событий
        if (_input != null)
        {
            _input.OnMove -= HandleMoveInput;           
        }
    }


    private void HandleMoveInput(Vector2 moveVector)
    {
        // Сохраняем полученный вектор движения (x и y)
        _currentMoveInput = moveVector;
    }

    public void ChangeSpeed(float speed)
    {
        currentSpeed += speed;
    }

    public void StopMovement(bool isOn)
    {
        currentSpeed = isOn ? 0 : previousSpeed;       
        Debug.Log("current speed = " +  currentSpeed);
    }


}
