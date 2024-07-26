using System.Collections.Generic;
using UnityEngine;

public class InvenController : CommonInven {

    private InvenUIController invenUi;
    private MenuWeapon menuWeapon;
    private PlayerStatus playerStatus;
    private PlayerItemUseControll playerItemUse;

    private void Awake() {
        invenUi = FindObjectOfType<InvenUIController>();
        menuWeapon = FindObjectOfType<MenuWeapon>();
        playerStatus = FindObjectOfType<PlayerStatus>();
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
        if (inventory[index] != null) {
            if (inventory[index]?.GetComponent<MedicItem>() != null) {
                //선택한 아이템이 약품이면 1개 사용
                MedicItem medicItem = inventory[index].GetComponent<MedicItem>();
                medicItem.useCurrStack(1);
                //playerStatus.EatMedicine(medicItem);   //플레이어 실제 아이템 섭취
                if (medicItem.CurrStackCount == 0) {
                    inventory[index] = null;
                }
                updateInvenInvoke();
            }
            else if (inventory[index]?.GetComponent<FoodItem>() != null) {
                //선택한 아이템이 음식이면 1개 사용
                FoodItem foodItem = inventory[index].GetComponent<FoodItem>();
                foodItem.useCurrStack(1);
                playerStatus.EatFood(foodItem);  //플레이어 실제 아이템 섭취
                if (foodItem.CurrStackCount == 0) {
                    inventory[index] = null;
                }
                updateInvenInvoke();
            }
            else if (inventory[index]?.GetComponent<WeaponItem>() != null) {
                //도구면 장착 - 이미 슬롯 장착 되어 있으면 스위칭
                menuWeapon.addSlotFromInvenWeapon(index, inventory[index]);
                updateInvenInvoke();
            }
            else {
                return;
            }
        }
        else {
            return;
        }
    }

    //아이템 제작시 인벤에 들어갈 수 있는지 여부
    //TODO: 세부 테스트 필요
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
                        //추가
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
        WorkshopInvenControll workshopInven = FindObjectOfType<WorkshopInvenControll>();
        //플레이어 인벤 체크
        for (int i = 0; i < inventory.Count; i++) {
            if (inventory[i] != null) {
                if (inventory[i].GetComponent<CountableItem>() != null) {
                    CountableItem invenCountItem = inventory[i].GetComponent<CountableItem>();
                    if (invenCountItem.itemData.Key == key) {
                        totalCount += invenCountItem.CurrStackCount;
                    }
                }
            }
        }
        //작업장 인벤 체크
        for (int i = 0; i < workshopInven.Inventory.Count; i++) {
            if (workshopInven.Inventory[i] != null) {
                if (workshopInven.Inventory[i].GetComponent<CountableItem>() != null) {
                    CountableItem workInvenCountItem = workshopInven.Inventory[i].GetComponent<CountableItem>();
                    if (workInvenCountItem.itemData.Key == key) {
                        totalCount += workInvenCountItem.CurrStackCount;
                    }
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

    //작업장에 아이템 있는지 + 인벤에 있는지 확인해서 아이템 사용
    public void craftItemUseMain(ItemData itemData) {
        if (itemData is MedicItemData medicItemData) {
            int[] matKeys = (int[])medicItemData.MaterialKey.Clone();
            int[] matCount = (int[])medicItemData.MaterialCount.Clone();

            craftItemUseCommon(matKeys, matCount);
            GameObject newItemObject = Instantiate(itemData.DropItemPrefab, playerStatus.gameObject.transform.position, Quaternion.identity, transform);
            itemObject = newItemObject;
            newItemObject.SetActive(false);
            ItemAdd();
        }
        else if (itemData is EquipItemData equipItemData) {
            int[] matKeys = (int[])equipItemData.MaterialKey.Clone();
            int[] matCount = (int[])equipItemData.MaterialCount.Clone();

            craftItemUseCommon(matKeys, matCount);

            GameObject newItemObject = Instantiate(itemData.DropItemPrefab, playerStatus.gameObject.transform.position, Quaternion.identity, transform);
            itemObject = newItemObject;
            newItemObject.SetActive(false);
            ItemAdd();
        }
    }

    //제작시 아이템 처리 공통
    private void craftItemUseCommon(int[] matKeys, int[] matCount) {
        WorkshopInvenControll workshopInven = FindObjectOfType<WorkshopInvenControll>();
        for (int j = 0; j < matKeys.Length; j++) {
            List<int> invenIndex = new List<int>();
            List<int> workshopIndex = new List<int>();

            int invenItemCount = 0;
            int workshopItemCount = 0;

            //플레이어 인벤 체크
            for (int i = 0; i < inventory.Count; i++) {
                if (inventory[i] != null) {
                    if (inventory[i].GetComponent<CountableItem>() != null) {
                        if (inventory[i].GetComponent<CountableItem>().itemData.Key == matKeys[j]) {
                            invenIndex.Add(i);
                        }
                    }
                }
            }

            //작업장 인벤 체크
            for (int i = 0; i < workshopInven.Inventory.Count; i++) {
                if (inventory[i] != null) {
                    if (inventory[i].GetComponent<CountableItem>() != null) {
                        if (inventory[i].GetComponent<CountableItem>().itemData.Key == matKeys[j]) {
                            workshopIndex.Add(i);
                        }
                    }
                }
            }

            for (int i = 0; i < invenIndex.Count; i++) {
                invenItemCount += inventory[invenIndex[i]].GetComponent<CountableItem>().CurrStackCount;
            }

            for (int i = 0; i < workshopIndex.Count; i++) {
                workshopItemCount += inventory[invenIndex[i]].GetComponent<CountableItem>().CurrStackCount;
            }

            if ((invenItemCount + workshopItemCount) >= matCount[j]) {
                if (invenItemCount < matCount[j]) {
                    //인벤 아이템 전부 삭제 + 작업장 아이템중 일부 삭제
                    for (int i = 0; i < invenIndex.Count; i++) {
                        removeItem(invenIndex[i]);
                    }
                    int leftItemCount = matCount[j] - invenItemCount;
                    workshopInven.removeItemCount(matKeys[j], leftItemCount);
                }
                else {
                    //인벤 아이템중 일부삭제
                    removeItemCount(matKeys[j], matCount[j]);
                }
            }
        }
    }

    public void buildingCreateUseItem(NeedItem[] needItems) {
        //빌딩 건설시 아이템 사용
        for (int i = 0; i < needItems.Length; i++) {
            int needCount = needItems[i].ItemNeedNum;
            removeItemCount(needItems[i].ItemKey, needCount);
            updateInvenInvoke();
        }
    }
}