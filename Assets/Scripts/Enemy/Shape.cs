using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Shape : MonoBehaviour, IHitable
{
    public event Action<Shape> OnShapeDestroyed;

    public int health;

    private int score;

    [SerializeField] private TextMeshProUGUI healtText;

    private void Awake()
    {
        score = health / 50;
    }

    private void Start()
    {        
        DisplayHealth();
    }   

    public void DisplayHealth()
    {
        healtText.text = health.ToString();
    }

    public void OnDie()
    {
        OnShapeDestroyed?.Invoke(this);
        Destroy(gameObject);
    }

    public void OnHit(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            OnDie();
            return;
        }
        DisplayHealth();
    }
    public int GetScore()
    {
        return score;
    }
}