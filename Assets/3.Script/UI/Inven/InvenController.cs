using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenController : MonoBehaviour {
    private bool isInvenFull = false;               //�κ��丮 ��ü ���ְ� �������� �߰� ���� ����
    public bool IsInvenFull { get { return isInvenFull; } }
    private List<ItemData> inventory;

    private InvenUIController invenUi;
    private GameObject itemObejct;

    private void Start() {
        inventory = new List<ItemData>();
        invenUi = FindObjectOfType<InvenUIController>();
    }

    public void invenFullReset() {
        isInvenFull = false;
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
    }

    //���� �ڽ��� �ش� item�� �ְ�, �ش� ĭ�� �߰� ������ �� �ش� ĭ num�� return, ������ 99 return
    private int canAddThisBox(int itemKey) {
        for (int i = 0; i < invenUi.CurrInvenCount; i++) {
            if (inventory[i].Key == itemKey) {
                if (inventory[i] is CountableItemData countItem) {
                    if (countItem.CurrStackCount < countItem.MaxStackCount) {
                        return i;
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
        inventory[index] = null;
    }

    //������ 1�� ���
    public void useItem(int index) {
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


    private void dropItem(int index) {
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
}