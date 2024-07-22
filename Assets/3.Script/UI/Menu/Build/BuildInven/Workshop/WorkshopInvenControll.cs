using System.Collections.Generic;
using UnityEngine;

public class WorkshopInvenControll : CommonInven {
    private int selectBoxKey = 0;

    private List<Item> shelterInven;
    private ShelterInvenUI shelterInvenUI;
    
    public void setCurrSelectSlot(int keyNum) {
        selectBoxKey = keyNum;
    }

    private void Awake() {
        shelterInven = new List<Item>();
        shelterInvenUI = FindObjectOfType<ShelterInvenUI>();
        initInven();
    }

    private void initInven() {
        for (int i = 0; i < shelterInvenUI.CurrInvenCount; i++) {
            shelterInven.Add(null);
        }
    }

}
