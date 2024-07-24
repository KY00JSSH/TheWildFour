using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampfireUI : PrefabSliderUI {


    protected override void Awake() {
        base.Awake();
        objectRenderer = transform.GetChild(3).GetComponent<Renderer>();
        parent = canvas.transform.Find("Etc").Find("FireSliderTotal").gameObject;
        if (parent == null) {
            Debug.Log("parent ¾øÀ½");
        }
        widthDelta = 1f;
        heightDelta = 0.1f;
    }
    private void Start() {
        totalvalue = GetComponent<Campfire>().GetTotalTime();
    }
    protected override void Update() {
        currentvalue = GetComponent<Campfire>().GetCurrentTime();
        base.Update();
    }
}
