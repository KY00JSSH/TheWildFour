using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelterPrefabUI : BuildPrefabUI {

    private ShelterCreate shelterCreate;
    protected override void Awake() {
        base.Awake();
        shelterCreate = FindObjectOfType<ShelterCreate>();
    }

    protected override void Update() {
        if(buildingObj != null) {
            isValid = shelterCreate.isValidBuild;
            base.Update();

        }
    }

    // ��ư�� ������ ��� UIǥ��
    public void ShelterBuildAvailable() {
        if (shelterCreate.isExist) return;
        buildingObj = shelterCreate.Building;
        isBuiltStart = true;
        BuildImg.SetActive(true);
    }

}
