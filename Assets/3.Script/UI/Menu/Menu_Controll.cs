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
    private Vector3[] buttonsPosition;
    private bool isGetKey = false;

    private int buttonIndex = -1;
    private int Activebutton_InLow_1 = -1;
    private GameObject activeButtonLow;

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
        /*
        if (Input.anyKey) {
            InitButtonEvent();
            isGetKey = true;
        }
        */
        if (Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.N)
            || Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape)) {

            InitButtonEvent();
            int btIndex = ButtonPress();

            if ((0 <= btIndex && btIndex <= 4) || btIndex == 99) {

                if (ButtonAction(btIndex)) GetButtonScript(btIndex);
            }
        }
    }

    // 키값 확인
    private int ButtonPress() {
        isGetKey = false;
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
            // 전체 원위치 => 하위 오브젝트가 열려있을 경우 닫기 버튼은 누리지 않게 해야함
            // 예시 : 건설의 툴바가 열려있을 경우 툴바가 닫히고 원위치 시켜야함

            buttonIndex = 99;
            Escape();
        }
        return buttonIndex;
    }

    private void Escape() {
        if (FindLowerButtonActive()) {
            // 하위 버튼 활성화 되어있으면 그 밑에 있는 버튼 재확인
            activeButtonLow = FindLowerButtonActive_2(Activebutton_InLow_1);
            if (activeButtonLow != null) {
                activeButtonLow.SetActive(false);
            }
            else {
                // 첫번째 하위 버튼 닫기
                Transform buttonObj = buttons[Activebutton_InLow_1].gameObject.transform;
                for (int i = 0; i < buttonObj.childCount; i++) {
                    GameObject child = buttonObj.GetChild(i).gameObject;
                    child.SetActive(false);
                }
            }
        }
        else {
            // 하위 버튼이 활성화가 안되어있음 => 삭제

            buttons[buttons.Length - 1].gameObject.SetActive(true);
        }
    }

    // 버튼들의 하위 버튼이 활성화되어있는지 확인
    private bool FindLowerButtonActive() {
        for (int i = 0; i < buttons.Length; i++) {
            if (buttons[i].gameObject.activeSelf) {
                Transform buttonObj = buttons[i].gameObject.transform;
                if (buttonObj.childCount == 0) continue;
                for (int j = 0; j < buttonObj.childCount; j++) {
                    GameObject child = buttonObj.GetChild(j).gameObject;
                    if (child != null) {
                        if (child.activeSelf) {
                            Debug.Log("Active child: " + child.name);
                            Activebutton_InLow_1 = i;
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
    private GameObject FindLowerButtonActive_2(int Activebutton_InLow_1) {
        Transform buttonObj = buttons[Activebutton_InLow_1].gameObject.transform;
        if (buttonObj.childCount == 0) return null;
        for (int i = 0; i < buttonObj.childCount; i++) {
            GameObject child = buttonObj.GetChild(i).gameObject;
            if (child != null) {
                if (child.activeSelf) {
                    Debug.Log("Active child: " + child.name);
                    return child;
                }
            }
        }
        return null;
    }


    // 인덱스로 위치 변경
    public bool ButtonAction(int buttonIndex) {
        switch (buttonIndex) {
            case 0:
            case 1:
            case 2:
            case 3:
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
                return true;
            case 4:
                // 무기 버튼 활성화 시켜야함
                Debug.Log("무기 버튼 선택됨 => 메소드 넣을 것");
                return true;
            case 99:
                // 버튼 나가기
                for (int i = 0; i < buttons.Length; i++) {
                    RectTransform buttonRectTransform = buttons[i].GetComponent<RectTransform>();
                    buttonRectTransform.anchoredPosition = buttonsPosition[i];
                }
                return false;
            default:
                return false;
        }
    }


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

}
