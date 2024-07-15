using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildPrefabUI : MonoBehaviour {
    /*
     건설에 해당하는 모든 스크립트에 들어가야하는 내용
    => build 버튼 클릭시 해당하는 내용
    1. 기존 null 스프라이트 저장
    2. bool 값 넣어주면 스프라이트 변경
    === 
    1. 각 prefabs UI에서 오브젝트를 찾음
    2. 오브젝트 찾아서 위치 따라감
     */
    [Header("Build Prefab UI")]
    public Sprite[] BuildAvailable;
    protected BuildingCreate buildingCreate;

    public GameObject BuildImg;
    protected Image[] buildImgs;

    public float[] positions = new float[2];
    public float[] sizes = new float[2];

    protected GameObject buildingObj;
    protected bool isBuiltStart = false;
    protected virtual void Awake() {
        buildingCreate = FindObjectOfType<BuildingCreate>();
        buildImgs = new Image[2];
        for (int i = 0; i < BuildImg.transform.childCount; i++) {
            buildImgs[i] = BuildImg.transform.GetChild(i).GetComponent<Image>();
        }
    }

    protected bool isValid;

    protected virtual void Update() {
        if (isBuiltStart) {
            Debug.Log(" 설치 가능 불가능" + isValid);

            if (isValid) {
                buildImgs[0].sprite = BuildAvailable[1];
                if (Input.GetMouseButtonDown(0)) {
                    isBuiltStart = false;
                    BuildImg.SetActive(false);
                }
            }
            else {
                buildImgs[0].sprite = BuildAvailable[2];
            }

            BuildPrefabUIPosition();
            //BuildPrefabUIFixedSize();
            BuildPrefabUISize();
        }
    }
    // UI 위치 정렬
    private void BuildPrefabUIPosition() {
        for (int i = 0; i < buildImgs.Length; i++) {
            RectTransform buildImgRe = buildImgs[i].GetComponent<RectTransform>();
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(buildingObj.transform.position);

            // 화면상의 멀리있는 물체와 가까이 있는 물체의 슬라이더의 y좌표 보정값
            float depthFactor = Mathf.Clamp(15f / screenPosition.z, 0.7f, 5f);
            screenPosition.y += positions[i] * depthFactor;
            buildImgRe.position = screenPosition;
        }
    }

    private void BuildPrefabUISize() {
        for (int i = 0; i < buildImgs.Length; i++) {
            RectTransform buildImgRe = buildImgs[i].GetComponent<RectTransform>();
            Renderer buildImgRenderer = buildingObj.transform.GetChild(0).GetComponent<Renderer>();
            if (buildImgRenderer != null) {
                Bounds bounds = buildImgRenderer.bounds;
                Vector3 screenMin = Camera.main.WorldToScreenPoint(bounds.min);
                Vector3 screenMax = Camera.main.WorldToScreenPoint(bounds.max);
                float width = Mathf.Abs(screenMax.x - screenMin.x);
                float height = Mathf.Abs(screenMax.y - screenMin.y);

                buildImgRe.sizeDelta = new Vector2(width / sizes[i], height / sizes[i]);
            }
        }
    }

    private void BuildPrefabUIFixedSize() {
        for (int i = 0; i < buildImgs.Length; i++) {
            RectTransform buildImgRe = buildImgs[i].GetComponent<RectTransform>();
            buildImgRe.sizeDelta = new Vector2(buildImgRe.sizeDelta.x * sizes[i], buildImgRe.sizeDelta.y * sizes[i]);
        }
    }



}
