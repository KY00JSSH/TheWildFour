using UnityEngine;

public class FoodItem : CountableItem {
    public FoodItemData foodItemData;
    private float currDecayTime = 0;   //현재 부패 시간
    public float CurrDecayTime { get { return currDecayTime; } }

    public float HealTime => foodItemData.HealTime;
}