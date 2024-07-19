using UnityEngine;

public class FoodItem : CountableItem {
    public FoodItemData foodItemData;

    public PlayerStatus playerStatus;

    private float currDecayTime = 0;   //���� ���� �ð�
    public float CurrDecayTime { get { return currDecayTime; } }

    public float HealTime => foodItemData.HealTime;
    private void Awake() {
        playerStatus = FindObjectOfType<PlayerStatus>();
    }

    public float getHealAmount() {
        return playerStatus.GetHealTick() * HealTime;
    }
}