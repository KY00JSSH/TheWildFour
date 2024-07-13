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
    [SerializeField] private float totalDurability;     //전체 내구도
    [SerializeField] private float currDurability;      //현재 내구도
    [SerializeField] private EquipType eqType;          //도구 타입
    [SerializeField] private int level;                 //도구 레벨

    public float TotalDurability { get { return totalDurability; } }
    public float CurrDurability { get { return currDurability; } }
    public EquipType EqType { get { return eqType; } }
    public int Level { get { return level; } }
}