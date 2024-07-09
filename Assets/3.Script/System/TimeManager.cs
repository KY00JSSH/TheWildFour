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

        timeScale = 5f;
    }

    [SerializeField] private float timeScale;
    private float WorldTime;
    private float WorldHour;
    private int SurviveDay;
    private int TotalDay;

    public float GetTimeScale() { return timeScale; }
    public float GetWorldTime() { return WorldTime; }

    private void Start() {
        //TODO: Save ���� �� ���̺� �� WorldTime���� ��������
        WorldTime = 80f;
        SurviveDay = 0;
        TotalDay = (int)(WorldTime % (360f / 24f));
    }

    private void Update() {
        WorldTime += Time.deltaTime * timeScale;
        Debug.Log(TotalDay);
    }

    public float GetWorldHour() {
        WorldHour = WorldTime / (360f / 24f);
        if (WorldHour >= 24f) {
            WorldHour -= 24f;
            SurviveDay++;
            TotalDay = (int)(WorldTime % (360f / 24f));
        }

        return WorldHour;
    }
}
