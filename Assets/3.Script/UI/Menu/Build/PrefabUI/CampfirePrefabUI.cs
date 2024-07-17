using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfirePrefabUI : BuildPrefabUI {
    private CampfireCreate campfireCreate;
    protected override void Awake() {
        base.Awake();
        campfireCreate = FindObjectOfType<CampfireCreate>();
    }

    protected override void Update() {
        if (buildingObj != null) {
            isValid = campfireCreate.isValidBuild;
            base.Update();
        }
    }

    public void BuildAvaildableMode() {
        //if (!build_Tooltip.isBuildAvailable) return;
        if (campfireCreate.isExist) return;
        buildingObj = campfireCreate.Building;
        isBuiltStart = true;
        BuildImg.SetActive(true);
    }
}
