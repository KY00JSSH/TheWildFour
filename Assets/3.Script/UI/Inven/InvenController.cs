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

    //장비창과 인벤창 아이템 스위칭
    public void changeItemIntoWeapSlot(WeaponItemData item, int index) {
        //무기가 이미 있을때 인벤창이랑 스위칭 아니면 인벤에 있는 아이템 지움
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

    //특정 인덱스에 무기 아이템 있는지 확인
    public WeaponItemData getIndexItem(int index) {
        if (inventory[index]?.itemData is WeaponItemData weapItem) {
            return weapItem;
        }
        else {
            return null;
        }
    }

    //아이템 F로 사용
    public void useInvenItem(int index) {
        if (inventory[index]?.itemData is MedicItemData medicItem) {
            //선택한 아이템이 약품이면 1개 사용
            var countItem = inventory[index] as CountableItem;
            countItem.useCurrStack(1);
            MedItem eatMed = inventory[index] as MedItem;
            playerStatus.EatMedicine(eatMed);   //플레이어 실제 아이템 섭취
            if (countItem.CurrStackCount == 0) {
                inventory[index] = null;
            }
            updateInvenInvoke();
        }
        else if (inventory[index]?.itemData is FoodItemData foodItem) {
            //선택한 아이템이 음식이면 1개 사용
            var countItem = inventory[index] as CountableItem;
            countItem.useCurrStack(1);
            FoodItem eatFood = inventory[index] as FoodItem;
            playerStatus.EatFood(eatFood);  //플레이어 실제 아이템 섭취
            if (countItem.CurrStackCount == 0) {
                inventory[index] = null;
            }
            updateInvenInvoke();
        }
        else if (inventory[index]?.itemData is WeaponItemData weapItem) {
            //도구면 장착 - 이미 슬롯 장착 되어 있으면 스위칭
            menuWeapon.addSlotFromInvenWeapon(weapItem, index);
            updateInvenInvoke();
        }
        else {
            return;
        }
    }

    //아이템 제작시 인벤에 들어갈 수 있는지 여부
    public bool createItem(ItemData item) {
        bool isCreate = false;
        if (item is MedicItemData medicItem) {
            int[] matKeyArr = (int[])medicItem.MaterialKey.Clone();
            int[] matCountArr = (int[])medicItem.MaterialCount.Clone();
            for (int i = 0; i < medicItem.MaterialKey.Length; i++) {
                if (isMaterials(matKeyArr[i], matCountArr[i])) {
                    if (isAddNewItem(matKeyArr[i], matCountArr[i])) {
                        isCreate = true;
                        //TODO: 아이템 제작 후 인벤에 추가
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
                        //TODO: 아이템 제작 후 인벤에 추가
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
            //빈박스 있음
            return true;
        }
        else {
            //아이템 사용해서 빈박스가 생기는지 여부
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

    //거처 혹은 작업장 아이템과 아이템 스위칭
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

    //거처 혹은 작업장에 아이템 추가
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

    //TODO: 제작시 사용하는 필요 아이템 있으면 사용
}