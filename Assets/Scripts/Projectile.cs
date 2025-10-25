using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damageAmount = 1; // �� ����ü�� ���� ������
    public float speed = 20f;
    public float lifeTime = 2f;


    void Start()
    {
        Destroy(gameObject, lifeTime);

    }


    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 1. �浹�� ������Ʈ�� �±װ� "Enemy"���� Ȯ��
        if (other.CompareTag("Enemy"))
        {
            // 2. Enemy ��ũ��Ʈ ������Ʈ�� ������
            Enemy enemy = other.GetComponent<Enemy>();

            // 3. Enemy ��ũ��Ʈ�� �����Ѵٸ�
            if (enemy != null)
            {
                // 4. Enemy�� TakeDamage �޼��带 ȣ���Ͽ� �������� ����
                enemy.TakeDamage(damageAmount);
            }

            // 5. �������� ���� �� ����ü �ڽ��� �ı�
            Destroy(gameObject);
        }

    }
}