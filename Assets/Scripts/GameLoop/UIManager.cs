using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameController gameController;
    [SerializeField] GameObject UIShop;
    [SerializeField] LevelInfo levelInfo;

    private float currentScore;
    private int currentLevel;
    private int scoreToWin;


    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI scoreToWinText;



    public void InitializePlayerUI()
    {        
        currentScore = PlayersManagerSingletone.Instance.LocalPlayer.Stats.scoreInCurrentLevel;
        currentLevel = PlayersManagerSingletone.Instance.LocalPlayer.Stats.currentLevel;
        scoreToWin = levelInfo.GetScoreToWin();
        UpdatePlayerUI();
    }
    public void OnShopOpen(bool isOn)
    {
        UIShop.SetActive(isOn);
        Debug.Log("Магазин " +  isOn);
    }   

    public void OnClickSkip()
    {
        
        gameController.CloseInterLevelMenu();

    }

    public void UpdatePlayerUI()
    {
        currentScore = PlayersManagerSingletone.Instance.LocalPlayer.Stats.scoreInCurrentLevel;
        currentLevel = PlayersManagerSingletone.Instance.LocalPlayer.Stats.currentLevel;
        scoreToWin = levelInfo.GetScoreToWin();
        scoreText.text = currentScore.ToString();
        levelText.text = currentLevel.ToString();
        scoreToWinText.text = scoreToWin.ToString();
    }
}
