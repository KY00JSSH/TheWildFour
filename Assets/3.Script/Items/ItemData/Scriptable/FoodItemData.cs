using UnityEngine;

[CreateAssetMenu(fileName = "FoodItemData", menuName = "Items/FoodItemData", order = 3)]
public class FoodItemData : CountableItemData {
    [SerializeField] private float fullPoint;   //������ ����Ʈ
    [SerializeField] private float healPoint;   //ȸ�� ����Ʈ
    [SerializeField] private float totalDecayTime;   //��ü ���� �ð�

    public float FullPoint { get { return fullPoint; } }
    public float HealPoint { get { return healPoint; } }
    public float TotalDecayTime { get { return totalDecayTime; } }
}
