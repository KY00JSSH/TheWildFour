using UnityEngine;

public enum EquipType {
    PICK,
    AXE,
    SHIRT,
    SHOO,
    WEAP
}

[CreateAssetMenu(fileName = "EquipItemData", menuName = "Items/EquipItemData", order = 4)]
public class EquipItemData : ItemData {
    [SerializeField] private float totalDurability;     //��ü ������
    [SerializeField] private float currDurability;      //���� ������
    [SerializeField] private EquipType eqType;          //���� Ÿ��
    [SerializeField] private int level;                 //���� ����

    public float TotalDurability { get { return totalDurability; } }
    public float CurrDurability { get { return currDurability; } }
    public EquipType EqType { get { return eqType; } }
    public int Level { get { return level; } }
}