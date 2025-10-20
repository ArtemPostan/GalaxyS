using UnityEngine;

public class ShooterComponent : MonoBehaviour
{
    [Header("Настройки Пули")]
    [Tooltip("Префаб пули, которую нужно инстанцировать.")]
    public GameObject bulletPrefab;

    [Tooltip("Точка в пространстве, откуда будет начинаться выстрел (например, 'Дуло').")]
    public Transform firePoint;

    [Header("Настройки Стрельбы")]
    [Tooltip("Частота выстрелов в секунду (чем выше значение, тем чаще стрельба).")]
    public float fireRatePerSecond = 2f;

    [Tooltip("Включить ли автоматическую стрельбу при старте.")]
    public bool autoFireOnStart = true;

    // Приватные переменные для управления таймером
    private float timeBetweenShots;
    private float nextFireTime;
    private bool isShooting = false;


    void Start()
    {
        // Вычисляем время между выстрелами: 1 / Частота
        if (fireRatePerSecond > 0)
        {
            timeBetweenShots = 1f / fireRatePerSecond;
        }
        else
        {
            timeBetweenShots = 0.1f; // Значение по умолчанию
        }

        // Если включена автострельба, начинаем
        if (autoFireOnStart)
        {
            StartShooting();
        }
    }

    void Update()
    {
        if (isShooting && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + timeBetweenShots;
        }
    }

    // --- Основной метод выстрела ---

    private void Shoot()
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogError("Ошибка: Не установлен префаб пули или точка выстрела (FirePoint)!");
            return;
        }

        // Создаем пулю в позиции и с поворотом firePoint
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Опционально: можно настроить компонент пули здесь
        // Например: bullet.GetComponent<Bullet>().Initialize(transform.forward);
    }

    // --- Публичные методы управления ---

    /// <summary>
    /// Включает автоматическую стрельбу.
    /// </summary>
    public void StartShooting()
    {
        isShooting = true;
        // Устанавливаем таймер для немедленного первого выстрела
        nextFireTime = Time.time;
    }

    /// <summary>
    /// Выключает автоматическую стрельбу.
    /// </summary>
    public void StopShooting()
    {
        isShooting = false;
    }

    /// <summary>
    /// Изменяет частоту стрельбы.
    /// </summary>
    public void SetFireRate(float newRate)
    {
        if (newRate > 0)
        {
            fireRatePerSecond = newRate;
            timeBetweenShots = 1f / fireRatePerSecond;
        }
    }
}