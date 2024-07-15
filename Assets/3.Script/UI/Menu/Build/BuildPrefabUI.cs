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
    2. ������Ʈ ã�Ƽ� ��ġ ����
     */

    public Sprite[] BuildAvailable;
    protected BuildingCreate buildingCreate;

    public GameObject BuildImg;
    private Image[] buildImgs;

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

    protected virtual void Update() {
        if (isBuiltStart) {
            if (buildingCreate.isValidBuild) {

                buildImgs[0].sprite = BuildAvailable[1];
                if (Input.GetMouseButton(0)) {
                    isBuiltStart = false;
                    BuildImg.SetActive(false);
                }
            }
            else buildImgs[0].sprite = BuildAvailable[2];

            BuildPrefabUIPosition();
            //BuildPrefabUIFixedSize();
            BuildPrefabUISize();
        }

    }

    protected virtual void OnDisable() {
        isBuiltStart = false;
        if (BuildImg != null) {
            BuildImg.SetActive(false);
        }
    }

    // UI ��ġ ����
    private void BuildPrefabUIPosition() {
        for (int i = 0; i < buildImgs.Length; i++) {
            RectTransform buildImgRe = buildImgs[i].GetComponent<RectTransform>();
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(buildingObj.transform.position);

            // ȭ����� �ָ��ִ� ��ü�� ������ �ִ� ��ü�� �����̴��� y��ǥ ������
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
