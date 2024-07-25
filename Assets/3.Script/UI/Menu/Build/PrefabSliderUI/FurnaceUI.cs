using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceUI : PrefabSliderUI {

    protected override void Awake() {
        base.Awake();
        objectRenderer = transform.GetChild(3).GetComponent<Renderer>();
        parent = canvas.transform.Find("Etc").Find("FurnaceSliderTotal").gameObject;
        if (parent == null) {
            Debug.Log("parent ¾øÀ½");
        }
        widthDelta = 0.8f;
        heightDelta = 0.08f;
    }

    private void Start() {
        totalvalue = GetComponent<Furnace>().GetTotalTime();
    }
    protected override void Update() {
        currentvalue = GetComponent<Furnace>().GetCurrentTime();
        base.Update();
    }
}
