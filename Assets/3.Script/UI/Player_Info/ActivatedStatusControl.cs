using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivatedStatusControl : MonoBehaviour
{
    // 현재 활성화 되어있는 이미지 개수
    public GameObject[] CurrentActivateBoxs;
    // 현재 활성화 표시할 스프라이트
    public Sprite[] ActivatedStatusSprites;



    // return 용 status 
    public Status AddActivatedStatus { get { return StatusControl.Instance.ActivatedStatus[StatusControl.Instance.ActivatedStatus.Count - 1].type; } }

    private List<GameObject> statusUIBoxs;

    private Vector2 defaultPos;

    private void Awake() {
        defaultPos = CurrentActivateBoxs[0].transform.position;
        CurrentActivateBoxs = new GameObject[5];
        foreach (GameObject item in CurrentActivateBoxs) {
            statusUIBoxs.Add(item);
        }
    }


}
/*
 1. StatusControl.Instance.ActivatedStatus의 갯수만큼 box list에 넣음
 2. 박스 리스트를 돌면서 setactive가 false면 list에서 빼고 마지막에 넣을 수 있나?
 3.  박스 리스트의 순서에서 다르다면 위치 변경

=================
1. 코르틴으로 각각 처리
2. 리스트 전부 다 순회해서 한번에
 */
