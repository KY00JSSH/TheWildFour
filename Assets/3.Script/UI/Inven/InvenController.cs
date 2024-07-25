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
        if(inventory[index] != null) {
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
            }
            else {
                return;
            }
        }else {
            return;
        }
    }

    //������ ���۽� �κ��� �� �� �ִ��� ����
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
        else if (item.GetComponent<WeaponItem>() != null) {
            WeaponItem weaponItem = item.GetComponent<WeaponItem>();
            int[] matKeyArr = (int[])weaponItem.weaponItemData.MaterialKey.Clone();
            int[] matCountArr = (int[])weaponItem.weaponItemData.MaterialCount.Clone();
            for (int i = 0; i < matKeyArr.Length; i++) {
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
    //TODO: ���۽� ����ϴ� �ʿ� ������ ������ ���
}