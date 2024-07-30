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
                BuildImg.SetActive(true);
                isValid = campfireCreate.isValidBuild;
                buildAnimator = buildingObj.GetComponent<Animator>();
                base.Update();
            }
            else {
                BuildImg.SetActive(false);
            }
        }
    }

    public override void BuildAvailableMode() {
        buildingCreate = campfireCreate;
        base.BuildAvailableMode();
    }
}
