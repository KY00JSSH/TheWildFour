using UnityEngine;

public class ShelterCreate : BuildingCreate {
    private ShelterManager shelterManager;
    private Animator shelterAnimator;

    protected override void Awake() {
        base.Awake();
        shelterAnimator = GetComponent<Animator>();
        shelterManager = GetComponent<ShelterManager>();
    }

    private void OnEnable() {
        Awake();
    }

    private void Start() {
        if(Save.Instance.saveData.shelterPosition != Vector3.zero) {
            Building.gameObject.SetActive(true);
            Building.transform.position = Save.Instance.saveData.shelterPosition;
            Building.transform.rotation = Save.Instance.saveData.shelterRotation;
            isExist = true; 
        }    
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
