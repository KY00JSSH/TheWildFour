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
    [SerializeField] private int[] materialKey;         //재료 아이템 key
    [SerializeField] private int[] materialCount;       //재료 아이템 개수

    public float TotalDurability { get { return totalDurability; } }
    public float CurrDurability { get { return currDurability; } }
    public EquipType EqType { get { return eqType; } }
    public int Level { get { return level; } }
    public int[] MaterialKey { get { return materialKey; } }
    public int[] MaterialCount { get { return materialCount; } }

    public void SetCurrDurability(float value) {
        currDurability = value;
    }
}