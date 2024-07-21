using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenController : MonoBehaviour {
    private bool isInvenFull = false;               //�κ��丮 ��ü ���ְ� �������� �߰� ���� ����
    public bool IsInvenFull { get { return isInvenFull; } }
    private List<Item> inventory;
    public List<Item> Inventory { get { return inventory; } }

    private InvenUIController invenUi;
    private MenuWeapon menuWeapon;
    private PlayerStatus playerStatus;

    public GameObject itemObject;

    public delegate void OnInvenChanged(List<Item> inventory);
    public event OnInvenChanged InvenChanged;

    private void Awake() {
        inventory = new List<Item>();
        invenUi = FindObjectOfType<InvenUIController>();
        menuWeapon = FindObjectOfType<MenuWeapon>();
        playerStatus = FindObjectOfType<PlayerStatus>();
        initInven();
    }

    private void initInven() {
        for (int i = 0; i < invenUi.CurrInvenCount; i++) {
            inventory.Add(null);
        }
    }

    public void invenFullReset() {
        isInvenFull = false;
    }

    public void addInvenBox() {
        inventory.Add(null);
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
        InvenChanged?.Invoke(inventory);
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
    private int canAddThisBox(int itemKey) {
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

    //�κ��� ���� �߰� ���� flag reset
    public void invenFullFlagReset() {
        isInvenFull = false;
    }
    //������ ������ �κ����� ����
    public void removeItem(int index) {
        inventory[index] = null;
        invenFullFlagReset();
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
                InvenChanged?.Invoke(inventory);
            }
            else {
                inventory[changeIndex] = inventory[currentIndex];
                inventory[currentIndex] = null;
                invenFullFlagReset();
                InvenChanged?.Invoke(inventory);
            }
        }
    }

    //���â�� �κ�â ������ ����Ī
    public void changeItemIntoWeapSlot(WeaponItemData item, int index) {
        //���Ⱑ �̹� ������ �κ�â�̶� ����Ī �ƴϸ� �κ��� �ִ� ������ ����
        if (item != null) {
            WeaponItem newItem = new WeaponItem();
            newItem.WeaponItemData = item;
            newItem.equipItemData = item;
            newItem.itemData = item;
            inventory[index] = newItem;
            InvenChanged?.Invoke(inventory);
        }
        else {
            inventory[index] = null;
            invenFullFlagReset();
            InvenChanged?.Invoke(inventory);
        }
    }

    //Ư�� �ε����� ���� ������ �ִ��� Ȯ��
    public WeaponItemData getIndexItem(int index) {
        if (inventory[index]?.itemData is WeaponItemData weapItem) {
            return weapItem;
        }
        else {
            return null;
        }
    }

    //Ư�� �ε����� ���� ������ �߰�
    public void addWeaponItem(WeaponItemData weapItem, int index) {
        WeaponItem newItem = new WeaponItem();
        newItem.WeaponItemData = weapItem;
        newItem.equipItemData = weapItem;
        newItem.itemData = weapItem;
        inventory[index] = newItem;
        InvenChanged?.Invoke(inventory);
    }

    //������ F�� ���
    public void useInvenItem(int index) {
        if (inventory[index]?.itemData is MedicItemData medicItem) {
            //������ �������� ��ǰ�̸� 1�� ���
            var countItem = inventory[index] as CountableItem;
            countItem.useCurrStack(1);
            MedItem eatMed = inventory[index] as MedItem;
            playerStatus.EatMedicine(eatMed);   //�÷��̾� ���� ������ ����
            if (countItem.CurrStackCount == 0) {
                inventory[index] = null;
            }
            InvenChanged?.Invoke(inventory);
        }
        else if (inventory[index]?.itemData is FoodItemData foodItem) {
            //������ �������� �����̸� 1�� ���
            var countItem = inventory[index] as CountableItem;
            countItem.useCurrStack(1);
            FoodItem eatFood = inventory[index] as FoodItem;
            playerStatus.EatFood(eatFood);  //�÷��̾� ���� ������ ����
            if (countItem.CurrStackCount == 0) {
                inventory[index] = null;
            }
            InvenChanged?.Invoke(inventory);
        }
        else if (inventory[index]?.itemData is WeaponItemData weapItem) {
            //������ ���� - �̹� ���� ���� �Ǿ� ������ ����Ī
            menuWeapon.addSlotFromInvenWeapon(weapItem, index);
            InvenChanged?.Invoke(inventory);
        }
        else {
            return;
        }
    }
    //TODO: ���۽� ����ϴ� �ʿ� ������ ������ ���
    //TODO: ���� �� �κ� �������� ���

    //������ ���۽� �κ��� �� �� �ִ��� ����
    public bool checkCanCreateItem(int itemKey) {
        int checkNum = canAddThisBox(itemKey);

        if (checkNum != 99) {
            return true;
        }
        else {
            int existBox = isExistEmptyBox();
            if (existBox != 99) {
                return true;
            }
            else {
                return false;
            }
        }
    }
}