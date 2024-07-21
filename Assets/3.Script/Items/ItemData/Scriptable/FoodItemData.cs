using UnityEngine;

[CreateAssetMenu(fileName = "FoodItemData", menuName = "Items/FoodItemData", order = 3)]
public class FoodItemData : CountableItemData {
    [SerializeField] private float healTime;            //회복 시간
    [SerializeField] private float totalDecayTime;      //전체 부패 시간
    [SerializeField] private bool isRoasted;            //구운 음식인지 여부. 굽지 않고 먹을수 있는 아이템이어도 true

    public float HealTime { get { return healTime; } }
    public float TotalDecayTime { get { return totalDecayTime; } }
    public bool IsRoasted { get { return isRoasted; } }
}
