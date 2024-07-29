using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkShopPrefabUI : BuildPrefabUI {

    public WorkshopCreate workshopCreate;
    protected override void Awake() {
        base.Awake();
        workshopCreate = FindObjectOfType<WorkshopCreate>();
        buildAnimationName = "WorkshopCreate";
    }

    protected override void Update() {
        if(buildingObj != null) {
            if (buildingObj.activeSelf) {
                BuildImg.SetActive(true);
                isValid = workshopCreate.isValidBuild;
                buildAnimator = buildingObj.GetComponentInParent<Animator>();
                base.Update();
            }
            else {
                BuildImg.SetActive(false);
            }
        }
    }

    public override void BuildAvailableMode() {
        buildingCreate = workshopCreate;
        base.BuildAvailableMode();
    }
}

