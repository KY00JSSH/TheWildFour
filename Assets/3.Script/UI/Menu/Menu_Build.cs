using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Build : MonoBehaviour, IMenuButton {
    /*
     * 1. 1번째 버튼 클릭 : build 버튼에 불들어옴 하단 6개 버튼이 보임
     * 
     */

    [SerializeField] private Menu_Controll menuControll;
    public Button[] buttons;
    private void Awake() {
        menuControll = FindObjectOfType<Menu_Controll>();
    }

    /*
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ButtonOffClick();
        }
    }
    */



    // 상위 버튼에서 사용함
    public void I_ButtonOnClick() {
        ButtonPosition();
    }

    public void I_ButtonOffClick() {
        TooltipOff();
        ButtonPositionOff();
    }

    // 해당 버튼을 마우스 클릭할때 사용함
    public void ButtonOffClick() {
        TooltipOff();
        menuControll.ButtonAction(99);
        ButtonPositionOff();
    }

    public void ButtonOnClick() {
        // 위치 변경하고 상위 스크립트의 
        menuControll.ButtonAction(0);
        ButtonPosition();
    }

    // 버튼의 위치 잡기
    public void ButtonPosition() {
        for (int i = 0; i < buttons.Length; i++) {
            buttons[i].gameObject.SetActive(true);
        }
        menuControll.isEscapeMain = false;
        Debug.Log("Menu_Build스크립트 ButtonPosition()가 불러오는 위치");
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
