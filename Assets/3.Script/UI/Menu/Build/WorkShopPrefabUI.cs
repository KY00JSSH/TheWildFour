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

    // ��ư�� ������ ��� UIǥ��
    public void WorkshopBuildAvailable() {
        if (workshopCreate.isExist) return;
        buildingObj = workshopCreate.Building;
        isBuiltStart = true;
        BuildImg.SetActive(true);        
    }

}
