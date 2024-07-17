using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenController : MonoBehaviour {
    private bool isInvenFull = false;               //�κ��丮 ��ü ���ְ� �������� �߰� ���� ����
    public bool IsInvenFull { get { return isInvenFull; } }
    private List<Item> inventory;
    public List<Item> Inventory { get { return inventory; } }

    private InvenUIController invenUi;
    public GameObject itemObject;

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
        Item item = itemObject.GetComponent<Item>();

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
            if (existBox != 99 && existBox != 17) {
                //null�� ����� inventory�� �߰�
                inventory[existBox] = item;
            }
            else if (existBox == 17) {
                //�� �ڽ��� ������ add
                inventory.Add(item);
            }
        }

        if (checkInvenFull()) {
            isInvenFull = true;
        }

        InvenChanged?.Invoke(inventory);
    }

    public bool canItemAdd() {
        Item item = itemObject.GetComponent<Item>();
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
                //��� �κ��丮 �������� max �������� üũ
                //������ ADD �ϰ� ���� �Ź� üũ
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
                if (inventory[i]?.Key != null && inventory[i]?.Key == itemKey) {
                    if (inventory[i] is CountableItem countItem) {
                        if (countItem?.CurrStackCount < countItem?.MaxStackCount) {
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
            for (int i = 0; i < inventory.Count; i++) {
                var weaponItem = inventory[i]?.itemData as WeaponItemData;
                var countableItem = inventory[i]?.itemData as CountableItemData;
                var foodItem = inventory[i]?.itemData as FoodItemData;
                var equipItem = inventory[i]?.itemData as EquipItemData;
                var medicItem = inventory[i]?.itemData as MedicItemData;

                if (weaponItem == null && countableItem == null && foodItem == null && equipItem == null && medicItem == null) {
                    if (inventory[i] == null) {
                        //������ ���������� null�� �ʱ�ȭ �� inventory�϶��� �ش� index return
                        return i;
                    }
                }
            }
            return 17;  //�ƿ� ������ ���� inven�� ������ 17���� return
        }
        else {
            for (int i = 0; i < inventory.Count; i++) {
                var weaponItem = inventory[i]?.itemData as WeaponItemData;
                var countableItem = inventory[i]?.itemData as CountableItemData;
                var foodItem = inventory[i]?.itemData as FoodItemData;
                var equipItem = inventory[i]?.itemData as EquipItemData;
                var medicItem = inventory[i]?.itemData as MedicItemData;

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

    //
    public void invenFullFlagReset() {
        isInvenFull = false;
    }
    //������ ������ �κ����� ����
    public void removeItem(int index) {
        inventory[index] = null;
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

    public int checkItemType(int index) {
        if (index >= 0 && index < inventory.Count) {
            if (inventory[index].itemData.Key != 1 && inventory[index].itemData.Key != 2) {
                //��, ���� �ƴҶ�
                if (inventory[index] is CountableItem countItem) {
                    return 2;
                }
                else {
                    return 3;
                }
            }
            else {
                return 1;   //��, �����϶�
            }
        }
        else {
            return 0;   //������ ������
        }
    }

    public void dropItem(int index) {
        int itemType = checkItemType(index);
        //type 1: ��,���� , 2: ī��Ʈ �Ǵ� item, 3: ī��Ʈ ���� ������
        if (itemType > 0) {
            if (itemType == 1) {
                if (inventory[index] is CountableItem countItem) {
                    if (countItem.CurrStackCount > 8) {
                        countItem.useCurrStack(8);
                    }
                    else if (countItem.CurrStackCount == 8) {
                        countItem.useCurrStack(8);
                        removeItem(index);
                    }
                    else {
                        countItem.useCurrStack(countItem.CurrStackCount);
                        removeItem(index);
                    }
                    InvenChanged?.Invoke(inventory);
                }
            }
            else if (itemType == 2) {
                if (inventory[index] is CountableItem countItems) {
                    countItems.useCurrStack(1);
                    if (countItems.CurrStackCount == 0) {
                        removeItem(index);
                    }
                }
                InvenChanged?.Invoke(inventory);
            }
            else {
                removeItem(index);
                InvenChanged?.Invoke(inventory);
            }            
        }
        else {
            Debug.Log("no item");
        }
    }
}