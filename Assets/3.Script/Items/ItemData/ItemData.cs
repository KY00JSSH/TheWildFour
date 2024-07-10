using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData", order = 1)]
public class ItemData : ScriptableObject {
    [SerializeField] private int key;                        //������ id key
    [SerializeField] private string itemName;                    //������ �̸�
    [SerializeField] private string description;                 //������ ����(���콺 ������ ǥ�ü���)
    [SerializeField] private Sprite icon;              //������ �κ�â icon
    [SerializeField] private GameObject dropItemPrefab;      //������ ����� object prefab

    public int Key { get { return key; } }
    public string ItemName { get { return itemName; } }
    public string Description { get { return description; } }
    public Sprite Icon { get { return icon; } }
    public GameObject DropItemPrefab { get { return dropItemPrefab; } }
}
