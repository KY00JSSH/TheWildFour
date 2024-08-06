using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnmlAttack : MonoBehaviour {
    public void GetAttack(float attackPoint) {
        //attackPoint 만큼 동물에게 피해 입힘
        if(gameObject.GetComponent<Animal_Control>() != null) {
            Animal_Control animalCont = gameObject.GetComponent<Animal_Control>();
            animalCont.getDamage(attackPoint);
        }
        
    }
}
