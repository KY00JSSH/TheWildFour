using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceUI : PrefabSliderUI {

    protected override void Awake() {
        base.Awake();
        parent = canvas.transform.Find("Etc").Find("FurnaceSliderTotal").gameObject;
        if (parent == null) {
            Debug.Log("parent ����");
        }
    }

    private void Start() {
        //TODO: totalvalue ���� �������
        
    }
    protected override void Update() {
        //TODO: currentvalue ���� �������

        base.Update();
    }
}
