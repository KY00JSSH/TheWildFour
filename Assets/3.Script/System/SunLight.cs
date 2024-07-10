using UnityEngine;

public class SunLight : MonoBehaviour {
    private float worldHour;

    private void Start() {
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    private void Update() {
        worldHour = TimeManager.Instance.GetWorldHour();

        if (worldHour >= 6 && worldHour < 20) {
            float dayHours = 20f - 6f; // �� �ð����� �� �ð�
            float currentDayHour = worldHour - 6f; 
            float dayAngle = 180f * currentDayHour / dayHours;
            transform.rotation = Quaternion.Euler(dayAngle, dayAngle, 0);
        }
        else {
            float nightHours = 24f - 20f + 6f; // �� �ð����� �� �ð� (4�ð� + 6�ð�)
            float currentNightHour = (worldHour >= 20) ? worldHour - 20f : worldHour + 4f;
            float nightAngle = 180f * currentNightHour / nightHours;
            transform.rotation = Quaternion.Euler(nightAngle + 180f, nightAngle + 180f, 0);
        }

        //TODO: 6~7��, 22~23�� ��Ȳ ����
        //TODO: 23�� ~ 6�� ��Ǫ�� ����, 7�� ~ 22�� �� ����

    }
}
