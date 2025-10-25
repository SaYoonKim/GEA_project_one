using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    // ���� 2��
    public GameObject projectilePrefab; // Projectile ������
    // ���� 2��
    public Transform firePoint; // �߻� ��ġ (�ѱ�)

    Camera cam;
    // ���� 0��
    void Start()
    {
        cam = Camera.main; // ���� ī�޶� ��������
    }

    // ���� 0��
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ��Ŭ�� �߻�
        {
            Shoot();
        }
    }

    // ���� 1��
    void Shoot()
    {
        // ȭ�鿡�� ���콺�� ����(Ray) ���
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPoint = ray.GetPoint(50f);
        Vector3 direction = (targetPoint - firePoint.position).normalized; // ���� ����

        // Projectile ����
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(direction));
    }
}