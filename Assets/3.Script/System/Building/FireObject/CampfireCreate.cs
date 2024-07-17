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

        Building.GetComponent<Campfire>().enabled = false;
        foreach (Renderer each in Building.GetComponentsInChildren<MeshRenderer>())
            each.material = BuildMaterial;
        buildingColliders = Building.GetComponentsInChildren<Collider>();

        base.BuildMode();
    }

    public override void CreateBuilding() {
        Building.GetComponent<Campfire>().enabled = true;
        foreach (Renderer each in Building.GetComponentsInChildren<MeshRenderer>())
            each.material = ExistMaterial;
        buildingColliders = Building.GetComponentsInChildren<Collider>();

        base.CreateBuilding();

        isExist = false;
        Building.GetComponent<Animator>().SetTrigger("Create");
        newCampfire = null;
    }
    //TODO: ķ�����̾� ���� �� ��ƼŬ ������� �ȉ�
    //TODO: ķ�����̾� ��ġ UI ���� �ʿ�
}
