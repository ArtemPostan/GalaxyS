using UnityEngine;

public class ShooterComponent : MonoBehaviour
{
    [Header("��������� ����")]
    [Tooltip("������ ����, ������� ����� ��������������.")]
    public GameObject bulletPrefab;

    [Tooltip("����� � ������������, ������ ����� ���������� ������� (��������, '����').")]
    public Transform firePoint;

    [Header("��������� ��������")]
    [Tooltip("������� ��������� � ������� (��� ���� ��������, ��� ���� ��������).")]
    public float fireRatePerSecond = 2f;

    [Tooltip("�������� �� �������������� �������� ��� ������.")]
    public bool autoFireOnStart = true;

    // ��������� ���������� ��� ���������� ��������
    private float timeBetweenShots;
    private float nextFireTime;
    private bool isShooting = false;


    void Start()
    {
        // ��������� ����� ����� ����������: 1 / �������
        if (fireRatePerSecond > 0)
        {
            timeBetweenShots = 1f / fireRatePerSecond;
        }
        else
        {
            timeBetweenShots = 0.1f; // �������� �� ���������
        }

        // ���� �������� ������������, ��������
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

    // --- �������� ����� �������� ---

    private void Shoot()
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogError("������: �� ���������� ������ ���� ��� ����� �������� (FirePoint)!");
            return;
        }

        // ������� ���� � ������� � � ��������� firePoint
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // �����������: ����� ��������� ��������� ���� �����
        // ��������: bullet.GetComponent<Bullet>().Initialize(transform.forward);
    }

    // --- ��������� ������ ���������� ---

    /// <summary>
    /// �������� �������������� ��������.
    /// </summary>
    public void StartShooting()
    {
        isShooting = true;
        // ������������� ������ ��� ������������ ������� ��������
        nextFireTime = Time.time;
    }

    /// <summary>
    /// ��������� �������������� ��������.
    /// </summary>
    public void StopShooting()
    {
        isShooting = false;
    }

    /// <summary>
    /// �������� ������� ��������.
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