using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Build : MonoBehaviour, IMenuButton {

    /*
     * 1. 1��° ��ư Ŭ�� : build ��ư�� �ҵ��� �ϴ� 6�� ��ư�� ����
     * 
     */

    [SerializeField] private Menu_Controll menuControll;
    public Button[] buttons;
    private void Awake() {
        menuControll = FindObjectOfType<Menu_Controll>();
    }



    // ���� ��ư���� �����
    public void I_ButtonOnClick() {
        ButtonPosition();
    }

    public void I_ButtonOffClick() {
        TooltipOff();
        ButtonPositionOff();
    }

    // �ش� ��ư�� ���콺 Ŭ���Ҷ� �����
    public void ButtonOffClick() {
        TooltipOff();
        menuControll.ButtonAction(99);
        ButtonPositionOff();
    }

    public void ButtonOnClick() {
        // ��ġ �����ϰ� ���� ��ũ��Ʈ�� 
        menuControll.ButtonAction(0);
        ButtonPosition();
    }

    // ��ư�� ��ġ ���
    public void ButtonPosition() {
        for (int i = 0; i < buttons.Length; i++) {
            buttons[i].gameObject.SetActive(true);
        }
        menuControll.isEscapeMain = false;
        Debug.Log("Menu_Build��ũ��Ʈ ButtonPosition()�� �ҷ����� ��ġ");
    }

    public void ButtonPositionOff() {
        for (int i = 0; i < buttons.Length; i++) {
            buttons[i].gameObject.SetActive(false);
        }
        menuControll.isEscapeMain = true;
    }

    public void TooltipOff() {
        for (int i = 0; i < buttons.Length; i++) {
            Menu_Tooltip tootip = buttons[i].gameObject.GetComponent<Menu_Tooltip>();
            tootip.tooltipbox.SetActive(false);
        }
    }

}
