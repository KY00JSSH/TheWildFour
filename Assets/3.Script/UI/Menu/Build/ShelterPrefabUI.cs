using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelterPrefabUI : BuildPrefabUI {
    /* 
      1. 활성화되어있는 상위 오브젝트 찾기
      2. 상위 오브젝트에서 활성화 되어있는 오브젝트 찾기
      3. 
      */

    private GameObject parentbuildingObj;
    ShelterCreate shelterCreate;
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
            //Debug.Log("??????????????????");
            base.Update();
        }

        //if(buildingCreate.isValidBuild) buildImg.
    }

    // 버튼이 눌렸을 경우 UI표시
    public void ShelterBuildAvailable() {
        Debug.Log("?여기는 들어오는뎅?");
        buildingObj = shelterCreate.Building;
        isBuiltStart = true;
        buildImg.gameObject.SetActive(true);
    }


}
