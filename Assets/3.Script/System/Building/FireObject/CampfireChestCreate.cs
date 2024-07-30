using UnityEngine;

public class CampfireChestCreate : BuildingCreate {
    [SerializeField] private Material BuildMaterial;
    [SerializeField] private Material ExistMaterial;

    private GameObject newCampfireChest;

    public override GameObject Building {
        get { return newCampfireChest; }
    }

    public override void BuildMode() {
        if (Building == null) {
            newCampfireChest = Instantiate(buildingPrefabs[0], transform);
            //TODO: !!!!!!!!240729 김수주 - 비활성화 넣어도 됩니까?
            newCampfireChest.SetActive(false);
        }

        if (TryGetComponent(out Campfire campfire)) campfire.enabled = false;
        if (TryGetComponent(out Furnace furnace)) furnace.enabled = false;
        foreach (Renderer each in Building.GetComponentsInChildren<MeshRenderer>())
            each.material = BuildMaterial;
        buildingColliders = Building.GetComponentsInChildren<Collider>();

        base.BuildMode();
    }

    public override void CreateBuilding() {
        if (TryGetComponent(out Campfire campfire)) campfire.enabled = true;
        if (TryGetComponent(out Furnace furnace)) furnace.enabled = true;
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
