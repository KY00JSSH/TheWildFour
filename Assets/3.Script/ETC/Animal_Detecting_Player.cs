using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal_Detecting_Player : MonoBehaviour
{
    /*
    1. �� ���� ���� �÷��̾��±� ������Ʈ�� ������ ���, �÷��̾��� �ݴ� �������� �پ '������ �Ÿ�'�� �̵�
    2. ���� �þ� ���� ���� �÷��̾��±� ������Ʈ�� ������ ���, �÷��̾��� �ݴ� �������� �پ '������ �Ÿ�'�� �̵�
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
