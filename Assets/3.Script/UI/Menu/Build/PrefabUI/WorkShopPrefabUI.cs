using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkShopPrefabUI : BuildPrefabUI {

    public WorkshopCreate workshopCreate;
    protected override void Awake() {
        base.Awake();
        //workshopCreate = FindObjectOfType<WorkshopCreate>();
    }

    protected override void Update() {
        if (buildingObj != null) {
            isValid = workshopCreate.isValidBuild;
            base.Update();
        }

    }
    // 버튼이 눌렸을 경우 UI표시
    public void BuildAvailableMode() {
        Debug.Log(" build 확인"+ tooltip_Build.isBuildAvailable);
        if (!tooltip_Build.isBuildAvailable) return;

        if (workshopCreate.isExist) return;
        buildingObj = workshopCreate.Building;
        StartCoroutine(FindObject());
    }
    protected IEnumerator FindObject()
    {
        while (buildingObj == null)
        {
            buildingObj = workshopCreate.Building;
            yield return null;
        }

        Debug.Log("==========================" + buildingObj.name);
        isBuiltStart = true;
        BuildImg.SetActive(true);
    }
}

