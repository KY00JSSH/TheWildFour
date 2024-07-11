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
    //public Button[] buttons;
    private Vector3[] buttonsPosition;

    private int buttonIndex = -1;

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
        if (Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.N)
            || Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape)) {

            Escape();
            ButtonPress();
        }
    }

    // Ű�� Ȯ��
    private int ButtonPress() {
        if (Input.GetKeyDown(KeyCode.B)) {
            // �Ǽ� ������
            Debug.Log("�Ǽ� ��������");
            ButtonMove(150, true);
            GetButtonScript(0);
        }
        else if (Input.GetKeyDown(KeyCode.M)) {
            // �� ������
            Debug.Log("�� ��������");
            CloseUI();
            GetButtonScript(1);
        }
        else if (Input.GetKeyDown(KeyCode.N)) {
            // ��Ʈ ������
            Debug.Log("��Ʈ ��������");
            CloseUI();
            GetButtonScript(2);
        }
        else if (Input.GetKeyDown(KeyCode.V)) {
            // ���� ������
            Debug.Log("���� ��������");
            CloseUI();
            GetButtonScript(3);
        }
        else if (Input.GetKeyDown(KeyCode.X)) {
            // ���� ����
            Debug.Log("���� ���� ��������");
            GetButtonScript(4);
        }
        else if (Input.GetKeyDown(KeyCode.Escape)) {
            buttons[buttons.Length - 1].gameObject.SetActive(true);
            Escape();
        }
        return buttonIndex;
    }


    // �Ǽ��� �ε����� ��ġ ����
    public void ButtonMove(int moveamount, bool isBuildMove) {
        for (int i = 0; i < buttons.Length; i++) {
            if (i == (buttons.Length - 1)) {
                buttons[i].gameObject.SetActive(false);
            }
            else {
                RectTransform buttonRectTransform = buttons[i].GetComponent<RectTransform>();
                if (i == 0) {
                    if (isBuildMove) {
                        // ��ư�� ���� �ö󰡾���
                        buttonRectTransform.anchoredPosition = new Vector3(buttonsPosition[i].x, buttonsPosition[i].y + 50, buttonsPosition[i].z);
                    }
                }
                else {
                    // ��ư�� �Ʒ��� ����������
                    buttonRectTransform.anchoredPosition = new Vector3(buttonsPosition[i].x, buttonsPosition[i].y - moveamount, buttonsPosition[i].z);
                }
            }
        }
    }

    // ��, ���, ���� -> UI��ư ����
    public void CloseUI() {
        for (int i = 0; i < buttons.Length; i++) {
            buttons[i].gameObject.SetActive(false);
        }
    }


    // �ʱ�ȭ
    public void Escape() {
        // ��ư ������
        for (int i = 0; i < buttons.Length; i++) {
            buttons[i].gameObject.SetActive(true);
            RectTransform buttonRectTransform = buttons[i].GetComponent<RectTransform>();
            buttonRectTransform.anchoredPosition = buttonsPosition[i];
        }
        InitButtonEvent();
    }

    // ��ư�̺�Ʈ �ҷ�����
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
