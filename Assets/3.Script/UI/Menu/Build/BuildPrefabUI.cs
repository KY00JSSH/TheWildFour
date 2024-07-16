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
    [Header("Build Prefab UI")]
    public Sprite[] BuildAvailable;
    protected BuildingCreate buildingCreate;
    // 24 07 16 ����� �Ǽ� ��ġ ���� ���� bool �߰� - ������ ���� Ȯ��
    protected Build_Tooltip build_Tooltip;

    // ����ٴ� ������Ʈ
    public GameObject BuildImg;
    protected Image[] buildImgs;

    public float positions = 2;
    //public float[] sizes = new float[2];

    // ��ġ�� ������Ʈ
    protected GameObject buildingObj;
    protected bool isBuiltStart = false;

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

    // UI ��ġ ����
    private void BuildPrefabUIPosition() {
        
        BuildImg.transform.position = buildingObj.transform.position;
    }

    // �� ũ�� ����
    private void BuildPrefabUISize() {
        RectTransform buildImgRe = buildImgs[1].GetComponent<RectTransform>();

        // �ٴڸ��� ������ Ȯ��
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

        if (buildPrefabRe == null) Debug.LogWarning("Rendere ����");

        Vector3 size = buildPrefabRe.bounds.size;
        float width = size.x;
        float height = size.z;

        buildImgRe.sizeDelta = new Vector2(width, height);
    }

    // check �̹��� ��ġ ����
    private void BuildPrefabUIPosition_Vertical() {
        RectTransform buildImgRe = buildImgs[0].GetComponent<RectTransform>();
        Renderer buildPrefabRe = buildingObj.transform.GetChild(0).GetComponent<Renderer>();
        if (buildPrefabRe == null) {
            Transform buildPrefabChild = buildingObj.transform.GetChild(0);
            foreach (Transform child in buildPrefabChild) {
                if (child.TryGetComponent(out Renderer childRe)) {
                    buildPrefabRe = childRe;
                    break;
                }
            }
        }

        if (buildPrefabRe == null) {
            Debug.LogWarning("Renderer ����");
        }
        else {
            Vector3 size = buildPrefabRe.bounds.size;
            Vector3 center = buildPrefabRe.bounds.center;
            Vector3 topCenter = new Vector3(center.x, center.y + size.y / 2, center.z);

            buildImgRe.position = topCenter + new Vector3(0, 0.2f, 0); 
        }
    }

}
