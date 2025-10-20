using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Tooltip("Скорость пули.")]
    public float speed = 20f;

    [Tooltip("Время жизни пули в секундах (чтобы она не летела бесконечно).")]
    public float lifeTime = 5f;

    void Start()
    {
        // Уничтожаем пулю через заданное время
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Двигаем пулю вперед (вдоль её собственной оси Z, которую она получила от firePoint)
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Попадание");
        if (!other.gameObject.CompareTag("Enemy")) return;
     
       IHitable hitable = other.GetComponent<IHitable>();
        if (hitable != null)
        {
            hitable.OnHit(10);
        }

     
        Destroy(gameObject);
    }
}