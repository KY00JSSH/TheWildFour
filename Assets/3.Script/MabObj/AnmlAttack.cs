using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnmlAttack : MonoBehaviour
{
    public void GetAttack(float attackPoint) {
        //attackPoint ��ŭ �������� ���� ����

        if (gameObject.GetComponent<RockController>() != null) {
            RockController rockCont = gameObject.GetComponent<RockController>();
        }
    }
}
