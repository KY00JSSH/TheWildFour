using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedItem : CountableItem {
    public MedicItemData medicItemData;

   
    public void useMedicine(int amount) {
        //TODO: 사용시 힐 -> PLAYER 특성에 추가
        float heal = medicItemData.HealPoint;

        this.useFromStack(amount);
    }
}
