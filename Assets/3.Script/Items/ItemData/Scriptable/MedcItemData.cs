using UnityEngine;

[CreateAssetMenu(fileName = "MedcItemData", menuName = "Items/MedcItemData", order = 5)]
public class MedcItemData : ItemData {
    [SerializeField] private float healPoint;        //ȸ����

    public float HealPoint { get { return healPoint; } }
}
