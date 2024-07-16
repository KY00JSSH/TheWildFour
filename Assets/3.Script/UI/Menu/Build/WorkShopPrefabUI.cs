using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkShopPrefabUI : BuildPrefabUI {

    private WorkshopCreate workshopCreate;
    protected override void Awake() {
        base.Awake();
        workshopCreate = FindObjectOfType<WorkshopCreate>();
    }

    protected override void Update() {
        if (buildingObj != null) {
            isValid = workshopCreate.isValidBuild;
            base.Update();
        }
    }
    // 버튼이 눌렸을 경우 UI표시
    public void BuildAvailableMode() {
        if (!build_Tooltip.isStartBuildingNumCheck) return;

        if (workshopCreate.isExist) return;
        buildingObj = workshopCreate.Building;
        isBuiltStart = true;
        BuildImg.SetActive(true);
    }
}
