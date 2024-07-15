using UnityEngine;

[CreateAssetMenu(fileName = "CountableItemData", menuName = "Items/CountableItemData", order = 2)]
public class CountableItemData : ItemData {
    [SerializeField] private int maxStackCount;  //�ִ� ��ġ�� ����

    public int MaxStackCount { get { return maxStackCount; } }
}
