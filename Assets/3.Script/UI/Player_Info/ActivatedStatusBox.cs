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
        // StatusControl.Instance.ActivatedStatus�� ������ �ε����� ���¸��� �޾ƿ���
        currentStatus = actStatusCont.AddActivatedStatus;
        AddActivatedStatusInBox();
    }


    // StatusControl.Instance.ActivatedStatus�� ������ �ε����� ���¸� ã�� �ε����� ��ȣ�� �־����
    private void AddActivatedStatusInBox() {

        // �̹��� ����
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
                sliderImg.sprite = actStatusCont.ActivatedStatusSprites[4];
                sliderImg.color = Color.blue;
                break;
            default:
                break;
        }
        StartCoroutine(ActivateBoxsSliderChange_Co());
    }

    // slider value = �ڸ�ƾ : �����̴� ���� 0�� �ɶ�����
    private IEnumerator ActivateBoxsSliderChange_Co() {
        while (sliderImg.fillAmount > 0) {
            sliderImg.fillAmount = StatusControl.Instance.GetRemainTime(currentStatus) / StatusControl.Instance.GetTotalTime(currentStatus);
            // �����̵尡 ��ȭ�Ǵ� ������ �ڽ� �ε���
            yield return null;
        }
        // slider value = 0�̵Ǹ� �ش� �ε��� ��������
       gameObject.SetActive(false);
    }

}
/*
 1. Ȱ��ȭ �ɶ� status�� �޾ư�
 2. �ش� status�� ���缭 �̹��� ���� ����
 3. GetRemainTime / GetTotalTime ��ŭ �̹����� ��ȭ �Ǿ����
 4. GetRemainTime �� �ð��� 0�̵ȴٸ� ������Ʈ�� ��������
 
 */
