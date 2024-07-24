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
    }

    private void Start() {
        initInven();
    }

    private void initInven() {
        for (int i = 0; i < invenUi.CurrInvenCount; i++) {
            inventory.Add(null);
        }
    }

    //장비창과 인벤창 아이템 스위칭
    public void changeItemIntoWeapSlot(int index, GameObject item) {
        //무기가 이미 있을때 인벤창이랑 스위칭 아니면 인벤에 있는 아이템 지움
        if (item != null) {
            inventory[index] = item;
            updateInvenInvoke();
        }
        else {
            inventory[index] = null;
            invenFullFlagReset();
        }
    }

    //특정 인덱스에 아이템 있는지 확인
    public GameObject getIndexItem(int index) {
        if (inventory[index] != null) {
            return inventory[index];
        }
        else {
            return null;
        }
    }

    //아이템 F로 사용
    public void useInvenItem(int index) {
        if (inventory[index].GetComponent<MedicItemData>() != null) {
            //선택한 아이템이 약품이면 1개 사용
            MedicItem medicItem = inventory[index].GetComponent<MedicItem>();
            medicItem.useCurrStack(1);
            //playerStatus.EatMedicine(medicItem);   //플레이어 실제 아이템 섭취
            if (medicItem.CurrStackCount == 0) {
                inventory[index] = null;
            }
            updateInvenInvoke();
        }
        else if (inventory[index].GetComponent<FoodItemData>() != null) {
            //선택한 아이템이 음식이면 1개 사용
            FoodItem foodItem = inventory[index].GetComponent<FoodItem>();
            foodItem.useCurrStack(1);
            playerStatus.EatFood(foodItem);  //플레이어 실제 아이템 섭취
            if (foodItem.CurrStackCount == 0) {
                inventory[index] = null;
            }
            updateInvenInvoke();
        }
        else if (inventory[index].GetComponent<WeaponItem>() != null) {
            //도구면 장착 - 이미 슬롯 장착 되어 있으면 스위칭
            menuWeapon.addSlotFromInvenWeapon(index, inventory[index]);
            updateInvenInvoke();
        }
        else {
            return;
        }
    }

    //아이템 제작시 인벤에 들어갈 수 있는지 여부
    public bool createItem(GameObject item) {
        bool isCreate = false;
        if (item.GetComponent<MedicItem>() != null) {
            MedicItem medicItem = item.GetComponent<MedicItem>();
            int[] matKeyArr = (int[])medicItem.medicItemData.MaterialKey.Clone();
            int[] matCountArr = (int[])medicItem.medicItemData.MaterialCount.Clone();
            for (int i = 0; i < matKeyArr.Length; i++) {
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
        else if (item.GetComponent<WeaponItem>() != null) {
            WeaponItem weaponItem = item.GetComponent<WeaponItem>();
            int[] matKeyArr = (int[])weaponItem.weaponItemData.MaterialKey.Clone();
            int[] matCountArr = (int[])weaponItem.weaponItemData.MaterialCount.Clone();
            for (int i = 0; i < matKeyArr.Length; i++) {
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
            if (inventory[i].GetComponent < CountableItem>() != null) {
                if (inventory[i].GetComponent<CountableItem>().itemData.Key == key) {
                    totalCount += inventory[i].GetComponent<CountableItem>().CurrStackCount;
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
                if (inventory[i].GetComponent<CountableItem>() != null) {
                    if (inventory[i].GetComponent<CountableItem>().Key == key) {
                        if (inventory[i].GetComponent<CountableItem>().CurrStackCount == count) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }

    //거처 혹은 작업장 아이템과 아이템 스위칭
    public void switchingInvenItem(int target, bool isWorkshop) {
        if (isWorkshop) {
            List<GameObject> workshopInv = workshopInven.Inventory;
            GameObject item = workshopInv[target];

            addItemBuildInven(target, isWorkshop);
            addIndexItem( target , item);
            updateInvenInvoke();
        }
        else {
            List<GameObject> shelterInv = shelterInven.Inventory;
            GameObject item = shelterInv[target];

            addItemBuildInven(target, isWorkshop);
            addIndexItem(target, item);
            updateInvenInvoke();
        }
    }

    //거처 혹은 작업장에 아이템 추가
    public void addItemBuildInven(int target, bool isWorkshop) {
        if (isWorkshop) {
                workshopInven.addIndexItem(target, inventory[playerItemUse.selectBoxKey]);
            }
        else {
                shelterInven.addIndexItem(target, inventory[playerItemUse.selectBoxKey]);
        }
    }

    //TODO: 제작시 사용하는 필요 아이템 있으면 사용
}