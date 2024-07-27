using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefabSliderUI : MonoBehaviour {
    private Transform playerTransform;

    [Space((int)2)]
    [Header("Slider Prefab")] // 슬라이더 프리펩
    [SerializeField] protected GameObject SliderPrefab;

    protected GameObject parent;
    protected Canvas canvas;

    // 슬라이더 복제용 오브젝트 할당
    protected GameObject sliderObj;
    protected Coroutine fadeCoroutine;
    protected Slider slider;

    protected Renderer objectRenderer;

    // 슬라이더 전체 값과 현재 값은 하위 스크립트에서 할당 필요함
    protected float totalvalue;
    protected float currentvalue;

    // 슬라이더 보정값
    protected float widthDelta;
    protected float heightDelta;

    protected virtual void Awake() {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        canvas = FindObjectOfType<Canvas>();

        // parent 는 각자 스크립트에서 찾아야함
    }


    protected virtual void Update() {
        if (sliderObj != null) {
            SettingSliderPosition();
            SettingSliderSize(widthDelta, heightDelta);
            SliderValueCal();
        }
    }

    // 슬라이더 생성
    public void SliderInit() {
        if (parent != null) {
            sliderObj = Instantiate(SliderPrefab, parent.transform);
            sliderObj.name = SliderPrefab.name;
            sliderObj.SetActive(true);
        }
    }

    protected virtual void SettingSliderPosition() {
        RectTransform SliderPosition = sliderObj.GetComponent<RectTransform>();
        if (objectRenderer != null) {
            Vector3 size = objectRenderer.bounds.size;
            Vector3 center = objectRenderer.bounds.center;
            Vector3 topCenter = new Vector3(center.x, center.y + size.y / 2, center.z);

            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, topCenter + new Vector3(0, 0.3f, 0));
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPoint, canvas.worldCamera, out Vector2 localPoint);
            SliderPosition.localPosition = localPoint;
        }

    }

    protected virtual void SettingSliderSize(float widthDelta, float heightDelta) {
        RectTransform fireSliderPosition = sliderObj.GetComponent<RectTransform>();
        if (objectRenderer != null) {
            Bounds bounds = objectRenderer.bounds;
            Vector3 screenMin = Camera.main.WorldToScreenPoint(bounds.min);
            Vector3 screenMax = Camera.main.WorldToScreenPoint(bounds.max);

            float width = Mathf.Abs(screenMax.x - screenMin.x);
            float height = Mathf.Abs(screenMax.y - screenMin.y);

            fireSliderPosition.sizeDelta = new Vector2(width * widthDelta, height * heightDelta);
        }
    }

    protected virtual void SliderValueCal() {
        slider = sliderObj.GetComponent<Slider>();
        float sliderValue = currentvalue / totalvalue;
        slider.value = sliderValue;
        SliderDisappear();
    }


    protected virtual void SliderDisappear() {
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

    protected virtual IEnumerator SliderDisappear_co() {
        Image fillImage = slider.fillRect.GetComponent<Image>();  // Fill 이미지 가져오기
        Color newColor = fillImage.color;

        while (newColor.a > 0.5f) {  // 알파값이 0보다 클 동안 반복
            newColor.a -= 0.01f;  // 알파값을 감소
            fillImage.color = newColor;
            yield return null;
        }
        slider.gameObject.SetActive(false);
    }

    protected virtual void SliderAlphaInit() {
        slider.gameObject.SetActive(true);
        Image fillImage = slider.fillRect.GetComponent<Image>();  // Fill 이미지 가져오기
        Color newColor = fillImage.color;
        newColor.a = 1;
        fillImage.color = newColor;
    }

}
