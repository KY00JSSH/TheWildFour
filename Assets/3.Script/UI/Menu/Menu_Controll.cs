using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class Menu_Controll : MonoBehaviour {

    /*
     * ��ũ��Ʈ ��ġ : Menu
     * ���� : Ű���� �Է� -> ��ư Ŭ��
     *        ��ư�� ���� ��ư�� �������� ��� escŰ�� ���� ��ư�� �پ���� 
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
            Debug.Log("buttonsPosition rect������" + buttonsPosition[i]);
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

    // Ű�� Ȯ��
    private int ButtonPress() {
        isGetKey = false;
        if (Input.GetKeyDown(KeyCode.B)) {
            // �Ǽ� ������
            Debug.Log("�Ǽ� ��������");
            buttonIndex = 0;
        }
        else if (Input.GetKeyDown(KeyCode.M)) {
            // �� ������
            Debug.Log("�� ��������");
            buttonIndex = 1;
        }
        else if (Input.GetKeyDown(KeyCode.N)) {
            // ��Ʈ ������
            Debug.Log("��Ʈ ��������");
            buttonIndex = 2;
        }
        else if (Input.GetKeyDown(KeyCode.V)) {
            // ���� ������
            Debug.Log("���� ��������");
            buttonIndex = 3;
        }
        else if (Input.GetKeyDown(KeyCode.X)) {
            // ���� ����
            Debug.Log("���� ���� ��������");
            buttonIndex = 4;
        }
        else if (Input.GetKeyDown(KeyCode.Escape)) {
            // ��ü ����ġ => ���� ������Ʈ�� �������� ��� �ݱ� ��ư�� ������ �ʰ� �ؾ���
            // ���� : �Ǽ��� ���ٰ� �������� ��� ���ٰ� ������ ����ġ ���Ѿ���

            buttonIndex = 99;
            Escape();
        }
        return buttonIndex;
    }

    private void Escape() {
        if (FindLowerButtonActive()) {
            // ���� ��ư Ȱ��ȭ �Ǿ������� �� �ؿ� �ִ� ��ư ��Ȯ��
            activeButtonLow = FindLowerButtonActive_2(Activebutton_InLow_1);
            if (activeButtonLow != null) {
                activeButtonLow.SetActive(false);
            }
            else {
                // ù��° ���� ��ư �ݱ�
                Transform buttonObj = buttons[Activebutton_InLow_1].gameObject.transform;
                for (int i = 0; i < buttonObj.childCount; i++) {
                    GameObject child = buttonObj.GetChild(i).gameObject;
                    child.SetActive(false);
                }
            }
        }
        else {
            // ���� ��ư�� Ȱ��ȭ�� �ȵǾ����� => ����

            buttons[buttons.Length - 1].gameObject.SetActive(true);
        }
    }

    // ��ư���� ���� ��ư�� Ȱ��ȭ�Ǿ��ִ��� Ȯ��
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


    // �ε����� ��ġ ����
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
                            // ��ư�� ���� �ö󰡾���
                            buttonRectTransform.anchoredPosition = new Vector3(buttonsPosition[i].x, buttonsPosition[i].y + 50, buttonsPosition[i].z);
                        }
                        else {
                            // ��ư�� �Ʒ��� ����������
                            buttonRectTransform.anchoredPosition = new Vector3(buttonsPosition[i].x, buttonsPosition[i].y - 150, buttonsPosition[i].z);
                        }
                    }
                }
                return true;
            case 4:
                // ���� ��ư Ȱ��ȭ ���Ѿ���
                Debug.Log("���� ��ư ���õ� => �޼ҵ� ���� ��");
                return true;
            case 99:
                // ��ư ������
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
            Debug.LogWarning($"��ũ��Ʈ Ȥ�� �������̽� ����");
        }

    }

    // ��ư �̺�Ʈ �ʱ�ȭ
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
                Debug.LogWarning($"��ũ��Ʈ Ȥ�� �������̽� ����");
            }
        }
    }

}
