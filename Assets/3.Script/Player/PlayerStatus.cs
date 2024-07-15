using System;
using System.Collections;
using UnityEngine;


public class PlayerStatus : MonoBehaviour {
    private Player_InfoViewer infoViewer;
    
    private float defaultHp = 100, defaultHunger = 100, defaultWarm = 100;
    private float PlayerHp, PlayerHunger, PlayerWarm;
    public float PlayerMaxHp;

    private float WarmDamage = 0.5f, HungerDamage = 0.2f;



    private bool[] statusList;
    public bool GetPlayerStatus(Status status) { return statusList[(int)status]; }
    public void SetPlayerStatus(Status status) { statusList[(int)status] = true; }
    public void ResetPlayerStatus(Status status) { statusList[(int)status] = false; }

    public void TakeWarmDamage() {
        if (GetPlayerStatus(Status.Heat)) return;

        PlayerWarm -= WarmDamage * Time.deltaTime;
        
        if(PlayerWarm <= 0) {
            PlayerWarm = 0;
            TakeHpDamage(2f);
        }
    }

    public void TakeHungerDamage() {
        if (GetPlayerStatus(Status.Full) ||
            GetPlayerStatus(Status.Satiety)) return;

        PlayerHunger -= HungerDamage * Time.deltaTime;
        if (PlayerMove.isMove) PlayerHunger -= HungerDamage * Time.deltaTime * 0.5f;

        if (PlayerHunger <= 0) {
            PlayerHunger = 0;
            TakeHpDamage(3f);
        }
    }

    public void TakeHpDamage(float damage = 1f) {
        PlayerHp -= damage * Time.deltaTime;
        if(PlayerHp <= 0) {
            PlayerHp = 0;
            //TODO: 사망 이벤트 필요. 0707
        }
    }

    public void EatFood(FoodItem item) {
        StatusControl.Instance.GiveStatus(Status.Full, this, item.HealPoint);
        StatusControl.Instance.GiveStatus(Status.Satiety, this, item.FullPoint);
    }

    public void RestoreHp(float heal) {

    }
    private IEnumerator Heal() {

    }


    private void Awake() {
        infoViewer = FindObjectOfType<Player_InfoViewer>();
    }

    private void Start() {
        PlayerMaxHp = defaultHp;
        PlayerHp = PlayerMaxHp;
        PlayerHunger = defaultHunger;
        PlayerWarm = defaultWarm;
        statusList = new bool[Enum.GetValues(typeof(Status)).Length];
    }

    private void Update() {
        infoViewer.SetPlayerHp((int)PlayerHp);
        infoViewer.SetPlayerHunger((int)PlayerHunger);
        infoViewer.SetPlayerWarm((int)PlayerWarm);

        TakeWarmDamage();
        TakeHungerDamage();
    }

}