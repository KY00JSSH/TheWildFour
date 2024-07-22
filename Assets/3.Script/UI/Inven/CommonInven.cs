using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonInven : MonoBehaviour {
    protected List<Item> inventory = new List<Item>();
    public List<Item> Inventory { get { return inventory; } }

    public GameObject itemObject;

    public delegate void OnInvenChanged(List<Item> inventory);
    public event OnInvenChanged InvenChanged;

    private bool isInvenFull = false;               //�κ��丮 ��ü ���ְ� �������� �߰� ���� ����
    public bool IsInvenFull { get { return isInvenFull; } }

    protected void updateInvenInvoke() {
        InvenChanged?.Invoke(inventory);
    }

    public void addInvenBox() {
        inventory.Add(null);
    }

    public void invenFullReset() {
        isInvenFull = false;
    }

    //�κ��� �߰� ���� flag reset
    public void invenFullFlagReset() {
        isInvenFull = false;
    }

    //������ ������ �κ����� ����
    public void removeItem(int index) {
        inventory[index] = null;
        invenFullFlagReset();
        updateInvenInvoke();
    }

    //if �ش� �������� inven�� �ְ�,(�ش� box item count < itemMaxStackCount)
    // �ش� ĭ�� ������ �߰�
    //  else if(!full)  ���ڽ��� ������ add
    //else isInvenFull = true
    public void ItemAdd() {
        Item item = itemObject.GetComponent<Item>();

        int checkNum = canAddThisBox(item.Key);

        if (checkNum != 99) {
            //�ش�ĭ�� ������ �߰�
            if (item is CountableItem countItem &&
                inventory[checkNum] is CountableItem newCountItem) {
                newCountItem.addCurrStack(countItem.CurrStackCount);
            }
        }
        else {
            int existBox = isExistEmptyBox();
            if (existBox != 99) {
                //null�� ����� inventory�� �߰�
                inventory[existBox] = item;
            }
            else {
                isInvenFull = true;
            }
        }
        updateInvenInvoke();
    }

    //������ �߰� ���ɿ���
    public bool canItemAdd() {
        Item item = itemObject.GetComponent<Item>();
        //�������� ���ļ� ���� �� �ִ��� Ȯ��
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

    //���� �ڽ��� �ش� item�� �ְ�, �ش� ĭ�� �߰� ������ �� �ش� ĭ num�� return, ������ 99 return
    protected int canAddThisBox(int itemKey) {
        for (int i = 0; i < inventory.Count; i++) {
            var weaponItem = inventory[i]?.itemData as WeaponItemData;
            var countableItem = inventory[i]?.itemData as CountableItemData;
            var foodItem = inventory[i]?.itemData as FoodItemData;
            var equipItem = inventory[i]?.itemData as EquipItemData;
            var medicItem = inventory[i]?.itemData as MedicItemData;

            if (weaponItem != null || countableItem != null || foodItem != null || equipItem != null || medicItem != null) {
                if (inventory[i]?.Key != null && inventory[i]?.Key == itemKey) {
                    if (inventory[i] is CountableItem countItem) {
                        if (countItem?.CurrStackCount < countItem?.MaxStackCount) {
                            return i;
                        }
                    }
                }
            }
        }
        return 99;
    }

    //�� inven box�� �ִ��� ����
    public int isExistEmptyBox() {
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
        updateInvenInvoke();
    }

    //������ Ÿ�� üũ
    //type 1: ��,���� , 2: ī��Ʈ �Ǵ� item, 3: ī��Ʈ ���� ������
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

    //������ ����� ������ ����
    public void dropItem(int index) {
        int itemType = checkItemType(index);
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
                    updateInvenInvoke();
                }
            }
            else if (itemType == 2) {
                if (inventory[index] is CountableItem countItems) {
                    countItems.useCurrStack(1);
                    if (countItems.CurrStackCount == 0) {
                        removeItem(index);
                    }
                }
                updateInvenInvoke();
            }
            else {
                removeItem(index);
                updateInvenInvoke();
            }
        }
        else {
            Debug.Log("no item");
        }
    }

    //�κ� �����۳��� ����Ī
    public void changeInvenIndex(int currentIndex, int changeIndex) {
        if (currentIndex != changeIndex && changeIndex != 99) {
            var weaponItem = inventory[changeIndex]?.itemData as WeaponItemData;
            var countableItem = inventory[changeIndex]?.itemData as CountableItemData;
            var foodItem = inventory[changeIndex]?.itemData as FoodItemData;
            var equipItem = inventory[changeIndex]?.itemData as EquipItemData;
            var medicItem = inventory[changeIndex]?.itemData as MedicItemData;

            if (weaponItem != null || countableItem != null || foodItem != null ||
                equipItem != null || medicItem != null || inventory[changeIndex] != null) {
                Item changeItem = inventory[changeIndex];
                inventory[changeIndex] = inventory[currentIndex];
                inventory[currentIndex] = changeItem;
                updateInvenInvoke();
            }
            else {
                inventory[changeIndex] = inventory[currentIndex];
                inventory[currentIndex] = null;
                invenFullFlagReset();
                updateInvenInvoke();
            }
        }
    }
}
