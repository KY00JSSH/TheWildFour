using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedItem : CountableItem {
    public MedicItemData medicItemData;

   
    public void useMedicine(int amount) {
        //TODO: ���� �� -> PLAYER Ư���� �߰�
        float heal = medicItemData.HealPoint;

        this.useFromStack(amount);
    }
}
