using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayersManagerSingletone : MonoBehaviour
{
    private static PlayersManagerSingletone _instance;
    public static PlayersManagerSingletone Instance
    {
        get
        {           
            if (_instance != null)
            {
                return _instance;
            }

            _instance = FindObjectOfType<PlayersManagerSingletone>();
            
            if (_instance == null)
            {                
                Debug.LogError("PlayersManagerSingletone.Instance: В сцене отсутствует объект PlayerStats!");
            }

            return _instance;
        }
    }

    [SerializeField] private PlayerCharacter localPlayer;
    public PlayerCharacter LocalPlayer => localPlayer;

    private void Awake()
    {
       
        if (_instance != null && _instance != this)       
        {          
            Destroy(gameObject);
        }
        else if (_instance == null)
        {           
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
       
    }

    public void RegisterLocalPlayer(PlayerCharacter player)
    {
        localPlayer = player;       
    }


}
