using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    List<Shape> shapes;  

    public float speed = 10f;

    private float destroyZPosition = -10f;

    public int shapesCount => shapes.Count;

    private void Start()
    {
        shapes = new List<Shape>(GetComponentsInChildren<Shape>());
        Debug.Log($"Найдено дочерних компонентов типа ChildComponent: {shapesCount}");

        foreach (Shape shape in shapes)
        {
            // Подписываемся на событие Shape
            shape.OnShapeDestroyed += HandleShapeDestroyed;
        }

    }
    private void HandleShapeDestroyed(Shape destroyedShape)
    {
        // 4. Удаляем уничтоженный Shape из списка родителя
        shapes.Remove(destroyedShape);

        // Проверяем, умер ли враг (исчезли ли все Shape)
        Die();
    }
    private void Die()
    {
        if (shapesCount == 0)
        {
            Debug.Log("Смерть врага");
        }
    }

    void Update()
    {        
        transform.Translate(Vector3.back * speed * Time.deltaTime);

        
        if (transform.position.z < destroyZPosition)
        {            
            Destroy(gameObject);
           
        }
    }

    public void SetDestroyZPosition(float z)
    {
        destroyZPosition = z;
    }

    public void SetHealth()
    {
        foreach (Shape shape in shapes)
        {
            shape.health = 100;
        }
    }

    private void OnDestroy()
    {
        // Если EnemyData будет уничтожен раньше, чем дочерние Shape,
        // нужно отписаться, чтобы избежать висячих ссылок.
        foreach (Shape shape in shapes)
        {
            if (shape != null) // Проверяем, что Shape еще существует
            {
                shape.OnShapeDestroyed -= HandleShapeDestroyed;
            }
        }
    }
}
