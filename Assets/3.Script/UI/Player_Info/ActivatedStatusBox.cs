using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivatedStatusBox : MonoBehaviour
{
    private ActivatedStatusControl actStatusCont;

    private Status currentStatus;
    private Image sliderImg;
    // 빈박스로 돌릴 원형
    private Sprite defaultSprite;
    private Vector2 defaultPos;

    private void Awake() {
        defaultPos = transform.position;
        defaultSprite = transform.GetComponent<Image>().sprite;
        actStatusCont = FindObjectOfType<ActivatedStatusControl>();
        sliderImg = transform.Find("SlideImg").GetComponent<Image>();
    }

    private void OnEnable() {
        // StatusControl.Instance.ActivatedStatus의 마지막 인덱스의 상태리턴 받아오기
        currentStatus = actStatusCont.AddActivatedStatus;
        AddActivatedStatusInBox();
    }

    private void Update() {
        if (sliderImg.fillAmount <=0) {

            // slider value = 0이되면 해당 인덱스 꺼져야함
            gameObject.SetActive(false);
        }
    }


    // StatusControl.Instance.ActivatedStatus의 마지막 인덱스의 상태를 찾은 인덱스의 번호에 넣어야함
    private void AddActivatedStatusInBox() {
        Sprite iconSprite = transform.GetComponent<Image>().sprite;
        // 이미지 변경
        switch (currentStatus) {
            case Status.Heat:
                iconSprite = actStatusCont.ActivatedStatusSprites[1];
                sliderImg.color = Color.blue;
                break;
            case Status.Full:
                iconSprite = actStatusCont.ActivatedStatusSprites[2];
                sliderImg.color = Color.blue;
                break;
            case Status.Satiety:
                iconSprite = actStatusCont.ActivatedStatusSprites[3];
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
                iconSprite = actStatusCont.ActivatedStatusSprites[4];
                sliderImg.color = Color.blue;
                break;
            default:
                break;
        }
        Color newColor = sliderImg.color;  // 현재 이미지의 색상 정보를 가져옴
        newColor.a = 0.5f;  // 알파값을 원하는 값(여기서는 0.5)으로 설정
        sliderImg.color = newColor;  // 변경된 색상 정보를 이미지에 적용

        transform.GetComponent<Image>().sprite = iconSprite;
        StartCoroutine(ActivateBoxsSliderChange_Co());
    }

    // slider value = 코르틴 : 슬라이더 값이 0이 될때까지
    private IEnumerator ActivateBoxsSliderChange_Co() {
        while (sliderImg.fillAmount > 0) {
            sliderImg.fillAmount = StatusControl.Instance.GetRemainTime(currentStatus) / StatusControl.Instance.GetTotalTime(currentStatus);
            // 슬라이드가 변화되는 순간에 박스 인덱스
            yield return null;
        }
    }
    private void OnDisable() {
        transform.position = defaultPos;
        transform.GetComponent<Image>().sprite = defaultSprite;
        sliderImg.fillAmount = 1;
    }
}
/*
 1. 활성화 될때 status를 받아감
 2. 해당 status에 맞춰서 이미지 색상 변경
 3. GetRemainTime / GetTotalTime 만큼 이미지가 변화 되어야함
 4. GetRemainTime 의 시간이 0이된다면 오브젝트가 꺼져야함
 
 */
