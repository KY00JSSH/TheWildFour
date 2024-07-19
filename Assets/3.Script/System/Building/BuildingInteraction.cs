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

    private InteractionUIMapping InteractionUI;

    private ItemSelectControll selected;
    private Menu_Controll menuControl;

    private void Awake() {
        InteractionUI = FindObjectOfType<InteractionUIMapping>();    
        menuControl = FindObjectOfType<Menu_Controll>();
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
        menuControl.gameObject.SetActive(false);
        switch (type) {
            case BuildingType.Campfire: CampfireInteraction(); break;
            case BuildingType.Furnace: FurnaceInteraction(); break;
            case BuildingType.Shelter: ShelterInteraction(); break;
            case BuildingType.Workshop: WorkshopInteraction(); break;
            case BuildingType.Chest: ChestInteraction(); break;
        }
    }

    private void CampfireInteraction() { 
        
    }

    private void FurnaceInteraction() {
        
    }

    private void ShelterInteraction() {
        InteractionUI.ShelterUI.SetActive(true);
    }

    private void WorkshopInteraction() {
        InteractionUI.WorkShopUI.SetActive(true);
    }

    private void ChestInteraction() {
    
    }
}
