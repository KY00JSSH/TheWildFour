using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class Menu_Controll : MonoBehaviour {
    /*
     menu�� �پ Ű���带 �Է¹޴´ٸ� �ش� ��ư�� Ŭ���ǵ��� �� ����     
     */
    [SerializeField] private Button[] buttons;
    private Vector3[] buttonsPosition;
    private bool isGetKey = false;
    public bool isEscapeMain = true;

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

                Debug.Log("Menu_Controll ��ũ��Ʈ isEscapeMain�� ����� ��ġ" + isEscapeMain);
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
            if (isEscapeMain) {
                Debug.Log("Menu_Controll ��ũ��Ʈ isEscapeMain�� �ش��ϴ� ��ġ" + isEscapeMain);

                buttonIndex = 99;
                buttons[buttons.Length - 1].gameObject.SetActive(true);
            }
        }
        return buttonIndex;
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
