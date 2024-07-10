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
        //TODO: Save ���� �� ���̺� �� WorldTime���� ��������
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
