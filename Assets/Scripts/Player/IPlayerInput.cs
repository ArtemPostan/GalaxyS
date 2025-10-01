using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerInput 
{
    // ��������: ���������� Vector2 (x, y)
    event Action<Vector2> OnMove;

    // ��������: ���������� true ��� �������, false ��� ����������
    event Action<bool> OnShoot;

    void EnableInput();
    void DisableInput();
}
