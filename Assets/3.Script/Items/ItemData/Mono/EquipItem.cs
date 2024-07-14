using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipItem : Item {
    public EquipItemData equipItemData;

    //������ 0�̶� �ı� Ȯ��
    private void CheckDurability() {
        if (equipItemData.CurrDurability <= 0) {
            Destroy(gameObject);
        }
    }

    //���� ���� ������ ����
    public void UseItem(float amount) {
        float thisDurab = equipItemData.CurrDurability - amount;
        equipItemData.SetCurrDurability(thisDurab);
        CheckDurability();
    }

    //������ ��ü ����
    public void ResetDurability() {
        equipItemData.SetCurrDurability(equipItemData.TotalDurability);
    }
}
