using UnityEngine;

public class FoodItem : CountableItem {
    public FoodItemData foodItemData;

    public void useFood(int amount) {
        //TODO: 사용시 포만+힐 -> PLAYER 특성에 추가
        float heal = foodItemData.HealPoint;
        float full = foodItemData.FullPoint;

        this.useFromStack(amount);
    }
}