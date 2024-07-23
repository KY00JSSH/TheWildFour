using UnityEngine;

public class BuildingValidity : MonoBehaviour {
    private BuildingCreate buildingCreate;
    private float validAngle = 10f;
    private bool isTriggered;

    private void Awake() {
        buildingCreate = GetComponentInParent<BuildingCreate>();
    }

    private void Update() {
        if (buildingCreate.isBuild) {
            /*
           if ((Mathf.Abs(xRotation) > validAngle && Mathf.Abs(xRotation) < validAngle + 3) ||
               (Mathf.Abs(zRotation) > validAngle && Mathf.Abs(zRotation) < validAngle + 3) ||
               (Mathf.Abs(xRotation) > 360 - validAngle && Mathf.Abs(xRotation) < 360 - validAngle + 3) ||
               (Mathf.Abs(zRotation) > 360 - validAngle && Mathf.Abs(zRotation) < 360 - validAngle + 3)) {
               Quaternion clampedRotation = Quaternion.Euler(
                   Mathf.Clamp(xRotation, xRotation, xRotation > 0 ? validAngle : -validAngle),
                   buildingCreate.Building.transform.rotation.eulerAngles.y,
                   Mathf.Clamp(zRotation, zRotation, zRotation > 0 ? validAngle : -validAngle)
                   );
               Debug.Log(clampedRotation.eulerAngles.x);
               // 플레이어에 부딪힐 때 각도 값 수정

               buildingCreate.Building.transform.rotation = clampedRotation;
               buildingCreate.isValidBuild = false;
           }

           else if (!isTriggered) buildingCreate.isValidBuild = true;
           */
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
            buildingCreate.isValidBuild = false;
            isTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        buildingCreate.isValidBuild = true;
        isTriggered = false;
    }
}
