using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenController : MonoBehaviour {
    private bool isInvenFull = false;               //�κ��丮 ��ü ���ְ� �������� �߰� ���� ����
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

    //if �ش� �������� inven�� �ְ�,(�ش� box item count < itemMaxStackCount)
    // �ش� ĭ�� ������ �߰�
    //  else if(!full)  ���ڽ��� ������ add
    //else isInvenFull = true
    public void ItemAdd() {
        Item item = itemObejct.GetComponent<Item>();

        int checkNum = canAddThisBox(item.Key);

        if (checkNum == 16) {
            inventory.Add(item);
        }
        else if (checkNum != 99) {
            //�ش�ĭ�� ������ �߰�
            if (item is CountableItem countItem &&
                inventory[checkNum] is CountableItem newCountItem) {
                newCountItem.addCurrStack(countItem.CurrStackCount);
            }
        }
        else {
            int existBox = isExistEmptyBox();
            if (existBox == 17) {
                //�� �ڽ��� ������ add
                inventory.Add(item);
            }
            else if (existBox != 99) {
                //null�� ����� inventory�� �߰�
                inventory[existBox] = item;
            }
        }

        if (checkInvenFull()) {
            isInvenFull = true;
        }

        InvenChanged?.Invoke(inventory);
    }

    public bool canItemAdd() {
        Item item = itemObejct.GetComponent<Item>();
        int checkNum = canAddThisBox(item.Key);
        if (checkNum == 16 || checkNum != 99) {
            return true;
        }
        else {
            int existBox = isExistEmptyBox();
            if (existBox == 17 || existBox != 99) {
                return true;
            }
            else {
                Debug.Log("���� box�� �߰� ����");
                return false;
            }
        }
    }

    private bool checkInvenFull() {
        if (!canItemAdd()) {
            for (int i = 0; i < inventory.Count; i++) {
                //��� �κ��丮 ��������  countable�� �� stack == MAX üũ ��� MAX �� TRUE �� �ٲ�
                //�̰Ŵ� ������ ADD �ϰ� ���� �Ź� üũ
                if (inventory[i] is CountableItem ci) {
                    if (!ci.IsMax) {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    //���� �ڽ��� �ش� item�� �ְ�, �ش� ĭ�� �߰� ������ �� �ش� ĭ num�� return, ������ 99 return
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

    //�� inven box�� �ִ��� ����
    public int isExistEmptyBox() {
        if (inventory.Count < invenUi.CurrInvenCount) {
            return 17;  //�ƿ� ������ ���� inven�� ������ 17���� return
        }
        else {
            for (int i = 0; i < inventory.Count; i++) {
                var weaponItem = inventory[i].itemData as WeaponItemData;
                var countableItem = inventory[i].itemData as CountableItemData;
                var foodItem = inventory[i].itemData as FoodItemData;
                var equipItem = inventory[i].itemData as EquipItemData;
                var medicItem = inventory[i].itemData as MedicItemData;

                if (weaponItem == null && countableItem == null && foodItem == null && equipItem == null && medicItem == null) {
                    if (inventory[i] == null) {
                        //������ ���������� null�� �ʱ�ȭ �� inventory�϶��� �ش� index return
                        return i;
                    }
                }
            }
            return 99;  //�ƿ� �� �ڽ��� �����Ҷ�
        }
    }

    private void removeItem(int index) {
        if (index >= 0 && index < inventory.Count) {
            inventory[index] = null;
        }
        InvenChanged?.Invoke(inventory);
    }

    //������ 1�� ���
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
                //��, ���� �ƴҶ�
                if (inventory[index] is CountableItem countItem) {
                    countItem.useCurrStack(1);
                    //TODO: ������ ���
                }
            }
            else {
                if (inventory[index] is CountableItem countItem) {
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
        InvenChanged?.Invoke(inventory);
    }
}