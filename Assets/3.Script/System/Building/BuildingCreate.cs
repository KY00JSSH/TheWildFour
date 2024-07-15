using UnityEngine;

public class BuildingCreate : MonoBehaviour {
    [SerializeField] protected GameObject[] buildingPrefabs;
    [SerializeField] private Material buildingMaterial;
    private Transform playerTransform;
    protected Collider[] buildingColliders;

    public bool isExist = false;
    protected bool isBuild = false;

    public bool isValidBuild = true;

    protected virtual void Awake() {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start() {
        isValidBuild = true;
    }

    private void Update() {
        if (isBuild) {
            Building.SetActive(true);
            FollowMouse();

            if (Input.GetKeyDown(KeyCode.Escape)) {
                CancelBuilding();
            }
            else if (Input.GetMouseButton(0)) {
                if(isValidBuild)
                    CreateBuilding();
            }
        }
    }

    public virtual GameObject Building {
        get { return buildingPrefabs[0]; }
    }

    public virtual void BuildMode() {
        if (!isExist) {
            foreach (Collider collider in buildingColliders) {
                collider.isTrigger = true;
                if (!collider.TryGetComponent(out BuildingValidity validity)) {
                    collider.gameObject.AddComponent<BuildingValidity>();
                    collider.gameObject.AddComponent<Rigidbody>().isKinematic = true;
                }
            }
            isBuild = true;

            MaterialTransparent();
            Color materialColor = buildingMaterial.color;
            materialColor.a = 0.3f;
            buildingMaterial.color = materialColor;
        }
    }

    public virtual void CreateBuilding() {
        foreach (Collider collider in buildingColliders)
            collider.isTrigger = false;
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
                // 만약 거리가 반경보다 크다면 원의 경계에서 제한합니다
                Vector3 directionToCenter = (playerTransform.position - pointTolook).normalized;
                pointTolook = playerTransform.position - directionToCenter * buildAreaRadius;
            }
            Vector3 targetPosition = new Vector3(
                Mathf.Clamp(pointTolook.x, playerTransform.position.x - buildAreaRadius, playerTransform.position.x + buildAreaRadius),
                pointTolook.y,
                Mathf.Clamp(pointTolook.z + 0.01f, playerTransform.position.z - buildAreaRadius, playerTransform.position.z + buildAreaRadius)
            );
            Quaternion targetRotation =
                Quaternion.LookRotation(targetPosition - playerTransform.position) *
                Quaternion.Euler(180, 0, 180);

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
