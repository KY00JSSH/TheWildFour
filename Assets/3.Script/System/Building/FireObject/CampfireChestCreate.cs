using UnityEngine;

public class CampfireChestCreate : BuildingCreate {
    [SerializeField] private Material BuildMaterial;
    [SerializeField] private Material ExistMaterial;

    private GameObject newCampfireChest;

    public override GameObject Building {
        get { return newCampfireChest; }
    }

    public override void BuildMode() {

        if (!tooltip_Build.isBuildAvailable) return;

        if (Building == null) {
            newCampfireChest = Instantiate(buildingPrefabs[0], transform);
        }

        if (name == "Campfire(Clone)")
            Building.GetComponent<Campfire>().enabled = false;
        foreach (Renderer each in Building.GetComponentsInChildren<MeshRenderer>())
            each.material = BuildMaterial;
        buildingColliders = Building.GetComponentsInChildren<Collider>();

        base.BuildMode();
    }

    public override void CreateBuilding() {
        if (name == "Campfire(Clone)")
            Building.GetComponent<Campfire>().enabled = true;
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
