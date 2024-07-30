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
    protected Tooltip_Build tooltip_Build;

    // 따라다닐 오브젝트
    public GameObject BuildImg;
    protected Image[] buildImgs;
    // 24 07 28 건설 dust 이펙트 추가 
    [SerializeField] protected ParticleSystem dustPrefab;

    public float positions = 2;
    //public float[] sizes = new float[2];

    // 설치될 오브젝트
    public GameObject buildingObj;
    public bool isBuiltStart = false;

    private Animator playerAnimator;
    // Build 애니매이션이 실행중인지 알아야함
    protected Animator buildAnimator;
    protected string buildAnimationName;
    protected bool isBuildAniComplete;
    [SerializeField] protected float buildAnimationPlayTime; // 사용자가 플레이할 시간을 지정
    protected float playTime;

    protected virtual void Awake() {
        buildingCreate = FindObjectOfType<BuildingCreate>();
        tooltip_Build = FindObjectOfType<Tooltip_Build>();
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        isBuildAniComplete = false;
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

                    // dustPrefab 위치 설정
                    BuildDustPrefabPosition();
                }
            }
            else {
                buildImgs[0].sprite = BuildAvailable[2];
            }
            BuildPrefabUIPosition();
            BuildPrefabUISize();
            BuildPrefabUIPosition_Vertical();
        }
        //TODO: 애니메이션의 타이밍을 알 수 없음 -> 애니메이션 시작 끝 알아야할것같음!
        if (isValid) if (!BuildImg.activeSelf) 
                if(!isBuildAniComplete) BuildDustPrefabEffectStart();
        
    }

    protected virtual void OnDisable() {
        if (BuildImg != null) {
            BuildImg.SetActive(false);
            isBuiltStart = false;
        }
    }

    // DustPrefab 위치 조정
    private void BuildDustPrefabPosition() {
        // Build renderer
        Renderer buildPrefabRe = BuildPrefabRenderer();
        // 위치 조정
        dustPrefab.transform.position = buildPrefabRe.bounds.center;

        // size 조정 필요함
        Vector3 size = buildPrefabRe.bounds.size;
        float width = size.x;
        float height = size.z;

        // 파티클 -> 도넛 형태
        float diameter = Mathf.Max(width, height);
        float radius = diameter * 0.35f;

        // 파티클의 형태 중 radius 
        var shape = dustPrefab.shape;
        shape.radius = radius; 
    }

    private void BuildDustPrefabEffectStart() {
        if (IsInCreateState()) {
            StartCoroutine(BuildDustPrefabEffectPlayTime());
        }
    }

    // 건축물 애니메이션 시작 확인
    private bool IsInCreateState() {
        AnimatorStateInfo stateInfo = buildAnimator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(buildAnimationName); 
    }
    // 건축물 애니메이션 끝나는 시점 없음 -> 임의 초 지정하여 이펙트 실행
    private IEnumerator BuildDustPrefabEffectPlayTime() {
        while (playTime <= buildAnimationPlayTime) {
            dustPrefab.gameObject.SetActive(true);
            dustPrefab.Play();
            playTime += Time.deltaTime;
            yield return null;
        }
        BuildDustPrefabEffectOff();
    }
    private void BuildDustPrefabEffectOff() {
        isBuildAniComplete = true;
        dustPrefab.gameObject.SetActive(false);
        // 먼지 이펙트 종료
        dustPrefab.Stop();
    }


    // Slider UI 위치 조정
    private void BuildPrefabUIPosition() {
        BuildImg.transform.position = buildingObj.transform.position;
    }

    // 설치할 건물의 바닥면 사이즈
    private Renderer BuildPrefabRenderer() {
        // 바닥면의 사이즈 확인
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

        if (buildPrefabRe == null) Debug.LogWarning("Rendere 없음");
        return buildPrefabRe;
    }

    // 원 크기 조정
    private void BuildPrefabUISize() {
        RectTransform buildImgRe = buildImgs[1].GetComponent<RectTransform>();
        Renderer buildPrefabRe = BuildPrefabRenderer();

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

    public virtual void BuildAvailableMode() {
        if (!tooltip_Build.isBuildAvailable || buildingCreate.isExist ||
            playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Create")) return;

        buildingObj = buildingCreate.Building;
        StartCoroutine(FindObject());
        isBuildAniComplete = false;
        playTime = 0;
    }

    protected IEnumerator FindObject() {
        while (buildingObj == null) {
            buildingObj = buildingCreate.Building;
            yield return null;
        }
        isBuiltStart = true;
        BuildImg.SetActive(true);
    }

}
