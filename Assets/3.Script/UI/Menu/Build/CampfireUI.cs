using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampfireSliderList {
    public GameObject campfireSlPfb;
    public Vector3 campfireSlPos;
}


public class CampfireUI : MonoBehaviour {
    private Transform playerTransform;
    private FireObject fireObject;

    [Space((int)2)]
    [Header("Slider UI")]
    [SerializeField] private GameObject fireSliderPrefab;

    private bool isCamfireBuild = false;

    public Canvas canvas;

    private int fireObjsNum = 0;
    private int fireSlidersNum = 0;
    [SerializeField] private int Add_Y = 30;

    private void Awake() {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        fireObject = FindObjectOfType<FireObject>();
    }


    // 슬라이더 생성
    private void FireSliderInit(int fireObjsNum) {
        GameObject sliderObj = Instantiate(fireSliderPrefab, canvas.transform);
        sliderObj.name = fireSliderPrefab.name;
        CampfireSliderList newFireSlider = new CampfireSliderList {
            campfireSlPfb = sliderObj,
            campfireSlPos = transform.position
        };
        Debug.Log(" 슬라이터 프리펩 수량 " + fireSlidersNum);

    }

    private void SettingFireSliderPosition(int fireSlidersNum) {
        RectTransform fireSliderPosition = fireSliders[fireSlidersNum].campfireSlPfb.GetComponent<RectTransform>();
        Renderer fireObjectRenderer = transform.GetChild(5).GetComponent<Renderer>();
        if (fireObjectRenderer != null) {
            Vector3 size = fireObjectRenderer.bounds.size;
            Vector3 center = fireObjectRenderer.bounds.center;
            Vector3 topCenter = new Vector3(center.x, center.y + size.y / 2, center.z);

            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, topCenter + new Vector3(0, 0.3f, 0));
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPoint, canvas.worldCamera, out Vector2 localPoint);
            fireSliderPosition.localPosition = localPoint;
            fireSliders[fireSlidersNum].campfireSlPos = fireSliderPosition.localPosition;
            fireSliders[fireSlidersNum].campfireSlPfb.GetComponent<Slider>().value = SliderValueCal(fireSlidersNum);
        }
    }

    private void SettingFireSliderSize(int fireSlidersNum) {
        RectTransform fireSliderPosition = fireSliders[fireSlidersNum].campfireSlPfb.GetComponent<RectTransform>();
        Renderer fireObjectRenderer = fireObjects[fireSlidersNum].transform.GetChild(5).GetComponent<Renderer>();
        if (fireObjectRenderer != null) {
            Bounds bounds = fireObjectRenderer.bounds;
            Vector3 screenMin = Camera.main.WorldToScreenPoint(bounds.min);
            Vector3 screenMax = Camera.main.WorldToScreenPoint(bounds.max);

            float width = Mathf.Abs(screenMax.x - screenMin.x);
            float height = Mathf.Abs(screenMax.y - screenMin.y);

            fireSliderPosition.sizeDelta = new Vector2(width, height * 0.1f);
        }
    }

    private float SliderValueCal(int i) {
        float sliderValue = fireObjects[i].GetComponent<FireObject>().GetCurrentTime() / fireObjects[i].GetComponent<FireObject>().GetTotalTime();
        return sliderValue;
    }




}
