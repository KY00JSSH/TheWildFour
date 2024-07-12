using UnityEngine;

[CreateAssetMenu(fileName = "MedcItemData", menuName = "Items/MedcItemData", order = 5)]
public class MedcItemData : CountableItemData {
    [SerializeField] private float healPoint;        //È¸º¹¾ç

    public float HealPoint { get { return healPoint; } }
}
