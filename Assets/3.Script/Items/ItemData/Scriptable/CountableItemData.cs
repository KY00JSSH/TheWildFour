using UnityEngine;

[CreateAssetMenu(fileName = "CountableItemData", menuName = "Items/CountableItemData", order = 2)]
public class CountableItemData : ItemData {
    [SerializeField] private int maxStackCount;  //최대 겹치는 개수
    [SerializeField] private int currStackCount;  //현재 겹치고있는 개수

    public int MaxStackCount { get { return maxStackCount; } }
    public int CurrStackCount { get { return currStackCount; } }

    public void addCurrStack(int num) {
        currStackCount += num;

        if(currStackCount > maxStackCount) {
            int over = currStackCount - maxStackCount;
            currStackCount = maxStackCount;
            Debug.Log($"over num : {over}");
        }
    }

    public void resetCurrStack() {
        currStackCount = 0;
    }

    public void useCurrStack(int num) {
        currStackCount -= num;
        if (currStackCount <= 0) {
            Destroy(this);
        }
    }
}
