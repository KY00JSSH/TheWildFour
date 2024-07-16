using UnityEngine;

public class BuildingValidity : MonoBehaviour {
    private BuildingCreate buildingCreate;

    private void Awake() {
        buildingCreate = GetComponentInParent<BuildingCreate>();
    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.layer != 7)
            buildingCreate.isValidBuild = false;
    }

    private void OnTriggerExit(Collider other) {
        buildingCreate.isValidBuild = true;
    }
}
