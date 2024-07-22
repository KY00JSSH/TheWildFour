public class EquipItem : Item {
    public EquipItemData equipItemData;

    private float currDurability;           //���� ������
    public float CurrDurability { get { return currDurability; } }

    //������ 0�̶� �ı� Ȯ��
    private void CheckDurability() {
        if (currDurability <= 0) {
            Destroy(gameObject);
        }
    }

    //���� ���� ������ ����
    public void UseItem(float amount) {
        float thisDurab = currDurability - amount;
        SetCurrDurability(thisDurab);
        CheckDurability();
    }

    //������ ��ü ����
    public void ResetDurability() {
        SetCurrDurability(equipItemData.TotalDurability);
    }
    public void SetCurrDurability(float value) {
        currDurability = value;
    }
}
