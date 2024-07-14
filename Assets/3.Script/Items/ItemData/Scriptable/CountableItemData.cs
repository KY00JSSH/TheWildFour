using UnityEngine;

[CreateAssetMenu(fileName = "CountableItemData", menuName = "Items/CountableItemData", order = 2)]
public class CountableItemData : ItemData {
    [SerializeField] private int maxStackCount;  //�ִ� ��ġ�� ����
    [SerializeField] private int currStackCount;  //���� ��ġ���ִ� ����

    public int MaxStackCount { get { return maxStackCount; } }
    public int CurrStackCount { get { return currStackCount; } }

    public void addCurrStack(int num) {
        if ((currStackCount + num) <= maxStackCount) {
            currStackCount += num;
        }
        else {
            int leftItemCount = (currStackCount + num) - maxStackCount;
            currStackCount = maxStackCount;
            Debug.Log($"�߰� ���� item Count : {leftItemCount}");
            //TODO: �߰����� ITEM COUNT -> �� INVENTORY �������� Ȥ�� �ٴڿ� ������ üũ
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
