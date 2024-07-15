using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildPrefabUI : MonoBehaviour {
    /*
     �Ǽ��� �ش��ϴ� ��� ��ũ��Ʈ�� �����ϴ� ����
    => build ��ư Ŭ���� �ش��ϴ� ����
    1. ���� null ��������Ʈ ����
    2. bool �� �־��ָ� ��������Ʈ ����
     
    === 
    1. �� prefabs UI���� ������Ʈ�� ã��
    2. ������Ʈ�� ã������ ���� 
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
            //Debug.Log(" ��ġ ���� �Ұ��� Ȯ�� " + buildingCreate.isValidBuild);
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

    // UI ��ġ ����
    private void BuildPrefabUIPosition() {
        RectTransform buildImgRe = buildImg.GetComponent<RectTransform>();
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(buildingObj.transform.position);

        // ȭ����� �ָ��ִ� ��ü�� ������ �ִ� ��ü�� �����̴��� y��ǥ ������
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
