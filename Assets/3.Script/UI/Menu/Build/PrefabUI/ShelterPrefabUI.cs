using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelterPrefabUI : BuildPrefabUI {

    public ShelterCreate shelterCreate;
    protected override void Awake() {
        base.Awake();
        //shelterCreate = FindObjectOfType<ShelterCreate>();
    }

    protected override void Update() {
        if (buildingObj != null) {
            isValid = shelterCreate.isValidBuild;
            base.Update();

        }

    }


    // 버튼이 눌렸을 경우 UI표시
    public void BuildAvailableMode() {
        if (!tooltip_Build.isBuildAvailable) return;

        if (shelterCreate.isExist) return;
        buildingObj = shelterCreate.Building;
        StartCoroutine(FindObject());
    }
    protected IEnumerator FindObject()
    {
        while (buildingObj == null)
        {
            buildingObj = shelterCreate.Building;
            yield return null;
        }

        Debug.Log("==========================" + buildingObj.name);
        isBuiltStart = true;
        BuildImg.SetActive(true);
    }

}

