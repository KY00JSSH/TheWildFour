using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedItem : CountableItem {
    public MedicItemData medicItemData;

    public PlayerStatus playerStatus;

    public float HealTime => medicItemData.HealTime;

    private void Awake() {
        playerStatus = FindObjectOfType<PlayerStatus>();
    }

    public float getHealAmount() {
        return playerStatus.GetHealTick() * HealTime;
    }
}
