using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineSwitcher : MonoBehaviour
{

    public CinemachineVirtualCamera virtualCam;
    public CinemachineFreeLook freeLookCam;
    public bool usingFreeLook = false;
    //SetFreeLookState�� �Լ��� ���� PlayerController�� usingFreeLook ���� Ȯ�ΰ���
    public void SetFreeLookState(bool state)
    {
        usingFreeLook = state;
    }

    void Start()
    {
        virtualCam.Priority = 10;
        freeLookCam.Priority = 0;
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            usingFreeLook = !usingFreeLook;
            if (usingFreeLook)
            {
                freeLookCam.Priority = 20;
                virtualCam.Priority = 0;
            }
            else
            {
                virtualCam.Priority = 20;
                freeLookCam.Priority = 0;
            }
        }
    }
}
