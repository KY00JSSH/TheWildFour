using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceUI : PrefabSliderUI {

    protected override void Awake() {
        base.Awake();
        parent = canvas.transform.Find("Etc").Find("FurnaceSliderTotal").gameObject;
        if (parent == null) {
            Debug.Log("parent 없음");
        }
    }

    private void Start() {
        //TODO: totalvalue 설정 해줘야함
        
    }
    protected override void Update() {
        //TODO: currentvalue 설정 해줘야함

        base.Update();
    }
}
