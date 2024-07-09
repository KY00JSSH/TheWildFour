using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Build : MonoBehaviour, IMenuButton {
    /*
     * 1. 1번째 버튼 클릭 : build 버튼에 불들어옴 하단 6개 버튼이 보임
     * 2. 마우스를 버튼에 올리면 하이라이트 + 툴팁
     */


    public Button[] buttons;

    public void ButtonOffClick() {
        ButtonPositionOff();
    }

    public void ButtonOnClick() {
        ButtonPosition();
    }

    // 버튼의 위치 잡기
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
