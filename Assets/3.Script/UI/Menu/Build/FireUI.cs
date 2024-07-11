using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireUI : MonoBehaviour {
    public GameObject player;
    [SerializeField] private GameObject fireSliderPrefab;
    [SerializeField] private List<GameObject> fireObjects;
    [SerializeField] private List<GameObject> fireSliders;
    private int fireObjsNum = 0;
    private int fireSlidersNum = 0;

    private void FixedUpdate() {
        fireObjsNum = FireObjectNumCheck();
        fireSlidersNum = fireSliders.Count;

        if (fireObjsNum != fireSlidersNum) {
            FireObjectInit(fireObjsNum);
        }

        SettingFireSliderPosition();

        SettingFireSliderSize(); ;
    }

    private int FireObjectNumCheck() {
        FireObject[] fireObjs = FindObjectsOfType<FireObject>();
        fireObjects.Clear();
        foreach (FireObject fireObj in fireObjs) {
            fireObjects.Add(fireObj.gameObject);
        }
        return fireObjs.Length;
    }

    private void FireObjectInit(int fireObjsNum) {
        if (fireObjsNum > fireSliders.Count) {
            for (int i = fireSliders.Count; i < fireObjsNum; i++) {
                GameObject newFireSlider = Instantiate(fireSliderPrefab, transform);
                newFireSlider.name = fireSliderPrefab.name;
                fireSliders.Add(newFireSlider);
            }
        }
    }


    private void SettingFireSliderPosition() {
        for (int i = 0; i < fireSliders.Count; i++) {
            RectTransform fireSliderPosition = fireSliders[i].GetComponent<RectTransform>();
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(fireObjects[i].transform.position);

            // 화면상의 멀리있는 물체와 가까이 있는 물체의 슬라이더의 y좌표 보정값
            float depthFactor = Mathf.Clamp(15f / screenPosition.z, 0.7f, 5f);
            screenPosition.y += 50f * depthFactor;

            fireSliderPosition.position = screenPosition;

            fireSliders[i].GetComponent<Slider>().value = SliderValueCal(i);
        }
    }

    private void SettingFireSliderSize() {
        for (int i = 0; i < fireSliders.Count; i++) {
            RectTransform fireSliderPosition = fireSliders[i].GetComponent<RectTransform>();
            Renderer fireObjectRenderer = fireObjects[i].transform.GetChild(0).GetComponent<Renderer>();
            if (fireObjectRenderer != null) {
                Bounds bounds = fireObjectRenderer.bounds;
                Vector3 screenMin = Camera.main.WorldToScreenPoint(bounds.min);
                Vector3 screenMax = Camera.main.WorldToScreenPoint(bounds.max);
                float width = Mathf.Abs(screenMax.x - screenMin.x);
                float height = Mathf.Abs(screenMax.y - screenMin.y);

                fireSliderPosition.sizeDelta = new Vector2(width, height / 5);
            }
        }
    }


    private float SliderValueCal(int i) {
        float sliderValue = fireObjects[i].GetComponent<FireObject>().GetCurrentTime() / fireObjects[i].GetComponent<FireObject>().GetTotalTime();
        return sliderValue;
    }



    private bool IsFireNearPlayer(FireObject eachitem) {
        if (Vector3.Distance(player.transform.position, eachitem.transform.position) <= 3f) {
            return true;
        }
        else return false;
    }


}
