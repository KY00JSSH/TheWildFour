using UnityEngine;

public class WorkshopCreate : BuildingCreate {
    private WorkshopManager workshopManager;
    private Animator workshopAnimator;
    
    protected override void Awake() {
        base.Awake();
        workshopAnimator = GetComponent<Animator>();
        workshopManager = GetComponent<WorkshopManager>();
    }

    private void Start() {
        if (Save.Instance.saveData.workshopPosition != Vector3.zero) {
            Building.gameObject.SetActive(true);
            Building.transform.position = Save.Instance.saveData.workshopPosition;
            Building.transform.rotation = Save.Instance.saveData.workshopRotation;
            isExist = true;
        }
    }

    public override GameObject Building {
        get { return buildingPrefabs[workshopManager.WorkshopLevel - 1]; }
    }

    public override void BuildMode() {
        buildingColliders = Building.GetComponentsInChildren<Collider>();
        base.BuildMode();
    }

    public override void CreateBuilding() {
        buildingColliders = Building.GetComponentsInChildren<Collider>();
        base.CreateBuilding();
        workshopAnimator.SetTrigger("Create");
    }
}
