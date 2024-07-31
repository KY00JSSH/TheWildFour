using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStatus : MonoBehaviour {
    private Player_InfoViewer infoViewer;
    public UnityEvent onDead;
    private GameObject player;

    public static bool isDead { get; private set; }
    public void SetPlayerDead() {
        isDead = true;
        player.GetComponent<Animator>().SetBool("isDead", isDead);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().SetTrigger("triggerDie");
        AudioManager.instance.StopBGM();
        AudioManager.instance.PlaySFX(AudioManager.Sfx.PlayerDeath_LostAndWintered);
        AudioManager.instance.StopSFX(AudioManager.Sfx.NoHP_heartbeat);

    }

    public void PlayerRespawn() {
        isDead = false;
        player.GetComponent<Animator>().SetBool("isDead", isDead);
        player.transform.position = new Vector3(0, 0, 0);
        AudioManager.instance.StopSFX(AudioManager.Sfx.PlayerDeath_LostAndWintered);
        Start();
    }


    private float PlayerHp, PlayerHunger, PlayerWarm, PlayerMaxHp;
    public void SetPlayerMaxHp(float hp) { PlayerMaxHp = hp; }
    public float GetPlayerMaxHp() { return PlayerMaxHp; }
    public float GetPlayerHp() { return PlayerHp; }
    public float GetPlayerWarm() { return PlayerWarm; }
    public float GetPlayerHunger() { return PlayerHunger; }

    private float WarmDamage = 0.5f, HungerDamage = 0.2f;
    private float HealRestore = 0.3f, HungerRestore = 0.5f, WarmRestore = 1.2f;

    public float GetHealTick() { return HealRestore; }

    private bool[] statusList;
    public bool GetPlayerStatus(Status status) { return statusList[(int)status]; }
    public void SetPlayerStatus(Status status) { statusList[(int)status] = true; }
    public void ResetPlayerStatus(Status status) { statusList[(int)status] = false; }

    public void TakeWarmDamage() {
        if (GetPlayerStatus(Status.Heat)) return;
        PlayerWarm -= WarmDamage * Time.deltaTime;

        if (PlayerWarm <= 0) {
            PlayerWarm = 0;
            TakeHpDotDamage(4f);
        }
    }

    public void TakeHungerDamage() {
        if (GetPlayerStatus(Status.Full) ||
            GetPlayerStatus(Status.Satiety)) return;

        PlayerHunger -= HungerDamage * Time.deltaTime;
        if (PlayerMove.isMove) PlayerHunger -= HungerDamage * Time.deltaTime * 0.5f;

        if (PlayerHunger <= 0) {
            PlayerHunger = 0;
            TakeHpDotDamage(3f);
        }
    }

    public void TakeHpDotDamage(float damage = 1f) {
        PlayerHp -= damage * Time.deltaTime;
        if (PlayerHp <= 0) {
            PlayerHp = 0;
            onDead?.Invoke();
        }
    }

    public void TakeDamage(float damage) {
        PlayerHp -= damage;
        if (PlayerHp <= 0) {
            PlayerHp = 0;
            onDead?.Invoke();
        }
    }

    public void EatFood(FoodItem item) {
        if (item.Status == ItemStatus.Rotten) return;
        else if (item.Status == ItemStatus.Spoiled)
            StatusControl.Instance.GiveStatus(Status.Indigestion, this);
        else
            StatusControl.Instance.GiveStatus(Status.Full, this, item.HealTime);
        AudioManager.instance.PlaySFX(AudioManager.Sfx.Eat);
    }

    public void EatMedicine(MedicItem item) {
        if (item.HealTime == 0) {
            PlayerHp += 80;
            if (PlayerHp > PlayerMaxHp) PlayerHp = PlayerMaxHp;
            return;
        }
        StatusControl.Instance.GiveStatus(Status.Heal, this, item.HealTime);
    }

    public void RestoreWarm() {
        if (GetPlayerStatus(Status.Heat)) {
            PlayerWarm += WarmRestore * Time.deltaTime;
            if (PlayerWarm > 100) PlayerWarm = 100;
        }
    }

    public void RestoreHp() {
        if (GetPlayerStatus(Status.Heal)) {
            PlayerHp += HealRestore * Time.deltaTime;
            if (PlayerHp > PlayerMaxHp) PlayerHp = PlayerMaxHp;
        }
    }

    public void RestoreHpHunger() {
        if (GetPlayerStatus(Status.Full)) {
            PlayerHp += HealRestore * Time.deltaTime;
            PlayerHunger += HungerRestore * Time.deltaTime;
            if (PlayerHp > PlayerMaxHp) PlayerHp = PlayerMaxHp;
            if (PlayerHunger > 100) {
                PlayerHunger = 100;
                if (!GetPlayerStatus(Status.Satiety)) {
                    StatusControl.Instance.GiveStatus(Status.Satiety, this);
                }
            }
        }
    }

    private bool isSlowed = false;
    public void SatietySlow() {
        if (GetPlayerStatus(Status.Satiety) && !isSlowed) {
            StartCoroutine(Slow());
        }
    }

    public void TakeIndigestion() {
        if (GetPlayerStatus(Status.Indigestion)) {
            if (!isSlowed) StartCoroutine(Slow());
            TakeHpDotDamage(1f);
        }
    }

    public IEnumerator Slow() {
        isSlowed = true;

        // 공격모션 속도느리게 충돌 예상지점
        PlayerMove plyaerMove = GetComponentInChildren<PlayerMove>();
        float speed = plyaerMove.GetPlayerMoveSpeed();
        plyaerMove.SetPlayerMoveSpeed(speed * 0.8f);
        while (GetPlayerStatus(Status.Satiety) || GetPlayerStatus(Status.Indigestion)) {
            yield return null;
        }
        plyaerMove.SetPlayerMoveSpeed(speed);
        isSlowed = false;
    }

    private void Awake() {
        infoViewer = FindObjectOfType<Player_InfoViewer>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start() {
        PlayerMaxHp = Save.Instance.saveData.playerMaxHP;
        PlayerHp = Save.Instance.saveData.playerHP;
        PlayerWarm = Save.Instance.saveData.playerWarm;
        PlayerHunger = Save.Instance.saveData.playerHunger;
        statusList = new bool[Enum.GetValues(typeof(Status)).Length];
        AudioManager.instance.PlayBGM(AudioManager.Bgm.music_IndifferentSlow, 2);

        isDead = false;
    }

    private void Update() {
        infoViewer.SetPlayerHp((int)PlayerHp);
        infoViewer.SetPlayerHunger((int)PlayerHunger);
        infoViewer.SetPlayerWarm((int)PlayerWarm);

        if (isDead) return;

        TakeWarmDamage();
        TakeHungerDamage();
        RestoreHpHunger();
        SatietySlow();
        RestoreHp();
        RestoreWarm();
        TakeIndigestion();
    }
}
