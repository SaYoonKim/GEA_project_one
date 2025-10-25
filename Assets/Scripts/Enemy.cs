using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyState { Idle, Trace, Attack, Flee } // Flee ���� �߰�
    public EnemyState state = EnemyState.Idle;

    public float moveSpeed = 2f;
    public float traceRange = 15f; // ���� ���� �Ÿ�
    public float attackRange = 6f; // ���� ���� �Ÿ�
    public float attackCooldown = 1.5f;

    public GameObject projectilePrefab; // ����ü ������
    public Transform firePoint; // �߻� ��ġ

    private Transform player;
    private float lastAttackTime;
    public int maxHP = 5;
    private int currentHP;

    public Slider EnemyhpSlider;

    private Rigidbody rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastAttackTime = -attackCooldown;
        currentHP = maxHP;
        EnemyhpSlider.value = 1f;

        rb = GetComponent<Rigidbody>();
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        EnemyhpSlider.value = (float)currentHP / maxHP;

        if (currentHP > 0 && currentHP <= maxHP * 0.2f)
        {
            // ���� ���� ���°� �ƴ϶�� ���� ��ȯ
            if (state != EnemyState.Flee)
            {
                state = EnemyState.Flee;
            }
        }

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(player.position, transform.position);
        float fleeThreshold = maxHP * 0.2f; // 20% HP �Ӱ�ġ

        //  �̹� ���� �����̰ų�, HP�� 20% ���ϸ� �ٸ� ���·� �Ѿ�� �ʰ� ���� ����
        if (currentHP > 0 && currentHP <= fleeThreshold) 
        {
            state = EnemyState.Flee;
        }

        // FSM ���� ��ȯ
        switch (state)
        {
            case EnemyState.Idle:
                if (dist < traceRange)
                    state = EnemyState.Trace;
                break;

            case EnemyState.Trace:
                if (dist < attackRange)
                    state = EnemyState.Attack;
                else if (dist > traceRange)
                    state = EnemyState.Idle;
                break;

            case EnemyState.Attack:
                if (dist > attackRange)
                    state = EnemyState.Trace;
                else
                    AttackPlayer();
                break;

            case EnemyState.Flee: // Flee ���� �߰�
                break;
        }
    }

    void FixedUpdate()
    {
        if (rb == null || player == null) return;

        // ���� ���¿� ���� ������ ���� �̵� �Լ� ȣ��
        switch (state)
        {
            case EnemyState.Trace:
                TracePlayer();
                break;

            case EnemyState.Flee:
                FleeFromPlayer();
                break;

            default:
                // Idle/Attack ���¿����� �̵� ������ ȣ������ �ʴ´�.
                break;
        }
    }

    void TracePlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;

        Vector3 newPos = rb.position + dir * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);

      
        transform.LookAt(player.position);
    }

    void AttackPlayer()
    {
        // ���� ��ٿ�� �߻�
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            ShootProjectile();
        }
    }

    void ShootProjectile()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            transform.LookAt(player.position);
            GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            EnemyProjectile ep = proj.GetComponent<EnemyProjectile>();
            if (ep != null)
            {
                Vector3 dir = (player.position - firePoint.position).normalized;
                ep.SetDirection(dir);
            }
        }
    }

    void FleeFromPlayer()
    {
        // �÷��̾�κ��� �־����� ������ ��� (�÷��̾� ��ġ - �� ��ġ)�� ������
        Vector3 fleeDir = (transform.position - player.position).normalized;

        // �̵�
        Vector3 newPos = rb.position + fleeDir * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);

        // �������� ������ �ٶ󺸵��� ȸ��
        if (fleeDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(fleeDir);
            transform.rotation = targetRotation;
        }
    }
}