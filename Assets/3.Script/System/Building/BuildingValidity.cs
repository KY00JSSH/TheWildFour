using UnityEngine;

public class BuildingValidity : MonoBehaviour {
    private BuildingCreate buildingCreate;
    private float validAngle = 10f;
    private bool isTriggered = false;

    private void Awake() {
        buildingCreate = GetComponentInParent<BuildingCreate>();
    }

    private void Update() {
        //Debug.Log("Valid : " + buildingCreate.isValidBuild);
        if(buildingCreate.isBuild) {
            float xRotation = buildingCreate.Building.transform.rotation.eulerAngles.x;
            float zRotation = buildingCreate.Building.transform.rotation.eulerAngles.z;

            if (Mathf.Abs(xRotation) > validAngle ||
                Mathf.Abs(zRotation) > validAngle) {
                Quaternion clampedRotation = Quaternion.Euler(
                    Mathf.Clamp(xRotation, xRotation, xRotation > 0 ? validAngle : -validAngle),
                    buildingCreate.Building.transform.rotation.eulerAngles.y,
                    Mathf.Clamp(zRotation, zRotation, zRotation > 0 ? validAngle : -validAngle)
                    );
                buildingCreate.Building.transform.rotation = clampedRotation;
                buildingCreate.isValidBuild = false;
            }
            else if (!isTriggered) buildingCreate.isValidBuild = true;
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.layer != LayerMask.NameToLayer("Ground")) {
            buildingCreate.isValidBuild = false;
            isTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        buildingCreate.isValidBuild = true;
        isTriggered = false;
    }
}
