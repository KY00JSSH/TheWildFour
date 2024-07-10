using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireUI : MonoBehaviour {
    public GameObject player;
    public Slider fireSlider;
    private List<Slider> activeSlider;
    private List<Slider> offSlider;
    private Dictionary<FireObject, Slider> sliders = new Dictionary<FireObject, Slider>();

    private void Update() {
        SliderInit();
    }

    private void SliderInit() {
        FireObject[] fireObjs = FindObjectsOfType<FireObject>();
        foreach (FireObject eachitem in fireObjs) {
            if (!sliders.ContainsKey(eachitem)) { // Dictionary에 저장해서 슬라이더가 있는지 확인
                Slider fireSliders = Instantiate(fireSlider);
                fireSliders.transform.SetParent(eachitem.transform, true);
                fireSliders.value = eachitem.GetCurrentTime() / eachitem.GetTotalTime();
                fireSliders.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(eachitem.transform.position);
                if (IsFireNearPlayer(eachitem)) {
                    activeSlider.Add(fireSliders);
                }
                else {
                    offSlider.Add(fireSliders);
                }
                sliders[eachitem] = fireSliders;
                fireSliders.gameObject.SetActive(true);
            }
        }
    }


    private bool IsFireNearPlayer(FireObject eachitem) {
        if (Vector3.Distance(player.transform.position, eachitem.transform.position) <= 3f) {
            return true;
        }
        else return false;
    }

    private void IsActiveSliderShow() {
        for (int i = 0; i < activeSlider.Count; i++) {
            activeSlider[i].gameObject.SetActive(true);
        }
    }


}
