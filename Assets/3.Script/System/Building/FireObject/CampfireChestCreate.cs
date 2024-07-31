using UnityEngine;

public class CampfireChestCreate : BuildingCreate {
    [SerializeField] private Material BuildMaterial;
    [SerializeField] private Material ExistMaterial;

    private GameObject newCampfireChest;

    public override GameObject Building {
        get { return newCampfireChest; }
    }

    private void Start() {
        if(buildingPrefabs[0].TryGetComponent(out Campfire campfire) &&
            Save.Instance.saveData.campfirePosition != null) { 
            foreach(var position in Save.Instance.saveData.campfirePosition) {
                Instantiate(buildingPrefabs[0], position, Quaternion.identity, transform);
            }
        }
        else if(buildingPrefabs[0].TryGetComponent(out Furnace furnace) &&
            Save.Instance.saveData.furnacePosition != null) {
            foreach (var position in Save.Instance.saveData.furnacePosition) {
                Instantiate(buildingPrefabs[0], position, Quaternion.identity, transform);
            }

        }
        else if (Save.Instance.saveData.chestPosition != null) {
            foreach (var position in Save.Instance.saveData.chestPosition) {
                Instantiate(buildingPrefabs[0], position, Quaternion.identity, transform);
            }
        }
    }

    public override void BuildMode() {
        if (Building == null) {
            newCampfireChest = Instantiate(buildingPrefabs[0], transform);
            newCampfireChest.SetActive(false);
        }

        foreach (Renderer each in Building.GetComponentsInChildren<MeshRenderer>())
            each.material = BuildMaterial;
        buildingColliders = Building.GetComponentsInChildren<Collider>();

        base.BuildMode();
    }

    public override void CreateBuilding() {

        foreach (Renderer each in Building.GetComponentsInChildren<MeshRenderer>())
            each.material = ExistMaterial;
        buildingColliders = Building.GetComponentsInChildren<Collider>();

        base.CreateBuilding();

        isExist = false;
        Building.GetComponent<Animator>().SetTrigger("Create");
        Destroy(Building.GetComponentInChildren<Rigidbody>());
        Destroy(Building.GetComponentInChildren<BuildingValidity>());
        newCampfireChest = null;
    }
}
