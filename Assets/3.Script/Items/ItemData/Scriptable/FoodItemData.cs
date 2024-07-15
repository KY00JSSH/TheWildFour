using UnityEngine;

[CreateAssetMenu(fileName = "FoodItemData", menuName = "Items/FoodItemData", order = 3)]
public class FoodItemData : CountableItemData {
    [SerializeField] private float healTime;   //회복 시간
    [SerializeField] private float totalDecayTime;   //전체 부패 시간

    public float HealTime { get { return healTime; } }
    public float TotalDecayTime { get { return totalDecayTime; } }
}
