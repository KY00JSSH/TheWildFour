using UnityEngine;

[CreateAssetMenu(fileName = "MedcItemData", menuName = "Items/MedcItemData", order = 5)]
public class MedcItemData : CountableItemData {
    [SerializeField] private float healPoint;       //회복양
    [SerializeField] private int[] materialKey;     //재료 아이템 key
    [SerializeField] private int[] materialCount;   //재료 아이템 개수

    public float HealPoint { get { return healPoint; } }
    public int[] MaterialKey { get { return materialKey; } }
    public int[] MaterialCount { get { return materialCount; } }
}
