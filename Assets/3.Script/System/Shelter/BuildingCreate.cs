using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCreate : MonoBehaviour {
    [SerializeField] protected GameObject[] buildingPrefabs;
    [SerializeField] private Material buildingMaterial;
    private Transform playerTransform;

    protected bool isExist = false;
    protected bool isBuild = false;

    protected virtual void Awake() {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update() {
        // TEST KEYCODE P for Debugging
        if (Input.GetKeyDown(KeyCode.P)) BuildMode();

        if (isBuild) {
            Building.SetActive(true);
            FollowMouse();

            if (Input.GetKeyDown(KeyCode.Escape)) {
                CancelBuilding();
            }
            else if (Input.GetMouseButton(0)) {
                CreateBuilding();
            }
        }
    }

    public virtual GameObject Building {
        get { return buildingPrefabs[0]; }
    }

    public void BuildMode() {
        if (!isExist) {
            isBuild = true;

            MaterialTransparent();
            Color materialColor = buildingMaterial.color;
            materialColor.a = 0.3f;
            buildingMaterial.color = materialColor;
        }
    }

    public virtual void CreateBuilding() {
        isBuild = false;
        isExist = true;
        MaterialOpaque();
    }

    public void DestroyBuilding() {
        isExist = false;
        Building.SetActive(false);
    }

    public void CancelBuilding() {
        isBuild = false;
        Building.SetActive(false);
        MaterialOpaque();
    }

    protected void FollowMouse() {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);

        float rotationSpeed = 5f;
        float buildAreaRadius = 3f;

        if (GroupPlane.Raycast(cameraRay, out float rayLength)) {
            Vector3 pointTolook = cameraRay.GetPoint(rayLength);
            float distanceToCenter = Vector3.Distance(pointTolook, playerTransform.position);
            if (distanceToCenter > buildAreaRadius) {
                // ���� �Ÿ��� �ݰ溸�� ũ�ٸ� ���� ��迡�� �����մϴ�
                Vector3 directionToCenter = (playerTransform.position - pointTolook).normalized;
                pointTolook = playerTransform.position - directionToCenter * buildAreaRadius;
            }
            Vector3 targetPosition = new Vector3(
                Mathf.Clamp(pointTolook.x, playerTransform.position.x - buildAreaRadius, playerTransform.position.x + buildAreaRadius),
                pointTolook.y,
                Mathf.Clamp(pointTolook.z + 0.01f, playerTransform.position.z - buildAreaRadius, playerTransform.position.z + buildAreaRadius)
            );
            Quaternion targetRotation = Quaternion.LookRotation(targetPosition - playerTransform.position);

            Building.transform.position = targetPosition;
            Building.transform.rotation = Quaternion.Slerp(
            Building.transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime);
        }
    }

    public void MaterialTransparent() {
        buildingMaterial.SetFloat("_Surface", 1);
        buildingMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        buildingMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        buildingMaterial.SetInt("_ZWrite", 0);
        buildingMaterial.DisableKeyword("_ALPHATEST_ON");
        buildingMaterial.EnableKeyword("_ALPHABLEND_ON");
        buildingMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        buildingMaterial.SetShaderPassEnabled("ShadowCaster", false);
        buildingMaterial.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }
    public void MaterialOpaque() {
        buildingMaterial.SetFloat("_Surface", 0); // 0: Opaque, 1: Transparent
        buildingMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        buildingMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        buildingMaterial.SetInt("_ZWrite", 1);
        buildingMaterial.DisableKeyword("_ALPHATEST_ON");
        buildingMaterial.DisableKeyword("_ALPHABLEND_ON");
        buildingMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        buildingMaterial.SetShaderPassEnabled("ShadowCaster", true);
        buildingMaterial.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
    }
}
