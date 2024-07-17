using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Build : MonoBehaviour, IMenuButton {

    /*
     * 1. 1��° ��ư Ŭ�� : build ��ư�� �ҵ��� �ϴ� 6�� ��ư�� ����
     * 
     */

    public Button[] Lowbuttons;
    private Menu_Controll menuControll;
    private int buttonCount = 0;
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

    // ���� ��ư�� Ŭ���ɶ� �ڵ� �ҷ�������
    public void ButtonOffClick() {
        TooltipOff();
        menuControll.Escape();
        ButtonPositionOff();
    }

    // ���콺 Ŭ�� ����
    public void ButtonOnClick() {
        buttonCount++;
        if (buttonCount==2) {
            buttonCount = 0;
            menuControll.Escape();
        }
        else {
            // ��ġ ���� -> ���� ��ũ��Ʈ�� ��ġ�� �� �������ҰͰ���
            menuControll.ButtonMove(150, false);
            ButtonPosition();
        }
    }

    // ��ư�� ��ġ ���
    public void ButtonPosition() {
        for (int i = 0; i < Lowbuttons.Length; i++) {
            Lowbuttons[i].gameObject.SetActive(true);
        }
    }

    public void ButtonPositionOff() {
        for (int i = 0; i < Lowbuttons.Length; i++) {
            Lowbuttons[i].gameObject.SetActive(false);
        }
    }

    public void TooltipOff() {
        Tooltip_Build tootip = gameObject.GetComponent<Tooltip_Build>();
        tootip.tooltipbox.SetActive(false);

    }

}
