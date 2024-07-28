using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfirePrefabUI : BuildPrefabUI {
    [SerializeField]private CampfireChestCreate campfireCreate;
    [SerializeField] private string _buildAnimationName;
    protected override void Awake() {
        base.Awake();
        buildAnimationName = _buildAnimationName;
    }

    protected override void Update() {
        if (buildingObj != null) {
            if (buildingObj.activeSelf) {
                isValid = campfireCreate.isValidBuild;
                buildAnimator = buildingObj.GetComponent<Animator>();
                base.Update();
            }
        }
    }

    public override void BuildAvailableMode() {
        buildingCreate = campfireCreate;
        isBuildAniComplete = false;
        playTime = 0;
        base.BuildAvailableMode();
    }
}
