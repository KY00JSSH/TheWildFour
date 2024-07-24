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
    private void Awake() {
        interactionManager = FindObjectOfType<BuildingInteractionManager>();

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
    }

    private void WorkshopInteraction() {
        CloseAllUI(); InteractionUI.WorkShopUI.SetActive(true);
    }

    private void ChestInteraction() {
        //menuControl.gameObject.SetActive(false);
    }

    public void CloseAllUI() {
        menuControl.gameObject.SetActive(false);
        InteractionUI.ShelterUI.SetActive(false);
        InteractionUI.WorkShopUI.SetActive(false);

    }
}
