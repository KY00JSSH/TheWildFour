using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenController : MonoBehaviour {
    private bool isInvenFull = false;               //인벤토리 전체 차있고 기존에도 추가 못함 여부
    public bool IsInvenFull { get { return isInvenFull; } }
    private List<ItemData> inventory;

    private InvenUIController invenUi;
    private GameObject itemObejct;

    public bool itemTest = false;
    public ItemData testItem;

    public delegate void OnInventoryChanged(List<ItemData> inventory);
    public event OnInventoryChanged InventoryChanged;

    private void Start() {
        inventory = new List<ItemData>();
        invenUi = FindObjectOfType<InvenUIController>();
    }

    public void invenFullReset() {
        isInvenFull = false;
    }

    private void Update() {
        if(itemTest) {
            if(testItem is CountableItemData countItem) {
                //TODO: 카운팅 체크...해야해
                countItem.resetCurrStack();
                countItem.addCurrStack(3);
                itemObejct = Instantiate(testItem.DropItemPrefab);
                ItemAdd();
            }

            itemTest = false;
            Debug.Log(inventory[0].ItemName);
            Debug.Log(inventory[0].Key);
        }
    }

    //if 해당 아이템이 inven에 있고,(해당 box item count < itemMaxStackCount)
    // 해당 칸에 아이템 추가
    //  else if(!full)  새박스에 아이템 add
    //else isInvenFull = true
    public void ItemAdd() {
        Item item = itemObejct.GetComponent<Item>();

        int checkNum = canAddThisBox(item.itemData.Key);
        if (checkNum != 99) {
            //해당칸에 아이템 추가
            if (item.itemData is CountableItemData countItem &&
                inventory[checkNum] is CountableItemData newCountItem) {
                newCountItem.addCurrStack(countItem.CurrStackCount);
            }
        }
        else {
            int existBox = isExistEmptyBox();
            if (existBox == 16) {
                //새 박스에 아이템 add
                inventory.Add(item.itemData);
            }
            else if (existBox != 99) {
                //null로 비워둔 inventory에 추가
                inventory[existBox] = item.itemData;
            }
            else {
                isInvenFull = true;
                Debug.Log("전체 차있고, 기존 box에도 추가 못함");
            }
        }
        InventoryChanged?.Invoke(inventory);
    }

    //현재 박스에 해당 item이 있고, 해당 칸에 추가 가능할 때 해당 칸 num을 return, 없을때 99 return
    private int canAddThisBox(int itemKey) {
        for (int i = 0; i < invenUi.CurrInvenCount; i++) {
            if (i < inventory.Count && inventory[i].Key == itemKey) {
                if (inventory[i].Key == itemKey) {
                    if (inventory[i] is CountableItemData countItem) {
                        if (countItem.CurrStackCount < countItem.MaxStackCount) {
                            return i;
                        }
                    }
                }
            }
        }
        return 99;
    }

    //빈 inven box가 있는지 여부
    private int isExistEmptyBox() {
        if (inventory.Count < invenUi.CurrInvenCount) {
            return 16;  //아예 생성도 안한 inven이 있으면 16으로 return
        }
        else {
            for (int i = 0; i < inventory.Count; i++) {
                if (inventory[i] == null) {
                    //기존에 생성했지만 null로 초기화 한 inventory일때는 해당 index return
                    return i;
                }
            }
            return 99;  //아예 빈 박스를 사용못할때
        }
    }

    private void removeItem(int index) {
        if (index >= 0 && index < inventory.Count) {
            inventory[index] = null;
        }
        InventoryChanged?.Invoke(inventory);
    }

    //아이템 1개 사용
    public void useItem(int index) {
        if (index >= 0 && index < inventory.Count && inventory[index] != null) {
            if (inventory[index] is CountableItemData countItem) {
                if (countItem.CurrStackCount == 1) {
                    removeItem(index);
                }
                else {
                    countItem.useCurrStack(1);
                }
            }
            else {
                removeItem(index);
            }
        }
        InventoryChanged?.Invoke(inventory);
    }


    private void dropItem(int index) {
        if (index >= 0 && index < inventory.Count && inventory[index] != null) {
            if (inventory[index].Key != 1 && inventory[index].Key != 2) {
                //돌, 나무 아닐때
                if (inventory[index] is CountableItemData countItem) {
                    countItem.useCurrStack(1);
                    //TODO: 아이템 드랍
                }
            }
            else {
                if (inventory[index] is CountableItemData countItem) {
                    if (countItem.CurrStackCount > 8) {
                        countItem.useCurrStack(8);
                        //TODO: 아이템 드랍
                    }
                    else if (countItem.CurrStackCount == 8) {
                        countItem.useCurrStack(8);
                        removeItem(index);
                        //TODO: 아이템 드랍
                    }
                    else {
                        countItem.useCurrStack(countItem.CurrStackCount);
                        //TODO: 아이템 드랍

                        removeItem(index);
                    }
                }
            }
        }
        InventoryChanged?.Invoke(inventory);
    }
}