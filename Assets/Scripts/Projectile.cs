using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damageAmount = 1; // 이 투사체가 입힐 데미지
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
        // 1. 충돌한 오브젝트의 태그가 "Enemy"인지 확인
        if (other.CompareTag("Enemy"))
        {
            // 2. Enemy 스크립트 컴포넌트를 가져옴
            Enemy enemy = other.GetComponent<Enemy>();

            // 3. Enemy 스크립트가 존재한다면
            if (enemy != null)
            {
                // 4. Enemy의 TakeDamage 메서드를 호출하여 데미지를 입힘
                enemy.TakeDamage(damageAmount);
            }

            // 5. 데미지를 입힌 후 투사체 자신을 파괴
            Destroy(gameObject);
        }

    }
}