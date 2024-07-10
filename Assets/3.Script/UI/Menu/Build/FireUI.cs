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
            Debug.Log("fireSliders[i] 위치" + fireSliders[i].transform.position);
            Debug.Log("fireObjects[i] 위치" + fireObjects[i].transform.position);

            RectTransform fireSliderPosition = fireSliders[i].GetComponent<RectTransform>();
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(fireObjects[i].transform.position);
            screenPosition.y += 50f;
            fireSliderPosition.position = screenPosition;
            fireSliders[i].GetComponent<Slider>().value = SliderValueCal(i);
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
