using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelterPrefabUI : BuildPrefabUI {

    private ShelterCreate shelterCreate;
    protected override void Awake() {
        base.Awake();
        shelterCreate = FindObjectOfType<ShelterCreate>();
    }

    protected override void Update() {
        if(buildingObj != null) {
            base.Update();
        }
    }

    // 버튼이 눌렸을 경우 UI표시
    public void ShelterBuildAvailable() {
        if (buildingCreate.isExist) return;
        buildingObj = shelterCreate.Building;
        isBuiltStart = true;
        BuildImg.SetActive(true);
    }

}
