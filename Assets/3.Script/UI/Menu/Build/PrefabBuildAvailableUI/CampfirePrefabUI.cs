using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfirePrefabUI : BuildPrefabUI {
    [SerializeField]private CampfireChestCreate campfireCreate;
    protected override void Awake() {
        base.Awake();
    }

    protected override void Update() {
        if (buildingObj != null) {
            if (buildingObj.activeSelf) {
                isValid = campfireCreate.isValidBuild;
                base.Update();
            }
        }
    }

    public override void BuildAvailableMode() {
        buildingCreate = campfireCreate;
        base.BuildAvailableMode();
    }
}
