using UnityEngine;

public class BuildingValidity : MonoBehaviour {
    private BuildingCreate buildingCreate;

    private void Awake() {
        buildingCreate = GetComponentInParent<BuildingCreate>();
    }
    private void OnTriggerStay(Collider other) {
        buildingCreate.isValidBuild = false;
    }

    private void OnTriggerExit(Collider other) {
        buildingCreate.isValidBuild = true;
    }
}
