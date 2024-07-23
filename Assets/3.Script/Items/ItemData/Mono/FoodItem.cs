using UnityEngine;

public enum ItemStatus {
    Fresh,   // 신선
    Spoiled, // 상함
    Rotten   // 부패
}

public class FoodItem : CountableItem {
    public FoodItemData foodItemData;

    public PlayerStatus playerStatus;

    private float currDecayTime;   //현재 부패 시간
    public float CurrDecayTime { get { return currDecayTime; } }

    private ItemStatus status = ItemStatus.Fresh;
    public ItemStatus Status { get { return status; } }


    public float HealTime => foodItemData.HealTime;

    private void Awake() {
        playerStatus = FindObjectOfType<PlayerStatus>();
        currDecayTime = foodItemData.TotalDecayTime;
    }

    public float getHealAmount() {
        return playerStatus.GetHealTick() * HealTime;
    }

    //부패시간 초기화

    //부패시간 줄이기
    public void decayDelete(float amount) {
        float thisDecayTime = currDecayTime - amount;
        SetCurrDecayTime(thisDecayTime);
        CheckStatus();
    }

    public void SetCurrDecayTime(float value) {
        currDecayTime = value;
    }

    public void CheckStatus() {
        if(currDecayTime == 0 ) {
            status = ItemStatus.Rotten;
            //foodItemData.icon = ;
            //foodItemData.dropItemPrefab = ;
        }
        else if( currDecayTime <= 0.25 * foodItemData.TotalDecayTime) {
            status = ItemStatus.Spoiled;
        }
        else {
            status = ItemStatus.Fresh;
        }
    }
}