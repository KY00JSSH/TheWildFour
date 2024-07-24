using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenController : CommonInven {

    private InvenUIController invenUi;
    private MenuWeapon menuWeapon;
    private PlayerStatus playerStatus;
    private PlayerItemUseControll playerItemUse;

    private WorkshopInvenControll workshopInven;
    private ShelterInvenControll shelterInven;

    private void Awake() {
        invenUi = FindObjectOfType<InvenUIController>();
        menuWeapon = FindObjectOfType<MenuWeapon>();
        playerStatus = FindObjectOfType<PlayerStatus>();
        workshopInven = FindObjectOfType<WorkshopInvenControll>();
        shelterInven = FindObjectOfType<ShelterInvenControll>();
        playerItemUse = FindObjectOfType<PlayerItemUseControll>();
        initInven();
    }

    private void initInven() {
        for (int i = 0; i < invenUi.CurrInvenCount; i++) {
            inventory.Add(null);
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
            updateInvenInvoke();
        }
        else {
            inventory[index] = null;
            invenFullFlagReset();
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
            updateInvenInvoke();
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
            updateInvenInvoke();
        }
        else if (inventory[index]?.itemData is WeaponItemData weapItem) {
            //������ ���� - �̹� ���� ���� �Ǿ� ������ ����Ī
            menuWeapon.addSlotFromInvenWeapon(weapItem, index);
            updateInvenInvoke();
        }
        else {
            return;
        }
    }

    //������ ���۽� �κ��� �� �� �ִ��� ����
    public bool createItem(ItemData item) {
        bool isCreate = false;
        if (item is MedicItemData medicItem) {
            int[] matKeyArr = (int[])medicItem.MaterialKey.Clone();
            int[] matCountArr = (int[])medicItem.MaterialCount.Clone();
            for (int i = 0; i < medicItem.MaterialKey.Length; i++) {
                if (isMaterials(matKeyArr[i], matCountArr[i])) {
                    if (isAddNewItem(matKeyArr[i], matCountArr[i])) {
                        isCreate = true;
                        //TODO: ������ ���� �� �κ���2
                        //�߰�
                    }
                    else {
                        isCreate = false;
                    }
                }
                else {
                    isCreate = false;
                }
            }
        }
        else if (item is WeaponItemData weaponItem) {
            int[] matKeyArr = (int[])weaponItem.MaterialKey.Clone();
            int[] matCountArr = (int[])weaponItem.MaterialCount.Clone();
            for (int i = 0; i < weaponItem.MaterialKey.Length; i++) {
                if (isMaterials(matKeyArr[i], matCountArr[i])) {
                    if (isAddNewItem(matKeyArr[i], matCountArr[i])) {
                        isCreate = true;
                        //TODO: ������ ���� �� �κ��� �߰�
                    }
                    else {
                        isCreate = false;
                    }
                }
                else {
                    isCreate = false;
                }
            }
        }
        else {
            Debug.Log("check");
        }

        return isCreate;
    }

    private bool isMaterials(int key, int count) {
        int totalCount = 0;
        for (int i = 0; i < inventory.Count; i++) {
            if (inventory[i] is CountableItem countItem) {
                if (countItem?.Key == key) {
                    totalCount += countItem.CurrStackCount;
                }
            }
        }
        if (totalCount >= count) {
            return true;
        }
        else {
            return false;
        }
    }

    private bool isAddNewItem(int key, int count) {
        if (isExistEmptyBox() != 99) {
            //��ڽ� ����
            return true;
        }
        else {
            //������ ����ؼ� ��ڽ��� ������� ����
            for (int i = 0; i < inventory.Count; i++) {
                if (inventory[i] is CountableItem countItem) {
                    if (countItem?.Key == key) {
                        if (countItem.CurrStackCount == count) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }

    //��ó Ȥ�� �۾��� �����۰� ������ ����Ī
    public void switchingInvenItem(int index, bool isWorkshop) {
        if (isWorkshop) {
            List<Item> workshopInv = workshopInven.Inventory;
            ItemData item = workshopInv[index]?.itemData;

            addItemBuildInven(index, isWorkshop);

            if (item is FoodItemData foodItem) {
                addIndexItem(foodItem, playerItemUse.selectBoxKey);
            }
            else if (item is MedicItemData medicItem) {
                addIndexItem(medicItem, playerItemUse.selectBoxKey);
            }
            else if (item is WeaponItemData weaponItem) {
                addIndexItem(weaponItem, playerItemUse.selectBoxKey);
            }
            else if (item is EquipItemData equipItem) {
                addIndexItem(equipItem, playerItemUse.selectBoxKey);
            }
            else if (item is CountableItemData countableItem) {
                addIndexItem(countableItem, playerItemUse.selectBoxKey);
            }
            else {
                addIndexItem(inventory[playerItemUse.selectBoxKey]?.itemData, index);
            }
            updateInvenInvoke();
        }
        else {
            List<Item> shelterInv = shelterInven.Inventory;
            ItemData item = shelterInv[index]?.itemData;

            addItemBuildInven(index, isWorkshop);

            if (item is FoodItemData foodItem) {
                addIndexItem(foodItem, playerItemUse.selectBoxKey);
            }
            else if (item is MedicItemData medicItem) {
                addIndexItem(medicItem, playerItemUse.selectBoxKey);
            }
            else if (item is WeaponItemData weaponItem) {
                addIndexItem(weaponItem, playerItemUse.selectBoxKey);
            }
            else if (item is EquipItemData equipItem) {
                addIndexItem(equipItem, playerItemUse.selectBoxKey);
            }
            else if (item is CountableItemData countableItem) {
                addIndexItem(countableItem, playerItemUse.selectBoxKey);
            }
            else {
                addIndexItem(inventory[playerItemUse.selectBoxKey]?.itemData, index);
            }
            updateInvenInvoke();
        }
    }

    //��ó Ȥ�� �۾��忡 ������ �߰�
    public void addItemBuildInven(int index, bool isWorkshop) {
        if (isWorkshop) {
            if (inventory[index]?.itemData is FoodItemData foodItem) {
                workshopInven.addIndexItem(foodItem, index);
            }
            else if (inventory[index]?.itemData is MedicItemData medicItem) {
                workshopInven.addIndexItem(medicItem, index);
            }
            else if (inventory[index]?.itemData is WeaponItemData weaponItem) {
                workshopInven.addIndexItem(weaponItem, index);
            }
            else if (inventory[index]?.itemData is EquipItemData equipItem) {
                workshopInven.addIndexItem(equipItem, index);
            }
            else if (inventory[index]?.itemData is CountableItemData countableItem) {
                workshopInven.addIndexItem(countableItem, index);
            }
            else {
                workshopInven.addIndexItem(inventory[index]?.itemData, index);
            }
        }
        else {
            if (inventory[index]?.itemData is FoodItemData foodItem) {
                shelterInven.addIndexItem(foodItem, index);
            }
            else if (inventory[index]?.itemData is MedicItemData medicItem) {
                shelterInven.addIndexItem(medicItem, index);
            }
            else if (inventory[index]?.itemData is WeaponItemData weaponItem) {
                shelterInven.addIndexItem(weaponItem, index);
            }
            else if (inventory[index]?.itemData is EquipItemData equipItem) {
                shelterInven.addIndexItem(equipItem, index);
            }
            else if (inventory[index]?.itemData is CountableItemData countableItem) {
                shelterInven.addIndexItem(countableItem, index);
            }
            else {
                shelterInven.addIndexItem(inventory[index]?.itemData, index);
            }
        }
    }

    //TODO: ���۽� ����ϴ� �ʿ� ������ ������ ���
}