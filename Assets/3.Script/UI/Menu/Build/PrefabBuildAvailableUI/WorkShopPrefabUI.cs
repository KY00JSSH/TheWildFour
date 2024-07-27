using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkShopPrefabUI : BuildPrefabUI {

    public WorkshopCreate workshopCreate;
    protected override void Awake() {
        base.Awake();
        workshopCreate = FindObjectOfType<WorkshopCreate>();
    }

    protected override void Update() {
        if(buildingObj != null) {
            if (buildingObj.activeSelf) {
                isValid = workshopCreate.isValidBuild;
                base.Update();
            }
        }
    }

    public override void BuildAvailableMode() {
        buildingCreate = workshopCreate;
        base.BuildAvailableMode();
    }
}

