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

        timeScale = 1f;
    }

    [SerializeField] private float timeScale;
    private float WorldTime;
    private float WorldHour;
    private int SurviveDay;
    private int TotalDay;

    private bool isOrangeSky = false;

    public float GetTimeScale() { return timeScale; }
    public float GetWorldTime() { return WorldTime; }
    public float GetWorldHour() { return WorldHour; }
    public int GetSurviveDay() { return SurviveDay; }
    public int GetTotalDay() { return TotalDay; }


    private void Start() {
        //TODO: Save 구현 시 세이브 된 WorldTime으로 가져오기
        WorldTime = 80f;
        WorldHour = WorldTime / (360f / 24f) % 24;
        SurviveDay = 1;
        TotalDay = (int)((WorldTime - 90f) / 360f) + 1;
    }

    private void Update() {
        WorldTime += Time.deltaTime * timeScale;
        WorldHour = WorldTime / (360f / 24f) % 24;

        if ((int)WorldHour == 6 || (int)WorldHour == 22) {
            if (!isOrangeSky) {
                isOrangeSky = true;
                if ((int)WorldHour == 6) {
                    SurviveDay++;
                    TotalDay++;
                }
            }
        }
        else isOrangeSky = false;
    }
}
