using System.Collections.Generic;
using UnityEngine;

public class ShelterInvenControll : CommonInven {
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
    }
    private void Start() {
        initInven();
    }

    private void initInven() {
        for (int i = 0; i < shelterInvenUI.CurrInvenCount; i++) {
            shelterInven.Add(null);
        }
    }

    //플레이어 인벤과 아이템 스위칭
    public void switchingInvenItem(int target) {
        List<GameObject> playerInven = invenController.Inventory;
        GameObject item = playerInven[target];

        addItemPlayerInven(target);
        addIndexItem(target, item);
        updateInvenInvoke();
    }

    //플레이어 인벤에 아이템 추가
    public void addItemPlayerInven(int index) {
        invenController.addIndexItem(index, inventory[selectBoxKey]);
    }
}
