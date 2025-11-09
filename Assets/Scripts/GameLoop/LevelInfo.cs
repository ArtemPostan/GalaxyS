using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    public event Action<int> TakeScore;

    private int scoreToWin = 10;

    public void IncrementScoreToWin()
    {
        scoreToWin *= PlayersManagerSingletone.Instance.LocalPlayer.Stats.currentLevel * 2;
        Debug.Log("Сейчас до следующего уровня нужно " + scoreToWin);
    }

    public int GetScoreToWin()
    {
        return scoreToWin;
    }
}
