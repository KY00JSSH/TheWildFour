using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *      [ READ ]
 *      상태 부여하는 오브젝트 스크립트에서 플레이어에게 상태 부여
 *      StatusControl.Instance.GiveStatus(Status status, PlayerStatus player)
 *      
 *      Unity 인스펙터에서 StatusControl 스크립트에 있는
 *      각 Status의 Total Time, Tick Time (1초당 감소되는 게이지) 설정 필요
 *      
 */

public enum Status {
    Heat,           // 열기
    Full,           // 포화 (회복)
    Satiety,        // 포만 (이동속도저하)
    Poison,         // 중독
    Bleeding,       // 출혈
    Blizzard,       // 눈보라
    Indigestion     // 소화불량
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

    // UI 제공용 return 메서드
    public float GetTotalTime(Status status) {
        return statusList[(int)status].totalTime;
    }
    public float GetRemainTime(Status status) {
        return statusList[(int)status].remainTime;
    }

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
}
