using System.Collections.Generic;
using UnityEngine;

public class ShelterInvenControll : CommonInven {
    private int selectBoxKey = 0;

    private ShelterInvenUI shelterInvenUI;

    private InvenController invenController;

    public void setCurrSelectSlot(int keyNum) {
        selectBoxKey = keyNum;
    }

    private void Awake() {
        shelterInvenUI = FindObjectOfType<ShelterInvenUI>();
        invenController = FindObjectOfType<InvenController>();
    }
    private void Start() {
        initInven();
    }

    private void initInven() {
        for (int i = 0; i < shelterInvenUI.CurrInvenCount; i++) {
            inventory.Add(null);
        }
    }

    //�÷��̾� �κ��� ������ ����Ī
    public void switchingInvenItem(int target) {
        List<GameObject> playerInven = invenController.Inventory;
        GameObject item = playerInven[target];

        addItemPlayerInven(target);
        addIndexItem(target, item);
        updateInvenInvoke();
    }

    //�÷��̾� �κ��� ������ �߰�
    public void addItemPlayerInven(int index) {
        invenController.addIndexItem(index, inventory[selectBoxKey]);
    }
}
