using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData", order = 1)]
public class ItemData : ScriptableObject {
    [SerializeField] private int key;                        //아이템 id key
    [SerializeField] private string itemName;                    //아이템 이름
    [SerializeField] private string tooltip;                 //아이템 설명(마우스 오버시 표시설명)
    [SerializeField] private Sprite icon;              //아이템 인벤창 icon
    [SerializeField] private GameObject dropItemPrefab;      //아이템 드랍시 object prefab

    public int Key { get { return key; } }
    public string ItemName { get { return itemName; } }
    public string ToolTip { get { return tooltip; } }
    public Sprite Icon { get { return icon; } }
    public GameObject DropItemPrefab { get { return dropItemPrefab; } }
}
