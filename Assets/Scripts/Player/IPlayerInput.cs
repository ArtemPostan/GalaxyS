using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerInput 
{
    // Движение: возвращает Vector2 (x, y)
    event Action<Vector2> OnMove;

    // Стрельба: возвращает true при нажатии, false при отпускании
    event Action<bool> OnShoot;

    void EnableInput();
    void DisableInput();
}
