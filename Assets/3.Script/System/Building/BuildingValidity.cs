using UnityEngine;

public class BuildingValidity : MonoBehaviour {
    private BuildingCreate buildingCreate;
    private float validAngle = 10f;
    private bool isTriggered;

    private void Awake() {
        buildingCreate = GetComponentInParent<BuildingCreate>();
    }

    private void Update() {
        Debug.Log(buildingCreate.isBuild);
        if (buildingCreate.isBuild) {
            float xRotation = buildingCreate.Building.transform.rotation.eulerAngles.x;
            float zRotation = buildingCreate.Building.transform.rotation.eulerAngles.z;

            if (xRotation > 180) xRotation -= 360;
            if (zRotation > 180) zRotation -= 360;
            if (Mathf.Abs(xRotation) > validAngle || Mathf.Abs(zRotation) > validAngle) {
                Quaternion clampedRotation = Quaternion.Euler(
                        Mathf.Clamp(xRotation, -validAngle, validAngle),
                        buildingCreate.Building.transform.rotation.eulerAngles.y,
                        Mathf.Clamp(zRotation, -validAngle, validAngle));

                buildingCreate.Building.transform.rotation = clampedRotation;
                buildingCreate.isValidBuild = false;
            }
            else if (isTriggered) buildingCreate.isValidBuild = false;
            else buildingCreate.isValidBuild = true;
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.layer != LayerMask.NameToLayer("Ground")) {
            Debug.Log(other.name);
            buildingCreate.isValidBuild = false;
            isTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        buildingCreate.isValidBuild = true;
        isTriggered = false;
    }
}
