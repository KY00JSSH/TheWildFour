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

    //Ư�� �ε����� ���� ������ �߰�
    public void addWeaponItem(WeaponItemData weapItem, int index) {
        WeaponItem newItem = new WeaponItem();
        newItem.WeaponItemData = weapItem;
        newItem.equipItemData = weapItem;
        newItem.itemData = weapItem;
        inventory[index] = newItem;
        updateInvenInvoke();
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