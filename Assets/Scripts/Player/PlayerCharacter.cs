// PlayerCharacter.cs (��� PlayerMovementLogic.cs)
using UnityEngine;

// ������� ������� PlayerInputHandler �� ��� �� ������� ��� ��������� �����
[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerCharacter : MonoBehaviour
{
    // ����������� (Dependency Injection) ����� ���������
    private IPlayerInput _input;
    private Rigidbody _rb;
    private Vector2 _currentMoveInput;

    public float MoveSpeed = 5f;

    void Start()
    {
        // �������� ������ �� ��� ���������� ����� ���������
        _input = GetComponent<IPlayerInput>();
        _rb = GetComponent<Rigidbody>();

        if (_input == null)
        {
            Debug.LogError("PlayerCharacter ������� PlayerInputHandler.");
            return;
        }

        // ������������� �� ������� �����
        _input.OnMove += HandleMoveInput;
        _input.OnShoot += HandleShootInput;
    }

    private void HandleMoveInput(Vector2 moveVector)
    {
        // ��������� ���������� ������ �������� (x � y)
        _currentMoveInput = moveVector;
    }

    private void HandleShootInput(bool isShooting)
    {
        if (isShooting)
        {
            // ����� ����� ���� ����� ������ ������ ����
            Debug.Log(">> ���! (����� ��������!)");
        }
    }

    void FixedUpdate()
    {
        // �������� (���������� ������ X-���������� ��� ����/�����)
        float horizontalMovement = _currentMoveInput.x;

        Vector3 movement = new Vector3(horizontalMovement, 0, 0) * MoveSpeed * Time.fixedDeltaTime;

        _rb.MovePosition(_rb.position + movement);
    }

    void OnDestroy()
    {
        // �����: ������������ �� �������
        if (_input != null)
        {
            _input.OnMove -= HandleMoveInput;
            _input.OnShoot -= HandleShootInput;
        }
    }
}