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

    //���â�� �κ�â ������ ����Ī
    public void changeItemIntoWeapSlot(int index, GameObject item) {
        //���Ⱑ �̹� ������ �κ�â�̶� ����Ī �ƴϸ� �κ��� �ִ� ������ ����
        if (item != null) {
            inventory[index] = item;
            updateInvenInvoke();
        }
        else {
            inventory[index] = null;
            invenFullFlagReset();
        }
    }

    //Ư�� �ε����� ������ �ִ��� Ȯ��
    public GameObject getIndexItem(int index) {
        if (inventory[index] != null) {
            return inventory[index];
        }
        else {
            return null;
        }
    }

    //������ F�� ���
    public void useInvenItem(int index) {
        if (inventory[index] != null) {
            if (inventory[index]?.GetComponent<MedicItem>() != null) {
                //������ �������� ��ǰ�̸� 1�� ���
                MedicItem medicItem = inventory[index].GetComponent<MedicItem>();
                medicItem.useCurrStack(1);
                //playerStatus.EatMedicine(medicItem);   //�÷��̾� ���� ������ ����
                if (medicItem.CurrStackCount == 0) {
                    inventory[index] = null;
                }
                updateInvenInvoke();
            }
            else if (inventory[index]?.GetComponent<FoodItem>() != null) {
                //������ �������� �����̸� 1�� ���
                FoodItem foodItem = inventory[index].GetComponent<FoodItem>();
                foodItem.useCurrStack(1);
                playerStatus.EatFood(foodItem);  //�÷��̾� ���� ������ ����
                if (foodItem.CurrStackCount == 0) {
                    inventory[index] = null;
                }
                updateInvenInvoke();
            }
            else if (inventory[index]?.GetComponent<WeaponItem>() != null) {
                //������ ���� - �̹� ���� ���� �Ǿ� ������ ����Ī
                menuWeapon.addSlotFromInvenWeapon(index, inventory[index]);
                updateInvenInvoke();
                FindObjectOfType<PlayerWeaponEquip>().ChangeEquipWeapon();
            }
            else {
                return;
            }
        }
        else {
            return;
        }
    }

    //������ ���۽� �κ��� �� �� �ִ��� ����
    //TODO: ���� �׽�Ʈ �ʿ�
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
        //�÷��̾� �κ� üũ
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
        //�۾��� �κ� üũ
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
            //��ڽ� ����
            return true;
        }
        else {
            //������ ����ؼ� ��ڽ��� ������� ����
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

    //�۾��忡 ������ �ִ��� + �κ��� �ִ��� Ȯ���ؼ� ������ ���
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

    //���۽� ������ ó�� ����
    private void craftItemUseCommon(int[] matKeys, int[] matCount) {
        WorkshopInvenControll workshopInven = FindObjectOfType<WorkshopInvenControll>();
        for (int j = 0; j < matKeys.Length; j++) {
            List<int> invenIndex = new List<int>();
            List<int> workshopIndex = new List<int>();

            int invenItemCount = 0;
            int workshopItemCount = 0;

            //�÷��̾� �κ� üũ
            for (int i = 0; i < inventory.Count; i++) {
                if (inventory[i] != null) {
                    if (inventory[i].GetComponent<CountableItem>() != null) {
                        if (inventory[i].GetComponent<CountableItem>().itemData.Key == matKeys[j]) {
                            invenIndex.Add(i);
                        }
                    }
                }
            }

            //�۾��� �κ� üũ
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
                    //�κ� ������ ���� ���� + �۾��� �������� �Ϻ� ����
                    for (int i = 0; i < invenIndex.Count; i++) {
                        removeItem(invenIndex[i]);
                    }
                    int leftItemCount = matCount[j] - invenItemCount;
                    workshopInven.removeItemCount(matKeys[j], leftItemCount);
                }
                else {
                    //�κ� �������� �Ϻλ���
                    removeItemCount(matKeys[j], matCount[j]);
                }
            }
        }
    }

    //���� �Ǽ��� ������ ���
    public void buildingCreateUseItem(NeedItem[] needItems) {
        for (int i = 0; i < needItems.Length; i++) {
            int needCount = needItems[i].ItemNeedNum;
            removeItemCount(needItems[i].ItemKey, needCount);
            updateInvenInvoke();
        }
    }
}