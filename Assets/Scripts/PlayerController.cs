using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    //Freelook��� true �Ͻ� �������� ��Ȱ��ȭ ��ų �Լ�
    public CinemachineSwitcher cinemachineSwitcher;

    public float jumpPower = 5f;
    public float gravity = -9.81f;
    public CinemachineVirtualCamera virtualCam;
    public float rotationSpeed = 10f;
    private CinemachinePOV pov;
    //�뽬 UI bool
    public GameObject shiftUI;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;



    public int maxHP = 100;
    private int currentHP;

    public Slider hpSlider;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        pov = virtualCam.GetCinemachineComponent<CinemachinePOV>();

        currentHP = maxHP;
        hpSlider.value = 1f;

        //�뽬 UI�� ���۽� ��Ȱ��ȭ
        if (shiftUI != null)
        {
            shiftUI.SetActive(false);
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            pov.m_HorizontalAxis.Value = transform.eulerAngles.y;
            pov.m_VerticalAxis.Value = 0f;
        }
        //Freelook��� true�Ͻ� ��� ������ �ڵ带 ����
        if (cinemachineSwitcher != null)
        {
            if (cinemachineSwitcher.usingFreeLook)
            {
                return;
            }
        }

        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 camForward = virtualCam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = virtualCam.transform.right;
        camRight.y = 0;
        camRight.Normalize();


        Vector3 move = (camForward * z + camRight * x).normalized;
        controller.Move(move * speed * Time.deltaTime);

        float cameraYaw = pov.m_HorizontalAxis.Value;
        Quaternion targetRot = Quaternion.Euler(0f, cameraYaw, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);


        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = jumpPower;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);


        //ShiftŰ ���� �ڵ� (�ӵ�, UI �߰�)
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 10f;

            //shift�� ������ �� �뽬 UI Ȱ��ȭ
            if (shiftUI != null)
            {
                shiftUI.SetActive(true);
            }
        }
        else
        {
            speed = 5f;

            //shift�� ���� �� �뽬 UI ��Ȱ��ȭ
            if (shiftUI != null)
            {
                shiftUI.SetActive(false);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        hpSlider.value = (float)currentHP / maxHP;

        if (currentHP <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }
  
}
