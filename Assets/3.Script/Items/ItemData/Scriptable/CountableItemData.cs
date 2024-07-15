using UnityEngine;

[CreateAssetMenu(fileName = "CountableItemData", menuName = "Items/CountableItemData", order = 2)]
public class CountableItemData : ItemData {
    [SerializeField] private int maxStackCount;  //최대 겹치는 개수

    public int MaxStackCount { get { return maxStackCount; } }
}
