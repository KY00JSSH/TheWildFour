using System.Collections;
using UnityEngine;

public class SunLight : MonoBehaviour {
    private Light sunLight;
    private float worldHour;

    private Color dayColor = new Color(200f / 255f, 215f / 255f, 240f / 255f);
    private Color orangeColor = new Color(200f / 255f, 70f / 255f, 0f / 255f);
    private Color nightColor = new Color(0f / 255f, 0f / 255f, 70f / 255f);

    private Color currentColor;
    private float transitionDuration = 8f;
    private float transitionProgress = 0f;

    private bool isChanging = false;

    private void Awake() {
        sunLight = GetComponent<Light>();
    }

    private void Start() {
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        currentColor = dayColor;
        sunLight.color = currentColor;
    }

    private void Update() {
        worldHour = TimeManager.Instance.GetWorldHour();

        if (worldHour >= 6 && worldHour < 23) {
            float dayHours = 23f - 6f; // �� �ð����� �� �ð�
            float currentDayHour = worldHour - 6f;
            float dayAngle = 180f * currentDayHour / dayHours;
            transform.rotation = Quaternion.Euler(dayAngle, dayAngle, 0);
        }
        else {
            float nightHours = 24f - 23f + 6f; // �� �ð����� �� �ð� (4�ð� + 6�ð�)
            float currentNightHour = (worldHour >= 23) ? worldHour - 23f : worldHour + 1f;
            float nightAngle = 180f * currentNightHour / nightHours;
            transform.rotation = Quaternion.Euler(nightAngle + 180f, nightAngle + 180f, 0);
        }

        if (!isChanging) {
            if (TimeManager.Instance.isOrangeSky) {
                StartCoroutine(ChangeSkyColor(orangeColor));
            }
            else if (worldHour > 6 && worldHour < 23)
                StartCoroutine(ChangeSkyColor(dayColor));
            else
                StartCoroutine(ChangeSkyColor(nightColor));
        }

        //TODO: 6~7��, 22~23�� ��Ȳ ����
        //TODO: 23�� ~ 6�� ��Ǫ�� ����, 7�� ~ 22�� �� ����

    }

    public IEnumerator ChangeSkyColor(Color targetColor) {
        isChanging = true;
        if (currentColor != targetColor) {
            while (transitionProgress < 1f) {
                transitionProgress += Time.deltaTime / transitionDuration;
                currentColor = Color.Lerp(currentColor, targetColor, transitionProgress);
                sunLight.color = currentColor;
                yield return null;
            }
            currentColor = targetColor;
            transitionProgress = 0f;
        }
        isChanging = false;
    }
}
