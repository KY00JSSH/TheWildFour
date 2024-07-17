using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampfireUI : MonoBehaviour {
    private Transform playerTransform;
    private FireObject fireObject;

    [Space((int)2)]
    [Header("Slider UI")]
    [SerializeField] private GameObject fireSliderPrefab;

    [SerializeField] private GameObject parent;
    [SerializeField] private Canvas canvas;

    [SerializeField] private int Add_Y = 30;

    GameObject sliderObj;

    private void Awake() {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        fireObject = FindObjectOfType<FireObject>();
        canvas = FindObjectOfType<Canvas>();
        parent = canvas.transform.Find("Etc").GetChild(1).gameObject;
        if (parent == null) {
            Debug.Log("parent 없음");
        }
    }
    private void Update() {
        if(sliderObj != null) {
            SettingFireSliderPosition();
            SettingFireSliderSize();
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
        if (fireSliderPosition == null) Debug.Log("????????????????");
        Renderer fireObjectRenderer = transform.GetChild(5).GetComponent<Renderer>();
        if (fireObjectRenderer != null) {
            Vector3 size = fireObjectRenderer.bounds.size;
            Vector3 center = fireObjectRenderer.bounds.center;
            Vector3 topCenter = new Vector3(center.x, center.y + size.y / 2, center.z);

            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, topCenter + new Vector3(0, 0.3f, 0));
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPoint, canvas.worldCamera, out Vector2 localPoint);
            fireSliderPosition.localPosition = localPoint;
            sliderObj.GetComponent<Slider>().value = SliderValueCal();
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

    private float SliderValueCal() {
        float sliderValue = GetComponent<Campfire>().GetCurrentTime() / GetComponent<Campfire>().GetTotalTime();
        return sliderValue;
    }




}
