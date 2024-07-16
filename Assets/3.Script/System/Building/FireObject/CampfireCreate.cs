using UnityEngine;
using System.Collections.Generic;

public class CampfireCreate : BuildingCreate {
    [SerializeField] private Material BuildMaterial;
    [SerializeField] private Material ExistMaterial;

    private GameObject newCampfire;

    public override GameObject Building {
        get { return newCampfire; }
    }

    public override void BuildMode() {
        if (Building == null) {
            newCampfire = Instantiate(buildingPrefabs[0], transform);
        }

        Building.GetComponentInChildren<Renderer>().material = BuildMaterial;
        buildingColliders = Building.GetComponentsInChildren<Collider>();
        base.BuildMode();
    }

    public override void CreateBuilding() {
        Building.GetComponentInChildren<Renderer>().material = ExistMaterial;
        buildingColliders = Building.GetComponentsInChildren<Collider>();
        base.CreateBuilding();
        isExist = false;
        Building.GetComponent<Animator>().SetTrigger("Create");
        newCampfire = null;
    }
    //TODO: 캠프파이어 생성 후 파티클 정상출력 안됌
    //TODO: 캠프파이어 설치 UI 연결 필요
}
