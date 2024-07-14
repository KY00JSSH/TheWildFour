using UnityEngine;

public class ShelterCreate : BuildingCreate {
    private ShelterManager shelterManager;
    private Animator shelterAnimator;

    //TODO: UI > 거처 메뉴 버튼 OnClicked => BuildShelter();
    //TODO: UI > 거처 내부 버튼 '짐싸기' OnClicked => DestroyShelter();

    protected override void Awake() {
        base.Awake();
        shelterAnimator = GetComponent<Animator>();
        shelterManager = GetComponent<ShelterManager>();

    }

    public override GameObject Building {
        get { return buildingPrefabs[shelterManager.ShelterLevel - 1]; }
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
