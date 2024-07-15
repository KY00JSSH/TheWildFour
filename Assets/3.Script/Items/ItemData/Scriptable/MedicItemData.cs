using UnityEngine;

[CreateAssetMenu(fileName = "MedcItemData", menuName = "Items/MedcItemData", order = 5)]
public class MedicItemData : CountableItemData {
    [SerializeField] private float healTime;   //회복 시간
    [SerializeField] private int[] materialKey;     //재료 아이템 key
    [SerializeField] private int[] materialCount;   //재료 아이템 개수

    public float HealTime { get { return healTime; } }
    public int[] MaterialKey { get { return materialKey; } }
    public int[] MaterialCount { get { return materialCount; } }
}
