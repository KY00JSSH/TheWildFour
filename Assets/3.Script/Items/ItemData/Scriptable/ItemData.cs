using UnityEngine;

public enum ItemType {
    FOOD,
    EQUIP,
    STUFF,
    MEDIC
}

[CreateAssetMenu(fileName = "ItemData", menuName = "Items/ItemData", order = 1)]
public class ItemData : ScriptableObject {
    [SerializeField] private int key;                       //아이템 id key
    [SerializeField] private string itemName;               //아이템 이름
    [TextArea(15,20)] 
    [SerializeField] private string description;            //아이템 설명(마우스 오버시 표시설명)
    [SerializeField] private Sprite icon;                   //아이템 인벤창 icon
    [SerializeField] private GameObject dropItemPrefab;     //아이템 드랍시 object prefab
    [SerializeField] private ItemType type;                 //아이템 type

    public int Key { get { return key; } }
    public string ItemName { get { return itemName; } }
    public string Description { get { return description; } }
    public Sprite Icon { get { return icon; } }
    public GameObject DropItemPrefab { get { return dropItemPrefab; } }
    public ItemType Type { get { return type; } }
}
