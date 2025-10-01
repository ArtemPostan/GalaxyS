// PlayerInputHandler.cs
using UnityEngine;
using UnityEngine.InputSystem;

// ��������� ��� ������������� ���������
public class PlayerInputHandler : MonoBehaviour, IPlayerInput
{
    private PlayerControls _controls;

    // ���������� ������� ����������
    public event System.Action<Vector2> OnMove;
    public event System.Action<bool> OnShoot;

    private void Awake()
    {
        // ������� ��������� ���������������� ������
        _controls = new PlayerControls();

        // ----------------------------------------------------
        // �������� �� ��������:
        // ----------------------------------------------------

        // �������� (Move):
        // ��� ������������ (performed) ��������� Vector2 � �������� ���� ������� OnMove
        _controls.Gameplay.Move.performed += ctx => OnMove?.Invoke(ctx.ReadValue<Vector2>());

        // ��� ������ (canceled) �������� Vector2.zero, ����� ���������� ��������
        _controls.Gameplay.Move.canceled += ctx => OnMove?.Invoke(Vector2.zero);

        // �������� (Shoot):
        // ��� ������� (performed) �������� OnShoot � ���������� true
        _controls.Gameplay.Shoot.performed += ctx => OnShoot?.Invoke(true);

        // ��� ���������� (canceled) �������� OnShoot � ���������� false
        _controls.Gameplay.Shoot.canceled += ctx => OnShoot?.Invoke(false);
    }

    // ������ ��� ���������/����������, ��������� �����������
    public void EnableInput() => _controls.Enable();
    public void DisableInput() => _controls.Disable();

    private void OnEnable() => EnableInput();
    private void OnDisable() => DisableInput();

    // �����: ���������� ��������� InputActions ��� ����������� �������
    private void OnDestroy() => _controls.Dispose();
}