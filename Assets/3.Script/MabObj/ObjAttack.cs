using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjAttack : MonoBehaviour {

    //��, �����϶�
    public void GetAttack(float attackPoint, float gatherPoint) {
        //attackPoint ��ŭ ����, ���� ������ ��
        //gatherPoint��ŭ ����, �� ����

        if(gameObject.GetComponent<RockController>() != null){
            RockController rockCont = gameObject.GetComponent<RockController>();
            rockCont.getDamage(attackPoint);
            //gatherPoint��ŭ ������ ���
            rockCont.dropRockItem(gatherPoint);
        }
        else if(gameObject.GetComponent<TreeBigController>() != null) {
            TreeBigController treeCont = gameObject.GetComponent<TreeBigController>();
            treeCont.getDamage(attackPoint);
            //gatherPoint��ŭ ������ ���
            treeCont .dropTreeItem(gatherPoint);
        }
    }
}
