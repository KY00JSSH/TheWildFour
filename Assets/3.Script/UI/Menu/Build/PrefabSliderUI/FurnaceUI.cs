using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceUI : PrefabSliderUI {

    protected override void Awake() {
        base.Awake();
        objectRenderer = transform.GetChild(3).GetComponent<Renderer>();
        parent = canvas.transform.Find("Etc").Find("FurnaceSliderTotal").gameObject;
        if (parent == null) {
            Debug.Log("parent 없음");
        }
        widthDelta = 0.8f;
        heightDelta = 0.08f;
    }

    private void Start() {
        //TODO: totalvalue 설정 해줘야함
        totalvalue = GetComponent<Furnace>().GetTotalTime();
    }
    protected override void Update() {
        //TODO: currentvalue 설정 해줘야함
        currentvalue = GetComponent<Furnace>().GetCurrentTime();
        base.Update();
    }
}
