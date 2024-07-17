using UnityEngine;

public class WorkshopCreate : BuildingCreate {
    private WorkshopManager workshopManager;
    private Animator workshopAnimator;

    protected override void Awake() {
        base.Awake();
        workshopAnimator = GetComponent<Animator>();
        workshopManager = GetComponent<WorkshopManager>();
    }

    public override GameObject Building {
        get { return buildingPrefabs[workshopManager.WorkshopLevel - 1]; }
    }

    public override void BuildMode() {

        if (!tooltip_Build.isStartBuildingNumCheck) return;
        buildingColliders = Building.GetComponentsInChildren<Collider>();
        base.BuildMode();
    }

    public override void CreateBuilding() {
        buildingColliders = Building.GetComponentsInChildren<Collider>();
        base.CreateBuilding();
        workshopAnimator.SetTrigger("Create");
    }
}
