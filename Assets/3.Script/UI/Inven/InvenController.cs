using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenController : MonoBehaviour {
    private bool isInvenFull = false;               //�κ��丮 ��ü ���ְ� �������� �߰� ���� ����
    public bool IsInvenFull { get { return isInvenFull; } }
    private List<Item> inventory;

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

    private void Update() {
        //if(itemTest) {
        //    if(testItem is CountableItemData countItem) {
        //        //TODO: ī���� üũ...�ؾ���
        //        countItem.resetCurrStack();
        //        countItem.addCurrStack(3);
        //        itemObejct = Instantiate(testItem.DropItemPrefab);
        //        ItemAdd();
        //    }

        //    itemTest = false;
        //    Debug.Log(inventory[0].itemData.ItemName);
        //    Debug.Log(inventory[0].itemData.Key);
        //}
    }

    //if �ش� �������� inven�� �ְ�,(�ش� box item count < itemMaxStackCount)
    // �ش� ĭ�� ������ �߰�
    //  else if(!full)  ���ڽ��� ������ add
    //else isInvenFull = true
    public void ItemAdd() {
        Item item = itemObejct.GetComponent<Item>();

        int checkNum = canAddThisBox(item.Key);
        if (checkNum != 99) {
            //�ش�ĭ�� ������ �߰�
            if (item is CountableItem countItem &&
                inventory[checkNum] is CountableItem newCountItem) {
                newCountItem.addToStack(countItem.CurrentCount);
            }
        }
        else {
            int existBox = isExistEmptyBox();
            if (existBox == 16) {
                //�� �ڽ��� ������ add
                inventory.Add(item);
            }
            else if (existBox != 99) {
                //null�� ����� inventory�� �߰�
                inventory[existBox] = item;
            }
            else {
                isInvenFull = true;
                Debug.Log("��ü ���ְ�, ���� box���� �߰� ����");
            }
        }

        for (int i = 0; i < inventory.Count; i++) {
            if(inventory[i] is CountableItem countItem) {
                Debug.Log($"{i} > {countItem.itemData.ItemName} : {countItem.countableData.CurrStackCount}");
            }
        }
        InvenChanged?.Invoke(inventory);
    }

    //���� �ڽ��� �ش� item�� �ְ�, �ش� ĭ�� �߰� ������ �� �ش� ĭ num�� return, ������ 99 return
    private int canAddThisBox(int itemKey) {
        for (int i = 0; i < invenUi.CurrInvenCount; i++) {
            if (i < inventory.Count && inventory[i].itemData.Key == itemKey) {
                if (inventory[i].Key == itemKey) {
                    if (inventory[i] is CountableItem countItem) {
                        if (countItem.CurrentCount < countItem.MaxStackCount) {
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
        InvenChanged?.Invoke(new List<Item>(inventory));
    }

    //������ 1�� ���
    public void useItem(int index) {
        if (index >= 0 && index < inventory.Count && inventory[index] != null) {
            if (inventory[index] is CountableItem countItem) {
                if (countItem.CurrentCount == 1) {
                    removeItem(index);
                }
                else {
                    countItem.useFromStack(1);
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
                //��, ���� �ƴҶ�
                if (inventory[index] is CountableItem countItem) {
                    countItem.useFromStack(1);
                    //TODO: ������ ���
                }
            }
            else {
                if (inventory[index] is CountableItem countItem) {
                    if (countItem.CurrentCount > 8) {
                        countItem.useFromStack(8);
                        //TODO: ������ ���
                    }
                    else if (countItem.CurrentCount == 8) {
                        countItem.useFromStack(8);
                        removeItem(index);
                        //TODO: ������ ���
                    }
                    else {
                        countItem.useFromStack(countItem.CurrentCount);
                        //TODO: ������ ���

                        removeItem(index);
                    }
                }
            }
        }
        InvenChanged?.Invoke(inventory);
    }
}