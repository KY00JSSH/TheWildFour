using System.Runtime.InteropServices;
using UnityEngine;

public enum BuildingType {
    Campfire,
    Furnace,
    Shelter,
    Workshop,
    Chest
}

public class BuildingInteraction : MonoBehaviour {
    [SerializeField] private BuildingType buildingType;
    public BuildingType Type { get { return buildingType; } }

    private BuildingInteractionManager interactionManager;
    private InteractionUIMapping InteractionUI;
    private Menu_Controll menuControl;
    private ItemSelectControll selected;

    private GameObject player;
    private CameraControl cameraControl;

    private void Awake() {
        interactionManager = FindObjectOfType<BuildingInteractionManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        cameraControl = FindObjectOfType<CameraControl>();

        InteractionUI = interactionManager.InteractionUI;
        menuControl = interactionManager.menuControl;
    }

    private void OnEnable() {
        selected = GetComponentInChildren<ItemSelectControll>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (PlayerItemPickControll.ClosestItem == selected.gameObject)
                Interaction(buildingType);
        }
    }

    private void Interaction(BuildingType type) {
        //Debug.Log(type);

        switch (type) {
            case BuildingType.Campfire: CampfireInteraction(); break;
            case BuildingType.Furnace: FurnaceInteraction(); break;
            case BuildingType.Shelter: ShelterInteraction(); break;
            case BuildingType.Workshop: WorkshopInteraction(); break;
            case BuildingType.Chest: ChestInteraction(); break;
        }
    }

    private void CampfireInteraction() {
        if (TryGetComponent(out Campfire campfire)) {
            campfire.AddWood();
        }
    }

    private void FurnaceInteraction() {
        if (TryGetComponent(out Furnace furnace)) {
            furnace.AddWood();
        }
    }

    private void ShelterInteraction() {
        CloseAllUI(); InteractionUI.ShelterUI.SetActive(true);
        PlayerEnterBuilding<ShelterCreate>();
    }

    private void WorkshopInteraction() {
        CloseAllUI(); InteractionUI.WorkShopUI.SetActive(true);
        PlayerEnterBuilding<WorkshopCreate>();
    }

    private void ChestInteraction() {
        //menuControl.gameObject.SetActive(false);
    }

    public void CloseAllUI() {
        menuControl?.gameObject.SetActive(false);
        InteractionUI?.ShelterUI.SetActive(false);
        InteractionUI?.WorkShopUI.SetActive(false);

    }

    public void PlayerEnterBuilding<T>() where T : MonoBehaviour, IBuildingCreateGeneric{
        T buildingCreate = FindObjectOfType<T>();
        buildingCreate.SetEnterPosition(player.transform.position);
        player.SetActive(false);
        Debug.Log("@@@" + player);
        cameraControl.cinemachineFreeLook.Follow = buildingCreate.Building.transform;
        cameraControl.cinemachineFreeLook.LookAt = buildingCreate.Building.transform;
    }

    public void PlayerExitBuilding<T>() where T : MonoBehaviour, IBuildingCreateGeneric {
        T buildingCreate = FindObjectOfType<T>();
        if (player) {
            player.transform.position = buildingCreate.LastPlayerPosition;
            player.SetActive(true);
            cameraControl.cinemachineFreeLook.Follow = buildingCreate.playerTransform;
            cameraControl.cinemachineFreeLook.LookAt = buildingCreate.playerTransform;
        }
    }
}
