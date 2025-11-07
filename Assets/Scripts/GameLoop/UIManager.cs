using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameController gameController;
    [SerializeField] GameObject UIShop;
    
    public void OnShopOpen(bool isOn)
    {
        UIShop.SetActive(isOn);
        Debug.Log("Магазин " +  isOn);
    }   

    public void OnClickSkip()
    {
        
        gameController.CloseInterLevelMenu();

    }
}
