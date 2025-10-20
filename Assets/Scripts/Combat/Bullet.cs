using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Tooltip("�������� ����.")]
    public float speed = 20f;

    [Tooltip("����� ����� ���� � �������� (����� ��� �� ������ ����������).")]
    public float lifeTime = 5f;

    void Start()
    {
        // ���������� ���� ����� �������� �����
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // ������� ���� ������ (����� � ����������� ��� Z, ������� ��� �������� �� firePoint)
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("���������");
        if (!other.gameObject.CompareTag("Enemy")) return;
     
       IHitable hitable = other.GetComponent<IHitable>();
        if (hitable != null)
        {
            hitable.OnHit(10);
        }

     
        Destroy(gameObject);
    }
}