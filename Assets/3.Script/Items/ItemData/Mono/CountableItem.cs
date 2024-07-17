using UnityEngine;

public class CountableItem : Item {
    public CountableItemData countableData;

    private int currStackCount = 1;  //현재 겹치고있는 개수
    public int CurrStackCount { get { return currStackCount; } }

    public bool IsMax => CurrStackCount >= countableData.MaxStackCount;

    public int MaxStackCount => countableData.MaxStackCount;

    public void addCurrStack(int num) {
        currStackCount += num;

        if (currStackCount > MaxStackCount) {
            int over = currStackCount - MaxStackCount;
            currStackCount = MaxStackCount;
        }
    }

    public void resetCurrStack() {
        currStackCount = 1;
    }

    public void useCurrStack(int num) {
        currStackCount -= num;
    }
}