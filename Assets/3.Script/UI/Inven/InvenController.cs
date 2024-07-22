using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenController : CommonInven {

    private InvenUIController invenUi;
    private MenuWeapon menuWeapon;
    private PlayerStatus playerStatus;

    private void Awake() {
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

    //특정 인덱스에 무기 아이템 추가
    public void addWeaponItem(WeaponItemData weapItem, int index) {
        WeaponItem newItem = new WeaponItem();
        newItem.WeaponItemData = weapItem;
        newItem.equipItemData = weapItem;
        newItem.itemData = weapItem;
        inventory[index] = newItem;
        updateInvenInvoke();
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
    //TODO: 제작시 사용하는 필요 아이템 있으면 사용
    //TODO: 제작 후 인벤 차있으면 드랍

    //아이템 제작시 인벤에 들어갈 수 있는지 여부
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