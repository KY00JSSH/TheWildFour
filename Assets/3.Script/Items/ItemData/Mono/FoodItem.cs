using UnityEngine;

public class FoodItem : CountableItem {
    public FoodItemData foodItemData;

    public void useFood(int amount) {
        //TODO: ���� ����+�� -> PLAYER Ư���� �߰�
        float heal = foodItemData.HealPoint;
        float full = foodItemData.FullPoint;

        this.useFromStack(amount);
    }
}