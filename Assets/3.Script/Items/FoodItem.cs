using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodItem : CountableItem {

    public FoodItem(CountableItemData data, int count = 1) : base(data, count) { }

    public bool Use() {
        CurrCount--;
        return true;
    }

    protected override CountableItem Clone(int count) {
        return new FoodItem(CountableData as FoodItemData, count);
    }
}
