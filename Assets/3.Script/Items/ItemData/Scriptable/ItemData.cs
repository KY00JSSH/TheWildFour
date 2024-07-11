using UnityEngine;

public enum ItemType {
    FOOD,
    EQUIP,
    STUFF,
    MEDIC
}

[CreateAssetMenu(fileName = "ItemData", menuName = "Items/ItemData", order = 1)]
public class ItemData : ScriptableObject {
    [SerializeField] private int key;                       //������ id key
    [SerializeField] private string itemName;               //������ �̸�
    [TextArea(15,20)] 
    [SerializeField] private string description;            //������ ����(���콺 ������ ǥ�ü���)
    [SerializeField] private Sprite icon;                   //������ �κ�â icon
    [SerializeField] private GameObject dropItemPrefab;     //������ ����� object prefab
    [SerializeField] private ItemType type;                 //������ type

    public int Key { get { return key; } }
    public string ItemName { get { return itemName; } }
    public string Description { get { return description; } }
    public Sprite Icon { get { return icon; } }
    public GameObject DropItemPrefab { get { return dropItemPrefab; } }
    public ItemType Type { get { return type; } }
}
