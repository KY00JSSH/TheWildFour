using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivatedStatusBox : MonoBehaviour
{
    private ActivatedStatusControl actStatusCont;

    private Status currentStatus;
    private Image sliderImg;

    private void Awake() {
        actStatusCont = FindObjectOfType<ActivatedStatusControl>();
        sliderImg = transform.Find("SlideImg").GetComponent<Image>();
    }

    private void OnEnable() {
        // StatusControl.Instance.ActivatedStatus의 마지막 인덱스의 상태리턴 받아오기
        currentStatus = actStatusCont.AddActivatedStatus;
        AddActivatedStatusInBox();
    }


    // StatusControl.Instance.ActivatedStatus의 마지막 인덱스의 상태를 찾은 인덱스의 번호에 넣어야함
    private void AddActivatedStatusInBox() {

        // 이미지 변경
        switch (currentStatus) {
            case Status.Heat:
                sliderImg.sprite = actStatusCont.ActivatedStatusSprites[1];
                sliderImg.color = Color.blue;
                break;
            case Status.Full:
                sliderImg.sprite = actStatusCont.ActivatedStatusSprites[2];
                sliderImg.color = Color.blue;
                break;
            case Status.Satiety:
                sliderImg.sprite = actStatusCont.ActivatedStatusSprites[3];
                sliderImg.color = Color.blue;
                break;
            case Status.Poison:
                Debug.LogWarning("Status.Poison 제작해야함");
                sliderImg.color = Color.red;
                break;
            case Status.Bleeding:
                Debug.LogWarning("Status.Bleeding 제작해야함");
                sliderImg.color = Color.red;
                break;
            case Status.Blizzard:
                Debug.LogWarning("Status.Blizzard 제작해야함");
                sliderImg.color = Color.red;
                break;
            case Status.Indigestion:
                Debug.LogWarning("Status.Indigestion 제작해야함");
                break;
            case Status.Heal:
                sliderImg.sprite = actStatusCont.ActivatedStatusSprites[4];
                sliderImg.color = Color.blue;
                break;
            default:
                break;
        }
        StartCoroutine(ActivateBoxsSliderChange_Co());
    }

    // slider value = 코르틴 : 슬라이더 값이 0이 될때까지
    private IEnumerator ActivateBoxsSliderChange_Co() {
        while (sliderImg.fillAmount > 0) {
            sliderImg.fillAmount = StatusControl.Instance.GetRemainTime(currentStatus) / StatusControl.Instance.GetTotalTime(currentStatus);
            // 슬라이드가 변화되는 순간에 박스 인덱스
            yield return null;
        }
        // slider value = 0이되면 해당 인덱스 꺼져야함
       gameObject.SetActive(false);
    }

}
/*
 1. 활성화 될때 status를 받아감
 2. 해당 status에 맞춰서 이미지 색상 변경
 3. GetRemainTime / GetTotalTime 만큼 이미지가 변화 되어야함
 4. GetRemainTime 의 시간이 0이된다면 오브젝트가 꺼져야함
 
 */
