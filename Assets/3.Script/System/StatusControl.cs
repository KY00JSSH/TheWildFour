using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *      [ READ ]
 *      ���� �ο��ϴ� ������Ʈ ��ũ��Ʈ���� �÷��̾�� ���� �ο�
 *      StatusControl.Instance.GiveStatus(Status status, PlayerStatus player)
 *      
 *      Unity �ν����Ϳ��� StatusControl ��ũ��Ʈ�� �ִ�
 *      �� Status�� Total Time, Tick Time (1�ʴ� ���ҵǴ� ������) ���� �ʿ�
 *      
 */

public enum Status {
    Heat,           // ����
    Full,           // ��ȭ (HP, Hunger ȸ��)
    Satiety,        // ���� (�̵��ӵ�����)
    Poison,         // �ߵ�
    Bleeding,       // ����
    Blizzard,       // ������
    Indigestion,    // ��ȭ�ҷ�
    Heal            // ġ�� (Hunger ȸ��)
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
        remainTime -= tickTime * Time.deltaTime;
    }
}

public class StatusControl : MonoBehaviour {
    public static StatusControl Instance = null;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    [SerializeField]
    private StatusData[] statusList =
        new StatusData[Enum.GetValues(typeof(Status)).Length];

    // UI ������ return �޼���
    public List<StatusData> ActivatedStatus = new List<StatusData>();
    public float GetTotalTime(Status status) {
        return statusList[(int)status].totalTime;
    }
    public float GetRemainTime(Status status) {
        return statusList[(int)status].remainTime;
    }

    public void GiveStatus(Status status, PlayerStatus player, float time = 0) {
        StatusData currentStatus = statusList[(int)status];
        if (time != 0) currentStatus.totalTime = time;

        if (currentStatus.isTicked) {
            if (status == Status.Heat || status == Status.Satiety)
                currentStatus.SetRemainTime(currentStatus.totalTime);
            else if (status == Status.Full)
                currentStatus.SetRemainTime(currentStatus.totalTime + currentStatus.remainTime);
        }
        else
            StartCoroutine(Tick(currentStatus, player));
    }

    private IEnumerator Tick(StatusData status, PlayerStatus player) {
        if (!ActivatedStatus.Contains(status)) ActivatedStatus.Add(status);
        status.SetRemainTime(status.totalTime);
        status.isTicked = true;

        player.SetPlayerStatus(status.type);
        while (status.remainTime > 0) {
            yield return null;
            status.Tick();
        }
        status.SetRemainTime(0f);
        player.ResetPlayerStatus(status.type);
        ActivatedStatus.Remove(status);
    }
}
