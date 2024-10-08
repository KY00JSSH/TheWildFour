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

    private Image statusImg;

    private void Awake() {
        defaultPos = transform.position;
        actStatusCont = FindObjectOfType<ActivatedStatusControl>();

        defaultSprite = transform.Find("StatusImg").GetComponent<Image>().sprite;
        statusImg = transform.Find("StatusImg").GetComponent<Image>();
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
        Image iconSprite = statusImg;
        // 이미지 변경
        switch (currentStatus) {
            case Status.Heat:
                iconSprite.sprite = actStatusCont.ActivatedStatusSprites[1];
                sliderImg.color = Color.blue;
                break;
            case Status.Full:
                iconSprite.sprite = actStatusCont.ActivatedStatusSprites[2];
                sliderImg.color = Color.blue;
                break;
            case Status.Satiety:
                iconSprite.sprite = actStatusCont.ActivatedStatusSprites[3];
                sliderImg.color = Color.blue;
                break;
            case Status.Poison:
                iconSprite.sprite = actStatusCont.ActivatedStatusSprites[4];
                sliderImg.color = Color.red;
                break;
            case Status.Bleeding:
                iconSprite.sprite = actStatusCont.ActivatedStatusSprites[5];
                sliderImg.color = Color.red;
                break;
            case Status.Blizzard:
                iconSprite.sprite = actStatusCont.ActivatedStatusSprites[6];
                sliderImg.color = Color.red;
                break;
            case Status.Indigestion:
                iconSprite.sprite = actStatusCont.ActivatedStatusSprites[7];
                break;
            case Status.Heal:
                iconSprite.sprite = actStatusCont.ActivatedStatusSprites[8];
                sliderImg.color = Color.blue;
                break;
            default:
                break;
        }
        Color newColor = sliderImg.color;  // 현재 이미지의 색상 정보를 가져옴
        newColor.a = 0.5f;  // 알파값을 원하는 값(여기서는 0.5)으로 설정
        sliderImg.color = newColor;  // 변경된 색상 정보를 이미지에 적용
        statusImg.sprite = iconSprite.sprite;
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
        statusImg.sprite = defaultSprite;
        sliderImg.fillAmount = 1;
    }
}
/*
 1. 활성화 될때 status를 받아감
 2. 해당 status에 맞춰서 이미지 색상 변경
 3. GetRemainTime / GetTotalTime 만큼 이미지가 변화 되어야함
 4. GetRemainTime 의 시간이 0이된다면 오브젝트가 꺼져야함
 
 */
