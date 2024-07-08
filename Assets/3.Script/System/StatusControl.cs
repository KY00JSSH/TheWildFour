using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Status {
    Heat,           // ����
    Full,           // ��ȭ (ȸ��)
    Satiety,        // ���� (�̵��ӵ�����)
    Poison,         // �ߵ�
    Bleeding,       // ����
    Blizzard,       // ������
    Indigestion     // ��ȭ�ҷ�
}

[System.Serializable]
public class StatusData {
    public Status type;
    public float totalTime;
    public float tickTime;
    public float remainTime { get; private set; }
    public bool isTicked = false;

    public void SetRemainTime(float time) {
        remainTime = time;
    }
    public void Tick() {
        remainTime -= tickTime;
    }
}

public class StatusControl : MonoBehaviour {
    public static StatusControl Instance = null;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    [SerializeField] private StatusData[] statusList = 
        new StatusData[Enum.GetValues(typeof(Status)).Length];

    public void GiveStatus(Status status, PlayerStatus player) {
        StatusData currentStatus = statusList[(int)status];

        if (currentStatus.isTicked)
            currentStatus.SetRemainTime(currentStatus.totalTime);
        else 
            StartCoroutine(Tick(currentStatus, player));
    }

    private IEnumerator Tick(StatusData status, PlayerStatus player) {
        status.SetRemainTime(status.totalTime);
        status.isTicked = true;

        player.SetPlayerStatus(status.type);
        while (status.remainTime > 0) {
            yield return new WaitForSeconds(1f);
            status.Tick();
        }
        status.SetRemainTime(0f);
        player.ResetPlayerStatus(status.type);
    }

    private void Update() {

        Debug.Log(statusList[(int)Status.Heat].remainTime);
    }
}