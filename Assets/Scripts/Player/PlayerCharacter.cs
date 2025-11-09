// PlayerCharacter.cs (или PlayerMovementLogic.cs)
using System;
using UnityEngine;

// Требует наличия PlayerInputHandler на том же объекте для получения ввода
[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerCharacter : MonoBehaviour
{    
    [SerializeField] ShooterComponent shooterComponent;
    public ShooterComponent ShooterComponent => shooterComponent;

    [SerializeField] MoveComponent moveComponent;

    public MoveComponent MoveComponent => moveComponent;

    public PlayerStats Stats;

    

    private void Awake()
    {
        Stats = new PlayerStats();
        Stats.Initialize();
    }
    void Start()
    {
        
    }

    

 

    

    public void StopShooting()
    {
        shooterComponent.StopShooting();
    }
}