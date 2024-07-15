using UnityEngine;

[CreateAssetMenu(fileName = "CountableItemData", menuName = "Items/CountableItemData", order = 2)]
public class CountableItemData : ItemData {
    [SerializeField] private int maxStackCount;  //�ִ� ��ġ�� ����
    [SerializeField] private int currStackCount;  //���� ��ġ���ִ� ����

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
