using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelterPrefabUI : BuildPrefabUI {

    public ShelterCreate shelterCreate;
    protected override void Awake() {
        base.Awake();
        //shelterCreate = FindObjectOfType<ShelterCreate>();
        buildAnimationName = "ShelterCreate";
    }

    protected override void Update() {
        if (buildingObj != null) {
            if (buildingObj.activeSelf) {
                BuildImg.SetActive(true);
                isValid = shelterCreate.isValidBuild;
                buildAnimator = buildingObj.GetComponentInParent<Animator>();
                base.Update();

            }
            else {
                BuildImg.SetActive(false);
            }
        }

    }

    // 버튼이 눌렸을 경우 UI표시
    public override void BuildAvailableMode() {
        buildingCreate = shelterCreate;
        base.BuildAvailableMode();
    }
}

