using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipItem : Item {
    public EquipItemData equipItemData;

    //내구도 0이라 파괴 확인
    private void CheckDurability() {
        if (equipItemData.CurrDurability <= 0) {
            Destroy(gameObject);
        }
    }

    //도구 사용시 내구도 삭제
    public void UseItem(float amount) {
        float thisDurab = equipItemData.CurrDurability - amount;
        equipItemData.SetCurrDurability(thisDurab);
        CheckDurability();
    }

    //내구도 전체 수리
    public void ResetDurability() {
        equipItemData.SetCurrDurability(equipItemData.TotalDurability);
    }
}
