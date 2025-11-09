using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    // События для внешних подписчиков (например, GameController)
    public event Action<int> OnEnemyPartDestroyed;          // очки за уничтожение части
    public event Action<int> OnEnemyCompletelyDestroyed;    // бонус за уничтожение всего врага

    [Header("Движение врага")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float destroyZPosition = -10f;

    [Header("Настройки бонуса")]
    [SerializeField] private int bonusMultiplier = 2; // множитель бонусных очков за последнюю часть

    private List<Shape> shapes; // все части врага

    public int ShapesCount => shapes?.Count ?? 0;

    private void Start()
    {
        // Находим все дочерние Shape-компоненты
        shapes = new List<Shape>(GetComponentsInChildren<Shape>());
        Debug.Log($"Найдено дочерних компонентов типа Shape: {ShapesCount}");

        // Подписываемся на события каждой части
        foreach (Shape shape in shapes)
        {
            shape.OnShapeDestroyed += HandleShapeDestroyed;
        }
    }

    private void Update()
    {
        // Простое движение врага вперёд
        transform.Translate(Vector3.back * speed * Time.deltaTime);

        // Удаляем врага, если он вышел за пределы
        if (transform.position.z < destroyZPosition)
        {
            Destroy(gameObject);
        }
    }

    private void HandleShapeDestroyed(Shape destroyedShape)
    {        
        shapes.Remove(destroyedShape);      

        // Проверяем, остались ли части
        if (ShapesCount == 0)
        {           

            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // Безопасно отписываемся от событий всех частей
        if (shapes == null) return;

        foreach (Shape shape in shapes)
        {
            if (shape != null)
                shape.OnShapeDestroyed -= HandleShapeDestroyed;
        }
    }

    // --- Вспомогательные методы ---

    public void SetDestroyZPosition(float z)
    {
        destroyZPosition = z;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void SetHealthForAllParts(int value)
    {
        foreach (Shape shape in shapes)
        {
            shape.health = value;
        }
    }
}
