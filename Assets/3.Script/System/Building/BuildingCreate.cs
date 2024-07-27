using UnityEngine;
using UnityEngine.EventSystems;

public interface IBuildingCreateGeneric {
    void SetEnterPosition(Vector3 position);
    void DestroyBuilding();
    GameObject Building { get; }
    Transform playerTransform { get; }
    Vector3 LastPlayerPosition { get; }

}

public class BuildingCreate : MonoBehaviour, IBuildingCreateGeneric {
    [SerializeField] protected GameObject[] buildingPrefabs;
    [SerializeField] private Material buildingMaterial;
    public Transform playerTransform { get; private set; }
    private Animator playerAnimator;
    public bool isFirst { get; private set; }

    private ItemSelectControll itemSelectControl;
    private InvenController invenCont;
    private TooltipNum tooltipNum;

    protected Collider[] buildingColliders;

    public bool isExist = false;
    public bool isBuild = false;

    public bool isValidBuild = true;

    protected Tooltip_Build tooltip_Build;

    private int layerMask;
    public Vector3 LastPlayerPosition { get; private set; }
    public void SetEnterPosition(Vector3 position) { LastPlayerPosition = position; }

    protected virtual void Awake() {
        tooltipNum = FindObjectOfType<TooltipNum>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerAnimator = playerTransform.GetComponent<Animator>();
        tooltip_Build = FindObjectOfType<Tooltip_Build>();
        invenCont = FindObjectOfType<InvenController>();
    }

    private void Start() {
        isValidBuild = true;
        isFirst = true;
        layerMask = 1 << LayerMask.NameToLayer("Ground");
    }

    private void Update() {
        if (isBuild) {
            if (!Building.activeSelf)
                Building.SetActive(true);
            FollowMouse();

            if (Input.GetKeyDown(KeyCode.Escape)) {
                CancelBuilding();
            }
            else if (Input.GetMouseButton(0)) {
                if(isValidBuild && !EventSystem.current.IsPointerOverGameObject())
                    CreateBuilding();
            }
        }
    }

    public virtual GameObject Building {
        get { return buildingPrefabs[0]; }
    }

    public virtual void BuildMode() {
        if (PlayerMove.isPlayerBuilding || !tooltip_Build.isBuildAvailable) return;

        if (!isExist) {
            foreach (Collider collider in buildingColliders) {
                collider.isTrigger = true;
                if (!collider.TryGetComponent(out BuildingValidity validity)) {
                    collider.gameObject.AddComponent<BuildingValidity>();
                    collider.gameObject.AddComponent<Rigidbody>().isKinematic = true;
                }
            }
            isBuild = true;

            itemSelectControl = Building.GetComponentInChildren<ItemSelectControll>();
            itemSelectControl.GetComponent<Collider>().enabled = false;
            itemSelectControl.enabled = false;

            MaterialTransparent();
            Color materialColor = buildingMaterial.color;
            materialColor.a = 0.3f;
            buildingMaterial.color = materialColor;
        }
    }

    public virtual void CreateBuilding() {
        foreach (Collider collider in buildingColliders)
            collider.isTrigger = false;

        buildItemUse();

        isBuild = false;
        isExist = true;
        isFirst = false;

        itemSelectControl = Building.GetComponentInChildren<ItemSelectControll>();
        itemSelectControl.GetComponent<Collider>().enabled = true;
        itemSelectControl.enabled = true;

        MaterialOpaque();
        playerAnimator.SetTrigger("triggerCreate");

        PanelMap_MarkerSpawner panelMap_MarkerSpawner = FindObjectOfType<PanelMap_MarkerSpawner>();
        panelMap_MarkerSpawner.SetMarker(Building.GetComponent<BuildingInteraction>().Type, Building.transform.position);

        MenuMap_MarkerSpawner menuMap_markerSpawner = FindObjectOfType<MenuMapZoom>().menuMap.transform.GetComponent<MenuMap_MarkerSpawner>();
        menuMap_markerSpawner.SetMarker(Building.GetComponent<BuildingInteraction>().Type, Building.transform.position);
    }

    private void buildItemUse() {
        switch (Building.GetComponent<BuildingInteraction>().Type) {
            case BuildingType.Campfire:
                invenCont.buildingCreateUseItem(tooltipNum.BuildItemCheck(0, 0).needItems);
                break;
            case BuildingType.Furnace:
                invenCont.buildingCreateUseItem(tooltipNum.BuildItemCheck(1, 0).needItems);
                break;
            case BuildingType.Shelter:
                if (isFirst) {
                    invenCont.buildingCreateUseItem(tooltipNum.BuildItemCheck(2, 0).needItems);
                }
                break;
            case BuildingType.Workshop:
                if (isFirst) {
                    invenCont.buildingCreateUseItem(tooltipNum.BuildItemCheck(3, 0).needItems);
                }
                break;
            case BuildingType.Chest:
                invenCont.buildingCreateUseItem(tooltipNum.BuildItemCheck(4, 0).needItems);
                break;
        }
    }

    //public void DestroyBuilding() {
    //    isExist = false;
    //    Building.SetActive(false);
    //
    //    PanelMap_MarkerSpawner panelMap_MarkerSpawner = FindObjectOfType<PanelMap_MarkerSpawner>();
    //    panelMap_MarkerSpawner.RemoveMarker(Building.GetComponent<BuildingInteraction>().Type, Building.transform.position);
    //
    //    MenuMap_MarkerSpawner menuMap_markerSpawner = FindObjectOfType<MenuMapZoom>().menuMap.transform.GetComponent<MenuMap_MarkerSpawner>();
    //    menuMap_markerSpawner.RemoveMarker(Building.GetComponent<BuildingInteraction>().Type, Building.transform.position);
    //}

    public void DestroyBuilding()
    {
        if(isExist)
        { 
            isExist = false;            

            PanelMap_MarkerSpawner panelMap_MarkerSpawner = FindObjectOfType<PanelMap_MarkerSpawner>();
            panelMap_MarkerSpawner.RemoveMarker(Building.GetComponent<BuildingInteraction>().Type/*, Building.transform.position*/);

            MenuMap_MarkerSpawner menuMap_markerSpawner = FindObjectOfType<MenuMapZoom>().menuMap.transform.GetComponent<MenuMap_MarkerSpawner>();
            menuMap_markerSpawner.RemoveMarker(Building.GetComponent<BuildingInteraction>().Type);

            Building.SetActive(false);
        }
        if(!PlayerStatus.isDead) {
            playerTransform.position = LastPlayerPosition;
            playerTransform.gameObject.SetActive(true);
            CameraControl cameraControl = FindObjectOfType<CameraControl>();
            cameraControl.cinemachineFreeLook.Follow = playerTransform;
            cameraControl.cinemachineFreeLook.LookAt = playerTransform;
        }
    }


    public void CancelBuilding() {
        isBuild = false;
        if (Building != null)
            Building.SetActive(false);
        MaterialOpaque();
    }

    protected void FollowMouse() {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        float rotationSpeed = 5f;
        float buildAreaRadius = 3f;

        if (Physics.Raycast(cameraRay, out RaycastHit raycastHit, Mathf.Infinity, layerMask)) {
            Vector3 pointTolook = raycastHit.point;

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
            Ray downRay = new Ray(new Vector3(
                targetPosition.x, playerTransform.position.y + buildAreaRadius, targetPosition.z), Vector3.down);
            if (Physics.Raycast(downRay, out RaycastHit downRaycastHit, Mathf.Infinity, layerMask)) {
                targetPosition.y = downRaycastHit.point.y;
            }

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

    public void CancelAllBuilings() {
        BuildingCreate[] buildingCreates = FindObjectsOfType<BuildingCreate>();
        foreach (var each in buildingCreates)
            each.CancelBuilding();
    }
}
