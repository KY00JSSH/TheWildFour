using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampfireUI : MonoBehaviour {
    private Transform playerTransform;

    [Space((int)2)]
    [Header("Slider UI")]
    [SerializeField] private GameObject fireSliderPrefab;

    private GameObject parent;
    private Canvas canvas;

    private Coroutine fadeCoroutine;
    private Slider slider;
    private GameObject sliderObj;


    private bool isStart = false;

    private void Awake() {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        canvas = FindObjectOfType<Canvas>();
        parent = canvas.transform.Find("Etc").GetChild(1).gameObject;
        if (parent == null) {
            Debug.Log("parent 없음");
        }
    }
    private void Update() {
        if (sliderObj != null) {
            SettingFireSliderPosition();
            SettingFireSliderSize();
            SliderValueCal();
        }

    }

    // 슬라이더 생성
    public void FireSliderInit() {
        if (parent != null) {
            sliderObj = Instantiate(fireSliderPrefab, parent.transform);
            sliderObj.name = fireSliderPrefab.name;
        }
    }

    private void SettingFireSliderPosition() {
        RectTransform fireSliderPosition = sliderObj.GetComponent<RectTransform>();
        Renderer fireObjectRenderer = transform.GetChild(5).GetComponent<Renderer>();
        if (fireObjectRenderer != null) {
            Vector3 size = fireObjectRenderer.bounds.size;
            Vector3 center = fireObjectRenderer.bounds.center;
            Vector3 topCenter = new Vector3(center.x, center.y + size.y / 2, center.z);

            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, topCenter + new Vector3(0, 0.3f, 0));
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPoint, canvas.worldCamera, out Vector2 localPoint);
            fireSliderPosition.localPosition = localPoint;
        }
    }

    private void SettingFireSliderSize() {
        RectTransform fireSliderPosition = sliderObj.GetComponent<RectTransform>();
        Renderer fireObjectRenderer = transform.GetChild(5).GetComponent<Renderer>();
        if (fireObjectRenderer != null) {
            Bounds bounds = fireObjectRenderer.bounds;
            Vector3 screenMin = Camera.main.WorldToScreenPoint(bounds.min);
            Vector3 screenMax = Camera.main.WorldToScreenPoint(bounds.max);

            float width = Mathf.Abs(screenMax.x - screenMin.x);
            float height = Mathf.Abs(screenMax.y - screenMin.y);

            fireSliderPosition.sizeDelta = new Vector2(width, height * 0.1f);
        }
    }

    private void SliderValueCal() {

        slider = sliderObj.GetComponent<Slider>();
        float sliderValue = GetComponent<Campfire>().GetCurrentTime() / GetComponent<Campfire>().GetTotalTime();
        slider.value = sliderValue;
        SliderDisappear();
    }


    private void SliderDisappear() {
        if (slider.value == 0) {
            fadeCoroutine = StartCoroutine(SliderDisappear_co());
        }
        else {
            SliderAlphaInit();
            if (fadeCoroutine != null) {
                StopCoroutine(fadeCoroutine);
                fadeCoroutine = null; // 코루틴 추적을 위해 null로 설정
            }
        }
    }

    private IEnumerator SliderDisappear_co() {
        Image fillImage = slider.fillRect.GetComponent<Image>();  // Fill 이미지 가져오기
        Color newColor = fillImage.color;

        while (newColor.a > 0.5f) {  // 알파값이 0보다 클 동안 반복
            newColor.a -= 0.01f;  // 알파값을 감소
            fillImage.color = newColor;
            yield return null;
        }
        slider.gameObject.SetActive(false);
    }

    private void SliderAlphaInit() {
        slider.gameObject.SetActive(true);
        Image fillImage = slider.fillRect.GetComponent<Image>();  // Fill 이미지 가져오기
        Color newColor = fillImage.color;
        newColor.a = 1;
        fillImage.color = newColor;
    }
}
