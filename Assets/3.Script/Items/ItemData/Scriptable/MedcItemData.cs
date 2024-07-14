using UnityEngine;

[CreateAssetMenu(fileName = "MedcItemData", menuName = "Items/MedcItemData", order = 5)]
public class MedcItemData : CountableItemData {
    [SerializeField] private float healPoint;       //ȸ����
    [SerializeField] private int[] materialKey;     //��� ������ key
    [SerializeField] private int[] materialCount;   //��� ������ ����

    public float HealPoint { get { return healPoint; } }
    public int[] MaterialKey { get { return materialKey; } }
    public int[] MaterialCount { get { return materialCount; } }
}
