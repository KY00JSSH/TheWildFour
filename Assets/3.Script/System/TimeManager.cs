using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {
    // 1. �� �ð� (����Ǿ�� ��)
    // 2. ���� �ð� (���� �� �ʱ�ȭ��)
    // 3. ���� �ð� (�� �ð��� �Ϸ�� �ð����� ���� ���?)
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
        // ��ħ 6�� ~ ���� 6�� : �ѹ���
        // ���� 6�� ~ ��ħ 6�� : �ѹ���
        /// �� ���� = 24�ð�

        if (WorldHour >= 6f && WorldHour <= 18f)
            return Mathf.InverseLerp(6f, 18f, WorldHour);
        else if (WorldHour > 18f)
            return Mathf.InverseLerp(18f, 24f, WorldHour);
        else if (WorldHour < 6f)
            return Mathf.InverseLerp(-6f, 6f, WorldHour);
    }
    */

    private void Start() {
        //TODO: Save ���� �� ���̺� �� WorldTime���� ��������
        WorldTime = 90f;
        WorldHour = WorldTime / (360f / 24f) % 24;
        SurviveDay = 1;
        TotalDay = (int)((WorldTime - 90f) / 360f) + 1;
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
                }
            }
        }
        else isOrangeSky = false;
    }
}
