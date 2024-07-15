using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountableItem : Item {
    public CountableItemData countableData;

    public int CurrentCount => countableData.CurrStackCount;

    public bool IsMax => countableData.CurrStackCount >= countableData.MaxStackCount;

    public int MaxStackCount => countableData.MaxStackCount;

    public void addToStack(int amount) {
        countableData.addCurrStack(amount);
    }

    public void useFromStack(int amount) {
        countableData.useCurrStack(amount);
    }

    public void resetStack() {
        countableData.resetCurrStack();
    }
}