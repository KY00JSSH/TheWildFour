using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenController : MonoBehaviour {
    private bool isInvenFull = false;               //인벤토리 전체 차있고 기존에도 추가 못함 여부
    public bool IsInvenFull { get { return isInvenFull; } }
    private List<Item> inventory; 
    public List<Item> Inventory { get { return inventory; } }

    private InvenUIController invenUi;
    public GameObject itemObejct;

    public bool itemTest = false;
    public ItemData testItem;

    public delegate void OnInvenChanged(List<Item> inventory);
    public event OnInvenChanged InvenChanged;

    private void Start() {
        inventory = new List<Item>();
        invenUi = FindObjectOfType<InvenUIController>();
    }

    public void invenFullReset() {
        isInvenFull = false;
    }

    //if 해당 아이템이 inven에 있고,(해당 box item count < itemMaxStackCount)
    // 해당 칸에 아이템 추가
    //  else if(!full)  새박스에 아이템 add
    //else isInvenFull = true
    public void ItemAdd() {
        Item item = itemObejct.GetComponent<Item>();

        int checkNum = canAddThisBox(item.Key);

        if (checkNum == 16) {
            inventory.Add(item);
        }
        else if (checkNum != 99) {
            //해당칸에 아이템 추가
            if (item is CountableItem countItem &&
                inventory[checkNum] is CountableItem newCountItem) {
                newCountItem.addCurrStack(countItem.CurrStackCount);
            }
        }
        else {
            int existBox = isExistEmptyBox();
            if (existBox == 17) {
                //새 박스에 아이템 add
                inventory.Add(item);
            }
            else if (existBox != 99) {
                //null로 비워둔 inventory에 추가
                inventory[existBox] = item;
            }
            else {
                isInvenFull = true;
                Debug.Log("전체 차있고, 기존 box에도 추가 못함");
            }
        }

        InvenChanged?.Invoke(inventory);
    }

    //현재 박스에 해당 item이 있고, 해당 칸에 추가 가능할 때 해당 칸 num을 return, 없을때 99 return
    private int canAddThisBox(int itemKey) {
        if (inventory.Count == 0) {
            return 16;
        }
        else {
            for (int i = 0; i < inventory.Count; i++) {
                if (inventory[i].Key == itemKey) {
                    if (inventory[i] is CountableItem countItem) {
                        if (countItem.CurrStackCount < countItem.MaxStackCount) {
                            return i;
                        }
                    }
                }
            }
            return 99;
        }
    }

    //빈 inven box가 있는지 여부
    private int isExistEmptyBox() {
        if (inventory.Count < invenUi.CurrInvenCount) {
            return 17;  //아예 생성도 안한 inven이 있으면 17으로 return
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
        InvenChanged?.Invoke(inventory);
    }

    //아이템 1개 사용
    public void useItem(int index) {
        if (index >= 0 && index < inventory.Count && inventory[index] != null) {
            if (inventory[index] is CountableItem countItem) {
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
        InvenChanged?.Invoke(inventory);
    }

    private void dropItem(int index) {
        if (index >= 0 && index < inventory.Count && inventory[index] != null) {
            if (inventory[index].itemData.Key != 1 && inventory[index].itemData.Key != 2) {
                //돌, 나무 아닐때
                if (inventory[index] is CountableItem countItem) {
                    countItem.useCurrStack(1);
                    //TODO: 아이템 드랍
                }
            }
            else {
                if (inventory[index] is CountableItem countItem) {
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
        InvenChanged?.Invoke(inventory);
    }
}