using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {
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
    [Header("DO NOT MODIFY")]
    [SerializeField] private float WorldTime;
    [SerializeField] private float WorldHour;
    private int SurviveDay;
    private int TotalDay;

    public bool isOrangeSky = false;

    public float GetTimeScale() { return timeScale; }
    public float GetWorldTime() { return WorldTime; }
    public float GetWorldHour() { return WorldHour; }
    public int GetSurviveDay() { return SurviveDay; }
    public int GetTotalDay() { return TotalDay; }

    public bool isDay() {
        if (WorldHour >= 6f && WorldHour <= 18f) return true;
        else return false;
    }

    /*
    public float GetSliderValue() {
        // 아침 6시 ~ 저녁 6시 : 한바퀴
        // 저녁 6시 ~ 아침 6시 : 한바퀴
        /// 한 바퀴 = 24시간

        if (WorldHour >= 6f && WorldHour <= 18f)
            return Mathf.InverseLerp(6f, 18f, WorldHour);
        else if (WorldHour > 18f)
            return Mathf.InverseLerp(18f, 24f, WorldHour);
        else if (WorldHour < 6f)
            return Mathf.InverseLerp(-6f, 6f, WorldHour);
    }
    */

    private void Start() {
        //TODO: Save 구현 시 세이브 된 WorldTime으로 가져오기 (완료)
        WorldTime = Save.Instance.saveData.WorldTime;
        WorldHour = WorldTime / (360f / 24f) % 24;
        SurviveDay = Save.Instance.saveData.SurviveDay;
        TotalDay = (int)((WorldTime - 90f) / 360f);
    }
    
    private void Update() {
        if (PlayerStatus.isDead) return;

        WorldTime += Time.deltaTime * timeScale;
        WorldHour = WorldTime / (360f / 24f) % 24;

        if ((int)WorldHour == 6 || (int)WorldHour == 22) {
            if (!isOrangeSky) {
                isOrangeSky = true;
                if ((int)WorldHour == 6) {
                    SurviveDay++;
                    TotalDay++;
                    AudioManager.instance.PlaySFX(AudioManager.Sfx.morning);
                }
                else
                    AudioManager.instance.PlaySFX(AudioManager.Sfx.night4);
            }
        }
        else isOrangeSky = false;
    }
}
