using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal_Detecting_Player : MonoBehaviour
{
    /*
    1. 원 범위 내에 플레이어태그 오브젝트가 감지될 경우, 플레이어의 반대 방향으로 뛰어서 '일정한 거리'를 이동
    2. 전방 시야 범위 내에 플레이어태그 오브젝트가 감지될 경우, 플레이어의 반대 방향으로 뛰어서 '일정한 거리'를 이동
    */

    public Animal_Control parentControl;

    private void Awake()
    {
        parentControl = GetComponentInParent<Animal_Control>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        { 
            parentControl.PlayerDetected();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            parentControl.PlayerLost();
        }
    }
}
