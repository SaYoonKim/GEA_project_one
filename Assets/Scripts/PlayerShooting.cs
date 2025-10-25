using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    // 참조 2개
    public GameObject projectilePrefab; // Projectile 프리팹
    // 참조 2개
    public Transform firePoint; // 발사 위치 (총구)

    Camera cam;
    // 참조 0개
    void Start()
    {
        cam = Camera.main; // 메인 카메라 가져오기
    }

    // 참조 0개
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 좌클릭 발사
        {
            Shoot();
        }
    }

    // 참조 1개
    void Shoot()
    {
        // 화면에서 마우스를 광선(Ray) 쏘기
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPoint = ray.GetPoint(50f);
        Vector3 direction = (targetPoint - firePoint.position).normalized; // 방향 벡터

        // Projectile 생성
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(direction));
    }
}