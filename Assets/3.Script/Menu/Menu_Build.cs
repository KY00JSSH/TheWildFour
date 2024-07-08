using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Build : MonoBehaviour, IMenuButton {
    /*
     * 1. 1��° ��ư Ŭ�� : build ��ư�� �ҵ��� �ϴ� 6�� ��ư�� ����
     */

    public Button[] buttons;

    public void ButtonOffClick() {
        ButtonPositionOff();
    }

    public void ButtonOnClick() {
        Debug.Log("�� ����� �ȵ����� �ǰ�?");
        ButtonPosition();
    }

    // ��ư�� ��ġ ���
    public void ButtonPosition() {
        //transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 50, transform.localPosition.z);
        for (int i = 0; i < buttons.Length; i++) {
            buttons[i].gameObject.SetActive(true);
        }
    }

    public void ButtonPositionOff() {
        //transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 50, transform.localPosition.z);
        for (int i = 0; i < buttons.Length; i++) {
            buttons[i].gameObject.SetActive(false);
        }
    }
}
