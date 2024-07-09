using UnityEngine;

public class SunLight : MonoBehaviour {
    private float worldHour;

    private void Start() {
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    private void Update() {
        worldHour = TimeManager.Instance.GetWorldHour();

        if (worldHour >= 6 && worldHour < 20) {
            float dayHours = 20f - 6f; // ³· ½Ã°£´ëÀÇ ÃÑ ½Ã°£
            float currentDayHour = worldHour - 6f; 
            float dayAngle = 180f * currentDayHour / dayHours;
            transform.rotation = Quaternion.Euler(dayAngle, dayAngle, 0);
        }
        else {
            float nightHours = 24f - 20f + 6f; // ¹ã ½Ã°£´ëÀÇ ÃÑ ½Ã°£ (4½Ã°£ + 6½Ã°£)
            float currentNightHour = (worldHour >= 20) ? worldHour - 20f : worldHour + 4f;
            float nightAngle = 180f * currentNightHour / nightHours;
            transform.rotation = Quaternion.Euler(nightAngle + 180f, nightAngle + 180f, 0);
        }
    }
}
