using UnityEngine;

[CreateAssetMenu(fileName = "FoodItemData", menuName = "Items/FoodItemData", order = 3)]
public class FoodItemData : CountableItemData {
    [SerializeField] private float fullPoint;   //포만감 포인트
    [SerializeField] private float healPoint;   //회복 포인트
    [SerializeField] private float totalDecayTime;   //전체 부패 시간

    public float FullPoint { get { return fullPoint; } }
    public float HealPoint { get { return healPoint; } }
    public float TotalDecayTime { get { return totalDecayTime; } }
}
