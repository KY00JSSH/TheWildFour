using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Build : MonoBehaviour, IMenuButton {
    /*
     * 1. 1��° ��ư Ŭ�� : build ��ư�� �ҵ��� �ϴ� 6�� ��ư�� ����
     * 2. ���콺�� ��ư�� �ø��� ���̶���Ʈ + ����
     */


    public Button[] buttons;

    public void ButtonOffClick() {
        ButtonPositionOff();
    }

    public void ButtonOnClick() {
        ButtonPosition();
    }

    // ��ư�� ��ġ ���
    public void ButtonPosition() {
        for (int i = 0; i < buttons.Length; i++) {
            buttons[i].gameObject.SetActive(true);
        }
    }

    public void ButtonPositionOff() {
        for (int i = 0; i < buttons.Length; i++) {
            buttons[i].gameObject.SetActive(false);
        }
    }

    private void ButtonTooltip() {

    }
    


}
