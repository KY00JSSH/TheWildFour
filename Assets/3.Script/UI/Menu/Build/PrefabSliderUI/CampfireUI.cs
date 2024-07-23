using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampfireUI : PrefabSliderUI {


    protected override void Awake() {
        base.Awake();
        parent = canvas.transform.Find("Etc").Find("FireSliderTotal").gameObject;
        if (parent == null) {
            Debug.Log("parent ¾øÀ½");
        }
    }
    private void Start() {
        totalvalue = GetComponent<Campfire>().GetTotalTime();
    }
    protected override void Update() {
        currentvalue = GetComponent<Campfire>().GetCurrentTime();
        base.Update();
    }
}
