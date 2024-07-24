using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivatedStatusControl : MonoBehaviour {
    // 현재 활성화 되어있는 이미지 개수
    public GameObject[] CurrentActivateBoxs;
    // 현재 활성화 표시할 스프라이트
    public Sprite[] ActivatedStatusSprites;

    // return 용 status 
    public Status AddActivatedStatus { get; private set; }

    private bool isPosChangeComplete = false;

    private void Update() {
        if (StatusControl.Instance.ActivatedStatus != null) {
            // 상태 리스트가 null이 아닐 경우 활성화 오브젝트 개수 확인
            if (StatusControl.Instance.ActivatedStatus.Count > FindActiveBox()) {
                Debug.Log("활성화 개수 : " + StatusControl.Instance.ActivatedStatus.Count);
                BoxAddActivatedStatus();
                Debug.Log("박스 활성화 status : " + AddActivatedStatus);
                Debug.Log("박스 활성화 개수 : " + FindActiveBox());

            }

        }
        if (StatusControl.Instance.ActivatedStatus.Count <= FindActiveBox()) BoxAddActivePositionChange();

    }

    // 활성화 되어있는 박스 오브젝트 개수 확인
    private int FindActiveBox() {
        int count = 0;
        for (int i = 0; i < CurrentActivateBoxs.Length; i++) {
            if (CurrentActivateBoxs[i].activeSelf) count++;
        }
        return count;
    }


    // 활성화 상태만큼 배열 활성화 해야함 => 상태 넘겨줘야함
    private void BoxAddActivatedStatus() {
        for (int i = 0; i < CurrentActivateBoxs.Length; i++) {
            if (!CurrentActivateBoxs[i].activeSelf) {
                AddActivatedStatus = StatusControl.Instance.ActivatedStatus[StatusControl.Instance.ActivatedStatus.Count - 1].type;
                CurrentActivateBoxs[i].SetActive(true);
                int positionDelta = StatusControl.Instance.ActivatedStatus.Count - 1;
                CurrentActivateBoxs[i].transform.position = new Vector2(CurrentActivateBoxs[i].transform.position.x + 65 * positionDelta, CurrentActivateBoxs[i].transform.position.y);
                return;
            }
        }
    }

    // 활성화가 끝난 박스를 찾아서 그뒤에 있는 박스들의 위치를 옮겨야함
    private void BoxAddActivePositionChange() {

        Debug.Log(StatusControl.Instance.ActivatedStatus.Count);
        Debug.Log("/ " + FindActiveBox());
        Debug.Log("BoxAddActivePositionChange !!!:");
        for (int i = 0; i < CurrentActivateBoxs.Length - 1; i++) {
            if (!CurrentActivateBoxs[i].activeSelf) {
                Debug.Log("CurrentActivateBoxs[i] :" + CurrentActivateBoxs[i].name);
                for (int j = i + 1; j < CurrentActivateBoxs.Length; j++) {
                    if (CurrentActivateBoxs[j].activeSelf) {
                        Debug.Log("CurrentActivateBoxs[j] :" + CurrentActivateBoxs[j].name);
                        CurrentActivateBoxs[j].transform.position = new Vector2(CurrentActivateBoxs[j].transform.position.x - 65, CurrentActivateBoxs[j].transform.position.y);
                    }
                }

            }
        }
    }

    // 활성화 된 박스 포지션확인해서 원위치 찾으면 중단 메소드


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
