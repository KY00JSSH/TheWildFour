using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Build : MonoBehaviour, IMenuButton {

    /*
     * 1. 1번째 버튼 클릭 : build 버튼에 불들어옴 하단 6개 버튼이 보임
     * 
     */

    public Button[] Lowbuttons;
    private Menu_Controll menuControll;
    private int buttonCount = 0;
    private void Awake() {
        menuControll = FindObjectOfType<Menu_Controll>();
    }

    // 상위 버튼에서 사용함
    public void I_ButtonOnClick() {
        ButtonPosition();
    }

    public void I_ButtonOffClick() {
        TooltipOff();
        ButtonPositionOff();
    }

    // 하위 버튼이 클릭될때 자동 불러가야함
    public void ButtonOffClick() {
        TooltipOff();
        menuControll.Escape();
        ButtonPositionOff();
    }

    // 마우스 클릭 실행
    public void ButtonOnClick() {
        buttonCount++;
        if (buttonCount==2) {
            buttonCount = 0;
            menuControll.Escape();
        }
        else {
            // 위치 변경 -> 상위 스크립트의 위치를 더 내려야할것같음
            menuControll.ButtonMove(150, false);
            ButtonPosition();
        }
    }

    // 버튼의 위치 잡기
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

        Build_Tooltip tootip = gameObject.GetComponent<Build_Tooltip>();
        tootip.tooltipbox.SetActive(false);
        /*
        for (int i = 0; i < Lowbuttons.Length; i++) {
            Menu_Tooltip tootip = Lowbuttons[i].gameObject.GetComponent<Menu_Tooltip>();
            tootip.tooltipbox.SetActive(false);
        }
        */
    }

}
