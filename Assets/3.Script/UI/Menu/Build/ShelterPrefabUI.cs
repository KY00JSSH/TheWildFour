using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelterPrefabUI : BuildPrefabUI {
    /* 
      1. 활성화되어있는 상위 오브젝트 찾기
      2. 상위 오브젝트에서 활성화 되어있는 오브젝트 찾기
      */

    private GameObject parentbuildingObj;
    private ShelterCreate shelterCreate;
    protected override void Awake() {
        base.Awake();
        shelterCreate = FindObjectOfType<ShelterCreate>();
        if (shelterCreate != null) {
            parentbuildingObj = shelterCreate.gameObject;
            Debug.Log("ShelterCreate component " + parentbuildingObj.name);
        }
        else {
            Debug.LogError("ShelterCreate component 없음");
        }

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
        //buildImg.gameObject.SetActive(true);
    }

}
