using System.Collections.Generic;
using UnityEngine;

public class WorkshopInvenControll : CommonInven {
    private int selectBoxKey = 0;

    private List<Item> shelterInven;
    private ShelterInvenUI shelterInvenUI;

    private InvenController invenController;

    public void setCurrSelectSlot(int keyNum) {
        selectBoxKey = keyNum;
    }

    private void Awake() {
        shelterInven = new List<Item>();
        shelterInvenUI = FindObjectOfType<ShelterInvenUI>();
        invenController = FindObjectOfType<InvenController>();
        initInven();
    }

    private void initInven() {
        for (int i = 0; i < shelterInvenUI.CurrInvenCount; i++) {
            shelterInven.Add(null);
        }
    }

    //플레이어 인벤과 아이템 스위칭
    public void switchingInvenItem(int index) {
        List<Item> playerInven = invenController.Inventory;
        ItemData item = playerInven[index]?.itemData;

        addItemPlayerInven(index);

        if (item is FoodItemData foodItem) {
            addIndexItem(foodItem, selectBoxKey);
        }
        else if (item is MedicItemData medicItem) {
            addIndexItem(medicItem, selectBoxKey);
        }
        else if (item is WeaponItemData weaponItem) {
            addIndexItem(weaponItem, selectBoxKey);
        }
        else if (item is EquipItemData equipItem) {
            addIndexItem(equipItem, selectBoxKey);
        }
        else if (item is CountableItemData countableItem) {
            addIndexItem(countableItem, selectBoxKey);
        }
        else {
            addIndexItem(inventory[selectBoxKey]?.itemData, index);
        }
        updateInvenInvoke();
    }

    //플레이어 인벤에 아이템 추가
    public void addItemPlayerInven(int index) {
        if (inventory[selectBoxKey]?.itemData is FoodItemData foodItem) {
            invenController.addIndexItem(foodItem, index);
        }
        else if (inventory[selectBoxKey]?.itemData is MedicItemData medicItem) {
            invenController.addIndexItem(medicItem, index);
        }
        else if (inventory[selectBoxKey]?.itemData is WeaponItemData weaponItem) {
            invenController.addIndexItem(weaponItem, index);
        }
        else if (inventory[selectBoxKey]?.itemData is EquipItemData equipItem) {
            invenController.addIndexItem(equipItem, index);
        }
        else if (inventory[selectBoxKey]?.itemData is CountableItemData countableItem) {
            invenController.addIndexItem(countableItem, index);
        }
        else {
            invenController.addIndexItem(inventory[selectBoxKey]?.itemData, index);
        }
    }
}
