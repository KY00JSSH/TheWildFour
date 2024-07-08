using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Build : MonoBehaviour, IMenuButton {
    /*
     * 1. 1번째 버튼 클릭 : build 버튼에 불들어옴 하단 6개 버튼이 보임
     */

    public Button[] buttons;

    public void ButtonOffClick() {
        ButtonPositionOff();
    }

    public void ButtonOnClick() {
        Debug.Log("왜 여기로 안들어오는 건가?");
        ButtonPosition();
    }

    // 버튼의 위치 잡기
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
