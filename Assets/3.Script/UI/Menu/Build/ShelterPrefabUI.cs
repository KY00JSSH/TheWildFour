using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelterPrefabUI : BuildPrefabUI {
    /* 
      1. Ȱ��ȭ�Ǿ��ִ� ���� ������Ʈ ã��
      2. ���� ������Ʈ���� Ȱ��ȭ �Ǿ��ִ� ������Ʈ ã��
      3. 
      */

    private GameObject parentbuildingObj;
    ShelterCreate shelterCreate;
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
            Debug.Log("??????????????????");
            base.Update();
        }
        Debug.Log(shelterCreate.Building.transform.position);

        //if(buildingCreate.isValidBuild) buildImg.
    }

    // ��ư�� ������ ��� UIǥ��
    public void ShelterBuildAvailable() {
        Debug.Log("?����� �����µ�?");
        buildingObj = shelterCreate.Building;
        isBuiltStart = true;
        buildImg.gameObject.SetActive(true);
    }


}
