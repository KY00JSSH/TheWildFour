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
    // 24 07 16 김수주 건설 설치 가능 여부 bool 추가 - 아이템 개수 확인
    protected Build_Tooltip build_Tooltip;

    // 따라다닐 오브젝트
    public GameObject BuildImg;
    protected Image[] buildImgs;

    public float positions = 2;
    //public float[] sizes = new float[2];

    // 설치될 오브젝트
    protected GameObject buildingObj;
    public bool isBuiltStart = false;

    protected virtual void Awake() {
        buildingCreate = FindObjectOfType<BuildingCreate>();
        build_Tooltip = FindObjectOfType<Build_Tooltip>();
        buildImgs = new Image[2];
        for (int i = 0; i < BuildImg.transform.childCount; i++) {
            buildImgs[i] = BuildImg.transform.GetChild(i).GetComponent<Image>();
        }
    }

    protected bool isValid;

    protected virtual void Update() {
        if (isBuiltStart) {
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
            BuildPrefabUISize();
            BuildPrefabUIPosition_Vertical();
        }
    }

    protected virtual void OnDisable() {
        if (BuildImg != null) {
            BuildImg.SetActive(false);
            isBuiltStart = false;
        }
    }

    // UI 위치 조정
    private void BuildPrefabUIPosition() {
        
        BuildImg.transform.position = buildingObj.transform.position;
    }

    // 원 크기 조정
    private void BuildPrefabUISize() {
        RectTransform buildImgRe = buildImgs[1].GetComponent<RectTransform>();

        // 바닥면의 사이즈 확인
        Renderer buildPrefabRe = buildingObj.transform.GetChild(0).GetComponent<Renderer>();
        if(buildPrefabRe == null) {
            Transform buildPrefabChild = buildingObj.transform.GetChild(0);
            foreach(Transform child in buildPrefabChild) {
                if(child.TryGetComponent(out Renderer childRe)) {
                    buildPrefabRe = childRe;
                    break;
                }
            }
        }

        if (buildPrefabRe == null) Debug.LogWarning("Rendere 없음");

        Vector3 size = buildPrefabRe.bounds.size;
        float width = size.x;
        float height = size.z;

        buildImgRe.sizeDelta = new Vector2(width, height);
    }

    // check 이미지 위치 조정
    private void BuildPrefabUIPosition_Vertical() {
        RectTransform buildImgRe = buildImgs[0].GetComponent<RectTransform>();
        Renderer buildPrefabRe = buildingObj.transform.GetChild(0).GetComponent<Renderer>();
        if (buildingObj.transform.GetChild(0).GetComponent<ParticleSystem>() != null) {
            Transform buildPrefabChild = buildingObj.transform.GetChild(5);
            if (buildPrefabChild.TryGetComponent(out Renderer childRe)) {
                buildPrefabRe = childRe;
            }
        }
        else {
            if (buildPrefabRe == null) {
                Transform buildPrefabChild = buildingObj.transform.GetChild(0);
                foreach (Transform child in buildPrefabChild) {
                    if (child.TryGetComponent(out Renderer childRe)) {
                        buildPrefabRe = childRe;
                        break;
                    }
                }
            }
        }


        if (buildPrefabRe == null) {
            Debug.LogWarning("Renderer 없음");
        }
        else {
            Vector3 size = buildPrefabRe.bounds.size;
            Vector3 center = buildPrefabRe.bounds.center;
            Vector3 topCenter = new Vector3(center.x, center.y + size.y / 2, center.z);

            buildImgRe.position = topCenter + new Vector3(0, 0.2f, 0); 
        }
    }

}
