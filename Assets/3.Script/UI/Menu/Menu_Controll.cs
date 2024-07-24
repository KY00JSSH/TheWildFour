using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class Menu_Controll : MonoBehaviour {

    /*
     * 스크립트 위치 : Menu
     * 역할 : 키보드 입력 -> 버튼 클릭
     *        버튼의 하위 버튼이 열려있을 경우 esc키는 하위 버튼에 붙어야함 
     */

    [SerializeField] private Button[] buttons;
    //public Button[] buttons;
    private Vector3[] buttonsPosition;

    private int buttonIndex = -1;

    private void Awake() {
        buttonsPosition = new Vector3[buttons.Length];
        for (int i = 0; i < buttons.Length; i++) {
            buttons[i] = transform.GetChild(i).GetComponent<Button>();
            RectTransform buttonRectTransform = buttons[i].GetComponent<RectTransform>();
            buttonsPosition[i] = buttonRectTransform.anchoredPosition;
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.N)
            || Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape)) {

            Escape();
            ButtonPress();
        }
    }

    // 키값 확인
    private int ButtonPress() {
        if (Input.GetKeyDown(KeyCode.B)) {
            ButtonMove(150, false);
            GetButtonScript(0);
        }
        else if (Input.GetKeyDown(KeyCode.M)) {
            CloseUI();
            GetButtonScript(1);
        }
        else if (Input.GetKeyDown(KeyCode.N)) {
            CloseUI();
            GetButtonScript(2);
        }
        else if (Input.GetKeyDown(KeyCode.V)) {
            CloseUI();
            GetButtonScript(3);
        }
        else if (Input.GetKeyDown(KeyCode.X)) {
            GetButtonScript(4);
        }
        else if (Input.GetKeyDown(KeyCode.Escape)) {
            buttons[buttons.Length - 1].gameObject.SetActive(true);
            Escape();
        }
        return buttonIndex;
    }


    // 건설만 인덱스로 위치 변경
    public void ButtonMove(int moveamount, bool isBuildMove) {
        for (int i = 0; i < buttons.Length; i++) {
            if (i == (buttons.Length - 1)) {
                buttons[i].gameObject.SetActive(false);
            }
            else {
                RectTransform buttonRectTransform = buttons[i].GetComponent<RectTransform>();
                if (i == 0) {
                    if (isBuildMove) {
                        // 버튼이 위로 올라가야함
                        buttonRectTransform.anchoredPosition = new Vector3(buttonsPosition[i].x, buttonsPosition[i].y + 50, buttonsPosition[i].z);
                    }
                }
                else {
                    // 버튼이 아래로 내려가야함
                    buttonRectTransform.anchoredPosition = new Vector3(buttonsPosition[i].x, buttonsPosition[i].y - moveamount, buttonsPosition[i].z);
                }
            }
        }
    }

    // 맵, 기록, 가방 -> UI버튼 닫힘
    public void CloseUI() {
        for (int i = 0; i < buttons.Length; i++) {
            buttons[i].gameObject.SetActive(false);
        }
    }


    // 초기화
    public void Escape() {
        // 버튼 나가기
        for (int i = 0; i < buttons.Length; i++) {
            buttons[i].gameObject.SetActive(true);
            RectTransform buttonRectTransform = buttons[i].GetComponent<RectTransform>();
            buttonRectTransform.anchoredPosition = buttonsPosition[i];
        }
        InitButtonEvent();
    }

    // 버튼이벤트 불러오기
    private void GetButtonScript(int buttonIndex) {

        IMenuButton buttonAction = buttons[buttonIndex].GetComponent<IMenuButton>();
        if (buttonAction != null) {
            buttonAction.I_ButtonOnClick();
            if (buttons[buttonIndex].interactable == false) {
                Debug.LogWarning("Button at index " + buttonIndex + " is not interactable.");
            }
        }
        else {
            Debug.LogWarning($"스크립트 혹은 인터페이스 없음");
        }

    }

    // 버튼 이벤트 초기화
    private void InitButtonEvent() {
        for (int i = 0; i < buttons.Length; i++) {

            IMenuButton buttonAction = buttons[i].GetComponent<IMenuButton>();
            if (buttonAction != null) {
                buttonAction.I_ButtonOffClick();
                if (buttons[i].interactable == false) {
                    Debug.LogWarning("Button at index " + i + " is not interactable.");
                }
            }
            else {
                Debug.LogWarning($"스크립트 혹은 인터페이스 없음");
            }
        }
    }

    private void OnDisable() {
        Escape();
    }

}
