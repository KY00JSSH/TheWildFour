using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelterPrefabUI : BuildPrefabUI {

    public ShelterCreate shelterCreate;
    protected override void Awake() {
        base.Awake();
        //shelterCreate = FindObjectOfType<ShelterCreate>();
    }

    protected override void Update() {
        if (buildingObj != null) {
            if (buildingObj.activeSelf) {
                isValid = shelterCreate.isValidBuild;
                base.Update();

            }
        }

    }

    // ��ư�� ������ ��� UIǥ��
    public override void BuildAvailableMode() {
        buildingCreate = shelterCreate;
        base.BuildAvailableMode();
    }
}

