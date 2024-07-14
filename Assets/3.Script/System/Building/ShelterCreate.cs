using UnityEngine;

public class ShelterCreate : BuildingCreate {
    private ShelterManager shelterManager;
    private Animator shelterAnimator;

    //TODO: UI > ��ó �޴� ��ư OnClicked => BuildShelter();
    //TODO: UI > ��ó ���� ��ư '���α�' OnClicked => DestroyShelter();

    protected override void Awake() {
        base.Awake();
        shelterAnimator = GetComponent<Animator>();
        shelterManager = GetComponent<ShelterManager>();

    }

    public override GameObject Building {
        get { return buildingPrefabs[shelterManager.ShelterLevel]; }
    }

    public override void BuildMode() {
        buildingColliders = Building.GetComponentsInChildren<Collider>();
        base.BuildMode();
    }

    public override void CreateBuilding() {
        buildingColliders = Building.GetComponentsInChildren<Collider>();
        base.CreateBuilding();
        shelterAnimator.SetTrigger("Create");
    }
}