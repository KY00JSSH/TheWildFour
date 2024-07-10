using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {
    // 1. 총 시간 (저장되어야 함)
    // 2. 생존 시간 (죽을 때 초기화됨)
    // 3. 낮밤 시간 (총 시간을 하루당 시간으로 나눠 계산?)
    public static TimeManager Instance = null;
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        timeScale = 5f;
    }

    [SerializeField] private float timeScale;
    private float WorldTime;
    private float WorldHour;

    private int SurviveDay;
    private int TotalDay;

    public float GetTimeScale() { return timeScale; }
    public float GetWorldTime() { return WorldTime; }
    public int GetSurviveDay() { return SurviveDay; }
    public int GetTotalDay() { return TotalDay; }

    public float GetWorldHour() {
        WorldHour = WorldTime / (360f / 24f);
        if (WorldHour >= 24f) {
            WorldHour -= 24f;
            SurviveDay++;
            TotalDay = (int)(WorldTime % (360f / 24f));
        }

        return WorldHour;
    }

    private void Start() {
        //TODO: Save 구현 시 세이브 된 WorldTime으로 가져오기
        WorldTime = 80f;
        SurviveDay = 0;
        TotalDay = (int)(WorldTime % (360f / 24f));
    }

    private void Update() {
        WorldTime += Time.deltaTime * timeScale;
        Debug.Log(TotalDay);
    }
}
