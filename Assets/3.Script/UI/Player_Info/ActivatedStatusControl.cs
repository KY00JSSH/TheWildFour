using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivatedStatusControl : MonoBehaviour {
    // 생성시킬 프리펩
    public GameObject CurrentActivateBoxPrf;
    // 생성 갯수
    [SerializeField] private int boxNum = 8;
    // 박스 리스트
    private List<GameObject> CurrentActivateBoxs;
    // 현재 활성화 표시할 스프라이트
    public Sprite[] ActivatedStatusSprites;

    // return 용 status 
    public Status AddActivatedStatus { get; private set; }

    // 생성 갯수만큼의 default 위치 정보값
    private Vector2[] defaultPositions;

    private void Awake() {
        defaultPositions = new Vector2[boxNum];
        /* 프리펩으로 해서 위치를 잃어버렸을 경우
        if(boxNum != 0) {
            CurrentActivateBoxPrf.transform.position = new Vector2(-880, -380);
        }
        */
        CurrentActivateBoxs = new List<GameObject>();
        BoxActivatedStatusInit();
        BoxsDefaultPositionSave();
    }


    private void Update() {
        if (StatusControl.Instance.ActivatedStatus != null) {
            // 상태 리스트가 null이 아닐 경우 활성화 오브젝트 개수 확인
            int activeBoxCnt = FindActiveBox();
            if (StatusControl.Instance.ActivatedStatus.Count > activeBoxCnt) {
                Debug.Log("StatusControl.Instance.ActivatedStatus.Count : " + StatusControl.Instance.ActivatedStatus.Count);
                BoxAddActivatedStatus(StatusControl.Instance.ActivatedStatus.Count - activeBoxCnt);
                Debug.Log("FindActiveBox : " + FindActiveBox());
            }
            // 비활성화 확인
           if(BoxDelActivatedStatus()) BoxPositionSetting();
        }
    }

    // 활성화 되어있는 박스 오브젝트 개수 확인
    private int FindActiveBox() {
        int count = 0;
        if (CurrentActivateBoxs.Count == 0) return count;

        for (int i = 0; i < CurrentActivateBoxs.Count; i++) {
            if (CurrentActivateBoxs[i].activeSelf) count++;
        }
        return count;
    }

    // 원하는 개수만큼 prefab 리스트 생성
    private void BoxActivatedStatusInit() {
        for (int i = 0; i < boxNum; i++) {
            // 프리펩 생성
            GameObject gameObject = Instantiate(CurrentActivateBoxPrf, transform);
            gameObject.name = CurrentActivateBoxPrf.name;
            gameObject.transform.position = CurrentActivateBoxPrf.transform.position;
            CurrentActivateBoxs.Add(gameObject);
            CurrentActivateBoxs[i].SetActive(false);
        }
    }

    // 위치 정보 저장
    private void BoxsDefaultPositionSave() {
        for (int i = 0; i < boxNum; i++) {
            defaultPositions[i] = new Vector2(CurrentActivateBoxPrf.transform.position.x + 65 * i, CurrentActivateBoxPrf.transform.position.y);
        }
    }


    // 받아온 상태 리스트와 활성화되어있는 prefab 리스트 차이만큼 활성화 생성
    private void BoxAddActivatedStatus(int activeBoxCnt) {
        for (int i = 0; i < activeBoxCnt; i++) {
            for (int j = 0; j < CurrentActivateBoxs.Count; j++) {
                if (!CurrentActivateBoxs[j].activeSelf) {
                    //TODO: 상태 넘기기
                    AddActivatedStatus = StatusControl.Instance.ActivatedStatus[StatusControl.Instance.ActivatedStatus.Count - (activeBoxCnt - i)].type;
                    CurrentActivateBoxs[j].SetActive(true);

                    // 생성 위치 잡기 _ 현재 상태 리스트의 갯수
                    int positionDelta = StatusControl.Instance.ActivatedStatus.Count - (activeBoxCnt - i);
                    CurrentActivateBoxs[j].transform.position = defaultPositions[positionDelta];
                    break;
                }
            }

        }
    }

    // 생성된 box리스트 중 비활성화 된 리스트가 있다면 삭제 후 위치 맞춰야함
    private bool BoxDelActivatedStatus() {
        int cnt = 0;
        for (int i = 0; i < CurrentActivateBoxs.Count; i++) {

            if (!CurrentActivateBoxs[i].activeSelf) {
                // 두 번째 요소를 임시 변수에 저장합니다.
                GameObject temp = CurrentActivateBoxs[i];

                // 두 번째 요소를 리스트에서 제거합니다.
                CurrentActivateBoxs.RemoveAt(i);

                // 저장한 요소를 리스트의 마지막에 추가합니다.
                CurrentActivateBoxs.Add(temp);
                cnt++;
            }
        }

        if (cnt > 0) return true;
        else return false;
    }

    // 생성된 box리스트 중 리스트의 인덱스 순서에 안맞는 위치가 있다면 변경해야함
    private void BoxPositionSetting() {
        for (int i = 0; i < CurrentActivateBoxs.Count; i++) {
            if (CurrentActivateBoxs[i].transform.position.x != defaultPositions[i].x) {
                // 코르틴으로 서서히 맞게할 것
                StartCoroutine(BoxPositionSetting_Co(i));
            }
        }
    }

    private IEnumerator BoxPositionSetting_Co(int i) {
        while (CurrentActivateBoxs[i].transform.position.x != defaultPositions[i].x) {
            Vector3 currentPosition = CurrentActivateBoxs[i].transform.position;
            float step = Time.deltaTime; // 부드럽게 이동하는 정도

            // 현재 박스가 기본 위치 값보다 오른쪽에 있다면
            if (currentPosition.x > defaultPositions[i].x) {
                currentPosition.x = Mathf.Max(currentPosition.x - step, defaultPositions[i].x);
            }
            else {// 현재 박스가 기본 위치 값보다 왼쪽에 있다면
                currentPosition.x = defaultPositions[i].x;
            }
            CurrentActivateBoxs[i].transform.position = currentPosition;
            yield return null;
        }
    }


}
/*
 1. StatusControl.Instance.ActivatedStatus의 갯수만큼 배열 활성화
    - 두개가 한번에 활성화가 되면 어떻게 ㅎ
 2. 박스 리스트를 돌면서 setactive가 false 
 3.  박스 리스트의 순서에서 다르다면 위치 변경

=================
1. 코르틴으로 각각 처리
2. 리스트 전부 다 순회해서 한번에
 */
