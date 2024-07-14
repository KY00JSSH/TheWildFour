using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenController : MonoBehaviour {
    private bool isInvenFull = false;               //�κ��丮 ��ü ���ְ� �������� �߰� ���� ����
    public bool IsInvenFull { get { return isInvenFull; } }
    private List<ItemData> inventory;
    public List<ItemData> Inventory { get { return inventory; } }
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
                //TODO: ī���� üũ...�ؾ���
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

    //if �ش� �������� inven�� �ְ�,(�ش� box item count < itemMaxStackCount)
    // �ش� ĭ�� ������ �߰�
    //  else if(!full)  ���ڽ��� ������ add
    //else isInvenFull = true
    public void ItemAdd() {
        Item item = itemObejct.GetComponent<Item>();

        int checkNum = canAddThisBox(item.itemData.Key);
        if (checkNum != 99) {
            //�ش�ĭ�� ������ �߰�
            if (item.itemData is CountableItemData countItem &&
                inventory[checkNum] is CountableItemData newCountItem) {
                newCountItem.addCurrStack(countItem.CurrStackCount);
            }
        }
        else {
            int existBox = isExistEmptyBox();
            if (existBox == 16) {
                //�� �ڽ��� ������ add
                inventory.Add(item.itemData);
            }
            else if (existBox != 99) {
                //null�� ����� inventory�� �߰�
                inventory[existBox] = item.itemData;
            }
            else {
                isInvenFull = true;
                Debug.Log("��ü ���ְ�, ���� box���� �߰� ����");
            }
        }
        InventoryChanged?.Invoke(inventory);
    }

    //���� �ڽ��� �ش� item�� �ְ�, �ش� ĭ�� �߰� ������ �� �ش� ĭ num�� return, ������ 99 return
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

    //�� inven box�� �ִ��� ����
    private int isExistEmptyBox() {
        if (inventory.Count < invenUi.CurrInvenCount) {
            return 16;  //�ƿ� ������ ���� inven�� ������ 16���� return
        }
        else {
            for (int i = 0; i < inventory.Count; i++) {
                if (inventory[i] == null) {
                    //������ ���������� null�� �ʱ�ȭ �� inventory�϶��� �ش� index return
                    return i;
                }
            }
            return 99;  //�ƿ� �� �ڽ��� �����Ҷ�
        }
    }

    private void removeItem(int index) {
        if (index >= 0 && index < inventory.Count) {
            inventory[index] = null;
        }
        InventoryChanged?.Invoke(inventory);
    }

    //������ 1�� ���
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
                //��, ���� �ƴҶ�
                if (inventory[index] is CountableItemData countItem) {
                    countItem.useCurrStack(1);
                    //TODO: ������ ���
                }
            }
            else {
                if (inventory[index] is CountableItemData countItem) {
                    if (countItem.CurrStackCount > 8) {
                        countItem.useCurrStack(8);
                        //TODO: ������ ���
                    }
                    else if (countItem.CurrStackCount == 8) {
                        countItem.useCurrStack(8);
                        removeItem(index);
                        //TODO: ������ ���
                    }
                    else {
                        countItem.useCurrStack(countItem.CurrStackCount);
                        //TODO: ������ ���

                        removeItem(index);
                    }
                }
            }
        }
        InventoryChanged?.Invoke(inventory);
    }
}