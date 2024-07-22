public class EquipItem : Item {
    public EquipItemData equipItemData;

    private float currDurability;           //현재 내구도
    public float CurrDurability { get { return currDurability; } }

    //내구도 0이라 파괴 확인
    private void CheckDurability() {
        if (currDurability <= 0) {
            Destroy(gameObject);
        }
    }

    //도구 사용시 내구도 삭제
    public void UseItem(float amount) {
        float thisDurab = currDurability - amount;
        SetCurrDurability(thisDurab);
        CheckDurability();
    }

    //내구도 전체 수리
    public void ResetDurability() {
        SetCurrDurability(equipItemData.TotalDurability);
    }
    public void SetCurrDurability(float value) {
        currDurability = value;
    }
}
