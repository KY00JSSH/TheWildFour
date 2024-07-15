using UnityEngine;

public class FoodItem : CountableItem {
    public FoodItemData foodItemData;
    private float currDecayTime = 0;   //���� ���� �ð�
    public float CurrDecayTime { get { return currDecayTime; } }

    public float HealTime => foodItemData.HealTime;
}