using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyState { Idle, Trace, Attack, Flee } // Flee 상태 추가
    public EnemyState state = EnemyState.Idle;

    public float moveSpeed = 2f;
    public float traceRange = 15f; // 추적 시작 거리
    public float attackRange = 6f; // 공격 시작 거리
    public float attackCooldown = 1.5f;

    public GameObject projectilePrefab; // 투사체 프리팹
    public Transform firePoint; // 발사 위치

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
            // 아직 도망 상태가 아니라면 상태 전환
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
        float fleeThreshold = maxHP * 0.2f; // 20% HP 임계치

        //  이미 도망 상태이거나, HP가 20% 이하면 다른 상태로 넘어가지 않고 도망 유지
        if (currentHP > 0 && currentHP <= fleeThreshold) 
        {
            state = EnemyState.Flee;
        }

        // FSM 상태 전환
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

            case EnemyState.Flee: // Flee 상태 추가
                break;
        }
    }

    void FixedUpdate()
    {
        if (rb == null || player == null) return;

        // 현재 상태에 따라 적절한 물리 이동 함수 호출
        switch (state)
        {
            case EnemyState.Trace:
                TracePlayer();
                break;

            case EnemyState.Flee:
                FleeFromPlayer();
                break;

            default:
                // Idle/Attack 상태에서는 이동 로직을 호출하지 않는다.
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
        // 일정 쿨다운마다 발사
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
        // 플레이어로부터 멀어지는 방향을 계산 (플레이어 위치 - 내 위치)의 역방향
        Vector3 fleeDir = (transform.position - player.position).normalized;

        // 이동
        Vector3 newPos = rb.position + fleeDir * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);

        // 도망가는 방향을 바라보도록 회전
        if (fleeDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(fleeDir);
            transform.rotation = targetRotation;
        }
    }
}