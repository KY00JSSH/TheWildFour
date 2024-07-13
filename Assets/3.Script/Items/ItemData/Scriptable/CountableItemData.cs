using UnityEngine;

[CreateAssetMenu(fileName = "CountableItemData", menuName = "Items/CountableItemData", order = 2)]
public class CountableItemData : ItemData {
    [SerializeField] private int maxStackCount;  //최대 겹치는 개수
    [SerializeField] private int currStackCount;  //현재 겹치고있는 개수

    public int MaxStackCount { get { return maxStackCount; } }
    public int CurrStackCount { get { return currStackCount; } }

    public void addCurrStack(int num) {
        if ((currStackCount + num) <= maxStackCount) {
            currStackCount += num;
        }
        else {
            int leftItemCount = (currStackCount + num) - maxStackCount;
            currStackCount = maxStackCount;
            Debug.Log($"추가 못한 item Count : {leftItemCount}");
            //TODO: 추가못한 ITEM COUNT -> 새 INVENTORY 차지할지 혹은 바닥에 버릴지 체크
        }
    }

    public void resetCurrStack() {
        currStackCount = 0;
    }

    public void useCurrStack(int num) {
        if (!(currStackCount - num < 0)) {
            currStackCount -= num;
        }
    }
}
