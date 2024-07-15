using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelterPrefabUI : BuildPrefabUI {
    /* 
      1. Ȱ��ȭ�Ǿ��ִ� ���� ������Ʈ ã��
      2. ���� ������Ʈ���� Ȱ��ȭ �Ǿ��ִ� ������Ʈ ã��
      */

    private GameObject parentbuildingObj;
    private ShelterCreate shelterCreate;
    protected override void Awake() {
        base.Awake();
        shelterCreate = FindObjectOfType<ShelterCreate>();
        if (shelterCreate != null) {
            parentbuildingObj = shelterCreate.gameObject;
            Debug.Log("ShelterCreate component " + parentbuildingObj.name);
        }
        else {
            Debug.LogError("ShelterCreate component ����");
        }

    }

    protected override void Update() {
        if(buildingObj != null) {
            base.Update();
        }
    }

    // ��ư�� ������ ��� UIǥ��
    public void ShelterBuildAvailable() {
        if (buildingCreate.isExist) return;
        buildingObj = shelterCreate.Building;
        isBuiltStart = true;
        BuildImg.SetActive(true);
        //buildImg.gameObject.SetActive(true);
    }

}
