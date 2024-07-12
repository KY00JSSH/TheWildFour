using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelterCreate : MonoBehaviour {
    [SerializeField] private GameObject[] shelterPrefabs;
    [SerializeField] private Material shelterMaterial; 
    private ShelterManager shelterManager;
    private Transform playerTransform;
    private Animator shelterAnimator;

    private bool isExist = false;
    private bool isBuild = false;

    //TODO: UI > 거처 메뉴 버튼 OnClicked => BuildShelter();
    //TODO: UI > 거처 내부 버튼 '짐싸기' OnClicked => DestroyShelter();

    private void Awake() {
        shelterAnimator = GetComponent<Animator>();
        shelterManager = GetComponent<ShelterManager>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update() {
        // TEST KEYCODE P for Debugging
        if (Input.GetKeyDown(KeyCode.P)) BuildShelter();

        if (isBuild) {
            Shelter().SetActive(true);
            FollowMouse();

            if(Input.GetKeyDown(KeyCode.Escape)) {
                CancelShelter();
            }
            else if(Input.GetMouseButton(0)) {
                CreateShelter();
            }
        }
    }

    public GameObject Shelter() {
        return shelterPrefabs[shelterManager.ShelterLevel];
    }

    public void CreateShelter() {
        isBuild = false;
        isExist = true;
        shelterAnimator.SetTrigger("Create");
        MaterialOpaque();
    }

    public void BuildShelter() {
        if (!isExist) {
            isBuild = true;
            
            MaterialTransparent();
            Color materialColor = shelterMaterial.color;
            materialColor.a = 0.3f;
            shelterMaterial.color = materialColor;
        }
    }

    public void CancelShelter() {
        isBuild = false;
        Shelter().SetActive(false);
        MaterialOpaque();
    }

    public void DestroyShelter() {
        isExist = false;
        Shelter().SetActive(false);
    }

    private void FollowMouse() {
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
            Quaternion targetRotation = Quaternion.LookRotation(targetPosition - playerTransform.position);

            Shelter().transform.position = targetPosition;
            Shelter().transform.rotation = Quaternion.Slerp(
            Shelter().transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime);
        }
    }

    public void MaterialTransparent() {
        shelterMaterial.SetFloat("_Surface", 1);
        shelterMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        shelterMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        shelterMaterial.SetInt("_ZWrite", 0);
        shelterMaterial.DisableKeyword("_ALPHATEST_ON");
        shelterMaterial.EnableKeyword("_ALPHABLEND_ON");
        shelterMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        shelterMaterial.SetShaderPassEnabled("ShadowCaster", false);
        shelterMaterial.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }
    public void MaterialOpaque() {
        shelterMaterial.SetFloat("_Surface", 0); // 0: Opaque, 1: Transparent
        shelterMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        shelterMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        shelterMaterial.SetInt("_ZWrite", 1);
        shelterMaterial.DisableKeyword("_ALPHATEST_ON");
        shelterMaterial.DisableKeyword("_ALPHABLEND_ON");
        shelterMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        shelterMaterial.SetShaderPassEnabled("ShadowCaster", true);
        shelterMaterial.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
    }
}
