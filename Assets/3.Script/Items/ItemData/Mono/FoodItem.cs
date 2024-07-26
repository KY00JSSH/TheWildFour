using UnityEngine;
using System.Collections;

public enum ItemStatus {
    Fresh,   // 신선
    Spoiled, // 상함
    Rotten   // 부패
}

public class FoodItem : CountableItem {
    public FoodItemData foodItemData;
    public PlayerStatus playerStatus;

    private Renderer objectRenderer;

    public bool isMeat = false;

    protected float totalTime, tickTime;
    protected bool spoilageStart = false;

    private float currDecayTime;   //현재 부패 시간
    public float CurrDecayTime { get { return currDecayTime; } }

    private ItemStatus status = ItemStatus.Fresh;
    public ItemStatus Status { get { return status; } }

    public float HealTime => foodItemData.HealTime;

    [SerializeField]
    private GameObject spoilPrf;

    private void Awake() {
        playerStatus = FindObjectOfType<PlayerStatus>();
        currDecayTime = foodItemData.TotalDecayTime;
        objectRenderer = GetComponent<Renderer>();
    }

    public float getHealAmount() {
        return playerStatus.GetHealTick() * HealTime;
    }

    //부패시간 초기화
    public void startSpoilage() {
        if (currDecayTime > 0 && !spoilageStart) {
            StartCoroutine(spoiling());
        }
    }

    private IEnumerator spoiling() {
        spoilageStart = true;

        while (currDecayTime > 0) {
            Debug.Log(currDecayTime);
            currDecayTime -= 1.0f;
            CheckStatus();
            yield return new WaitForSeconds(1f);
        }
        currDecayTime = 0;
        spoilageStart = false;
    }

    //부패시간 줄이기
    public void decayDelete(float amount) {
        float thisDecayTime = currDecayTime - amount;
        SetCurrDecayTime(thisDecayTime);
        CheckStatus();
    }

    public void setInvisible() {
        if (objectRenderer != null) {
            objectRenderer.enabled = false;
        }
    }

    public void setVisible() {
        if (objectRenderer != null) {
            objectRenderer.enabled = true;
        }
    }

    public void SetCurrDecayTime(float value) {
        currDecayTime = value;
    }

    public void CheckStatus() {
        if (currDecayTime == 0) {
            status = ItemStatus.Rotten;
            
            InvenController invenController = FindObjectOfType<InvenController>();
            invenController.updateInvenInvoke();
        }
        else if (currDecayTime <= 0.25 * foodItemData.TotalDecayTime) {
            status = ItemStatus.Spoiled;
        }
        else {
            status = ItemStatus.Fresh;
        }
    }
}