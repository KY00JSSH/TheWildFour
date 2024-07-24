using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefabSliderUI : MonoBehaviour {
    private Transform playerTransform;

    [Space((int)2)]
    [Header("Slider Prefab")] // �����̴� ������
    [SerializeField] protected GameObject SliderPrefab;

    protected GameObject parent;
    protected Canvas canvas;

    // �����̴� ������ ������Ʈ �Ҵ�
    protected GameObject sliderObj;
    protected Coroutine fadeCoroutine;
    protected Slider slider;

    protected Renderer objectRenderer;

    // �����̴� ��ü ���� ���� ���� ���� ��ũ��Ʈ���� �Ҵ� �ʿ���
    protected float totalvalue;
    protected float currentvalue;

    // �����̴� ������
    protected float widthDelta;
    protected float heightDelta;

    protected virtual void Awake() {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        canvas = FindObjectOfType<Canvas>();

        // parent �� ���� ��ũ��Ʈ���� ã�ƾ���
    }


    protected virtual void Update() {
        if (sliderObj != null) {
            SettingSliderPosition();
            SettingSliderSize(widthDelta, heightDelta);
            SliderValueCal();
        }
    }

    // �����̴� ����
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
                fadeCoroutine = null; // �ڷ�ƾ ������ ���� null�� ����
            }
        }
    }

    protected virtual IEnumerator SliderDisappear_co() {
        Image fillImage = slider.fillRect.GetComponent<Image>();  // Fill �̹��� ��������
        Color newColor = fillImage.color;

        while (newColor.a > 0.5f) {  // ���İ��� 0���� Ŭ ���� �ݺ�
            newColor.a -= 0.01f;  // ���İ��� ����
            fillImage.color = newColor;
            yield return null;
        }
        slider.gameObject.SetActive(false);
    }

    protected virtual void SliderAlphaInit() {
        slider.gameObject.SetActive(true);
        Image fillImage = slider.fillRect.GetComponent<Image>();  // Fill �̹��� ��������
        Color newColor = fillImage.color;
        newColor.a = 1;
        fillImage.color = newColor;
    }

}
