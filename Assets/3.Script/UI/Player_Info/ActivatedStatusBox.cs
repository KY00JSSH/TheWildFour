using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivatedStatusBox : MonoBehaviour
{
    private ActivatedStatusControl actStatusCont;

    private Status currentStatus;
    private Image sliderImg;
    // ��ڽ��� ���� ����
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
        // StatusControl.Instance.ActivatedStatus�� ������ �ε����� ���¸��� �޾ƿ���
        currentStatus = actStatusCont.AddActivatedStatus;
        AddActivatedStatusInBox();
    }

    private void Update() {
        if (sliderImg.fillAmount <=0) {

            // slider value = 0�̵Ǹ� �ش� �ε��� ��������
            gameObject.SetActive(false);
        }
    }


    // StatusControl.Instance.ActivatedStatus�� ������ �ε����� ���¸� ã�� �ε����� ��ȣ�� �־����
    private void AddActivatedStatusInBox() {
        Image iconSprite = statusImg;
        // �̹��� ����
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
                Debug.LogWarning("Status.Poison �����ؾ���");
                sliderImg.color = Color.red;
                break;
            case Status.Bleeding:
                Debug.LogWarning("Status.Bleeding �����ؾ���");
                sliderImg.color = Color.red;
                break;
            case Status.Blizzard:
                Debug.LogWarning("Status.Blizzard �����ؾ���");
                sliderImg.color = Color.red;
                break;
            case Status.Indigestion:
                Debug.LogWarning("Status.Indigestion �����ؾ���");
                break;
            case Status.Heal:
                iconSprite.sprite = actStatusCont.ActivatedStatusSprites[4];
                sliderImg.color = Color.blue;
                break;
            default:
                break;
        }
        Color newColor = sliderImg.color;  // ���� �̹����� ���� ������ ������
        newColor.a = 0.5f;  // ���İ��� ���ϴ� ��(���⼭�� 0.5)���� ����
        sliderImg.color = newColor;  // ����� ���� ������ �̹����� ����
        statusImg.sprite = iconSprite.sprite;
        StartCoroutine(ActivateBoxsSliderChange_Co());
    }

    // slider value = �ڸ�ƾ : �����̴� ���� 0�� �ɶ�����
    private IEnumerator ActivateBoxsSliderChange_Co() {
        while (sliderImg.fillAmount > 0) {
            sliderImg.fillAmount = StatusControl.Instance.GetRemainTime(currentStatus) / StatusControl.Instance.GetTotalTime(currentStatus);
            // �����̵尡 ��ȭ�Ǵ� ������ �ڽ� �ε���
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
 1. Ȱ��ȭ �ɶ� status�� �޾ư�
 2. �ش� status�� ���缭 �̹��� ���� ����
 3. GetRemainTime / GetTotalTime ��ŭ �̹����� ��ȭ �Ǿ����
 4. GetRemainTime �� �ð��� 0�̵ȴٸ� ������Ʈ�� ��������
 
 */
