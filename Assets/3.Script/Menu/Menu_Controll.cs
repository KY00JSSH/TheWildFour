using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class Menu_Controll : MonoBehaviour {
    /*
     menu에 붙어서 키보드를 입력받는다면 해당 버튼이 클릭되도록 할 예정     
     */
    [SerializeField] private Button[] buttons;
    private Vector3[] buttonsPosition;

    private int buttonIndex = -1;

    private void Awake() {
        buttonsPosition = new Vector3[buttons.Length];
        for (int i = 0; i < buttons.Length; i++) {
            buttons[i] = transform.GetChild(i).GetComponent<Button>();
            RectTransform buttonRectTransform = buttons[i].GetComponent<RectTransform>();
            buttonsPosition[i] = buttonRectTransform.anchoredPosition;
            Debug.Log("buttonsPosition rect포지션" + buttonsPosition[i]);
        }
    }

    private void Update() {
        ButtonPress();
    }

    private void ButtonPress() {
        bool isButtonEscape = false;

        if (Input.GetKeyDown(KeyCode.B)) {
            // 건설 열리기
            Debug.Log("건설 열려야함");
            buttonIndex = 0;
        }
        else if (Input.GetKeyDown(KeyCode.M)) {
            // 맵 열리기
            Debug.Log("맵 열려야함");
            buttonIndex = 1;
        }
        else if (Input.GetKeyDown(KeyCode.N)) {
            // 노트 열리기
            Debug.Log("노트 열려야함");
            buttonIndex = 2;
        }
        else if (Input.GetKeyDown(KeyCode.V)) {
            // 가방 열리기
            Debug.Log("가방 열려야함");
            buttonIndex = 3;
        }
        else if (Input.GetKeyDown(KeyCode.X)) {
            // 무기 변경
            Debug.Log("무기 변경 열려야함");
            buttonIndex = 4;
        }
        else if (Input.GetKeyDown(KeyCode.Escape)) {
            // 전체 원위치 
            isButtonEscape = true;
            buttonIndex = 99;
            buttons[buttons.Length - 1].gameObject.SetActive(true);
        }

        if ((0 <= buttonIndex && buttonIndex <= 4) || buttonIndex == 99) ButtonAction(isButtonEscape, buttonIndex);

    }



    private void ButtonAction(bool isButtonEscape, int buttonIndex) {
        if (isButtonEscape) {
            // 버튼 나가기
            for (int i = 0; i < buttons.Length; i++) {
                RectTransform buttonRectTransform = buttons[i].GetComponent<RectTransform>();
                buttonRectTransform.anchoredPosition = buttonsPosition[i];
            }
        }
        else {
            if (buttonIndex <= (buttons.Length - 1)) {
                for (int i = 0; i < buttons.Length; i++) {

                    if (i == (buttons.Length - 1)) {
                        buttons[i].gameObject.SetActive(false);
                    }
                    else {
                        RectTransform buttonRectTransform = buttons[i].GetComponent<RectTransform>();
                        if (i <= buttonIndex) {
                            // 버튼이 위로 올라가야함
                            buttonRectTransform.anchoredPosition = new Vector3(buttonsPosition[i].x, buttonsPosition[i].y + 50, buttonsPosition[i].z);
                        }
                        else {
                            // 버튼이 아래로 내려가야함
                            buttonRectTransform.anchoredPosition = new Vector3(buttonsPosition[i].x, buttonsPosition[i].y - 150, buttonsPosition[i].z);
                        }
                    }
                }

                // buttonIndex 번호의 버튼 메소드 들고와야함
            }
            else if (buttonIndex == (buttons.Length - 1)) {
                // 무기 버튼 활성화 시켜야함
                Debug.Log("무기 버튼 선택됨 => 메소드 넣을 것");
            }
        }
    }


}
