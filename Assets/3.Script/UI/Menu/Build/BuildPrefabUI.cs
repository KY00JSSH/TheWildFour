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
    2. 오브젝트가 찾아지면 시작 
    3. 

     */

    public Sprite[] BuildAvailable;
    protected BuildingCreate buildingCreate;
    public Image buildImg;
    protected GameObject buildingObj;

    protected bool isBuiltStart = false;
    protected virtual void Awake() {
        buildingCreate = FindObjectOfType<BuildingCreate>();
    }
    protected virtual void OnEnable() {
    }
    protected virtual void Update() {
        if (isBuiltStart) {
            //Debug.Log(" 설치 가능 불가능 확인 " + buildingCreate.isValidBuild);
            BuildPrefabUIPosition();
            BuildPrefabUISize();
            if (buildingCreate.isValidBuild) {
                buildImg.sprite = BuildAvailable[1];
                if (Input.GetMouseButton(0)) {
                    isBuiltStart = false;
                    buildImg.sprite = BuildAvailable[0];
                }
            }
            else buildImg.sprite = BuildAvailable[2];
        }
    }

    // UI 위치 정렬
    private void BuildPrefabUIPosition() {
        RectTransform buildImgRe = buildImg.GetComponent<RectTransform>();
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(buildingObj.transform.position);

        // 화면상의 멀리있는 물체와 가까이 있는 물체의 슬라이더의 y좌표 보정값
        float depthFactor = Mathf.Clamp(15f / screenPosition.z, 0.7f, 5f);
        screenPosition.y += 50f * depthFactor;

        buildImgRe.position = screenPosition;
    }

    private void BuildPrefabUISize() {
        RectTransform buildImgRe = buildImg.GetComponent<RectTransform>();
        Renderer buildImgRenderer = buildingObj.transform.GetChild(0).GetComponent<Renderer>();
        if (buildImgRenderer != null) {
            Bounds bounds = buildImgRenderer.bounds;
            Vector3 screenMin = Camera.main.WorldToScreenPoint(bounds.min);
            Vector3 screenMax = Camera.main.WorldToScreenPoint(bounds.max);
            float width = Mathf.Abs(screenMax.x - screenMin.x);
            float height = Mathf.Abs(screenMax.y - screenMin.y);

            buildImgRe.sizeDelta = new Vector2(width, height / 5);
        }
    }

}
