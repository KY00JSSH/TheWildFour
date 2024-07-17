using UnityEngine;

public class ShelterCreate : BuildingCreate {
    private ShelterManager shelterManager;
    private Animator shelterAnimator;

    protected override void Awake() {
        base.Awake();
        shelterAnimator = GetComponent<Animator>();
        shelterManager = GetComponent<ShelterManager>();

    }

    public override GameObject Building {
        get { return buildingPrefabs[shelterManager.ShelterLevel - 1]; }
    }

    public override void BuildMode() {

        if (!tooltip_Build.isBuildAvailable) return;

        buildingColliders = Building.GetComponentsInChildren<Collider>();
        base.BuildMode();
    }

    public override void CreateBuilding() {
        buildingColliders = Building.GetComponentsInChildren<Collider>();
        base.CreateBuilding();
        shelterAnimator.SetTrigger("Create");
    }
}
