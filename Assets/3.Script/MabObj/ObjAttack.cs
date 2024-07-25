using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjAttack : MonoBehaviour {

    //돌, 나무일때
    public void GetAttack(float attackPoint, float gatherPoint) {
        //attackPoint 만큼 나무, 돌에 데미지 줌
        //gatherPoint만큼 나무, 돌 떨굼

        if(gameObject.GetComponent<RockController>() != null){
            RockController rockCont = gameObject.GetComponent<RockController>();
            rockCont.getDamage(attackPoint);
            //gatherPoint만큼 아이템 드랍
        }
        else if(gameObject.GetComponent<TreeBigController>() != null) {
            TreeBigController treeCont = gameObject.GetComponent<TreeBigController>();
            treeCont.getDamage(attackPoint);
            //gatherPoint만큼 아이템 드랍
        }
    }
}
