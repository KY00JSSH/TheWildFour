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
        if (Input.anyKey) {
            InitButtonEvent();
            isGetKey = true;
        }
        if (isGetKey) {
            ButtonPress();
        }
    }

    private void ButtonPress() {

        isGetKey = false;
        bool isButtonEscape = false;

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
            // ��ü ����ġ 
            isButtonEscape = true;
            buttonIndex = 99;
            buttons[buttons.Length - 1].gameObject.SetActive(true);
        }

        Debug.Log("================================================");
        if ((0 <= buttonIndex && buttonIndex <= 4) || buttonIndex == 99) ButtonAction(isButtonEscape, buttonIndex);

    }



    private void ButtonAction(bool isButtonEscape, int buttonIndex) {
        if (isButtonEscape) {
            // ��ư ������
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
                            // ��ư�� ���� �ö󰡾���
                            buttonRectTransform.anchoredPosition = new Vector3(buttonsPosition[i].x, buttonsPosition[i].y + 50, buttonsPosition[i].z);
                        }
                        else {
                            // ��ư�� �Ʒ��� ����������
                            buttonRectTransform.anchoredPosition = new Vector3(buttonsPosition[i].x, buttonsPosition[i].y - 150, buttonsPosition[i].z);
                        }
                    }
                }

                // buttonIndex ��ȣ�� ��ư �޼ҵ� ���;���
                GetButtonScript(buttonIndex);
            }
            else if (buttonIndex == (buttons.Length - 1)) {
                // ���� ��ư Ȱ��ȭ ���Ѿ���
                Debug.Log("���� ��ư ���õ� => �޼ҵ� ���� ��");
            }
        }
    }


    private void GetButtonScript(int buttonIndex) {

        IMenuButton buttonAction = buttons[buttonIndex].GetComponent<IMenuButton>();
        if (buttonAction != null) {
            Debug.Log(" ��� ����??? ");
            buttons[buttonIndex].onClick.Invoke();
            if (buttons[buttonIndex].interactable == false) {
                Debug.LogWarning("Button at index " + buttonIndex + " is not interactable.");
            }
        }
        else {
            Debug.LogWarning($"��ũ��Ʈ Ȥ�� �������̽� ����");
        }

    }

    private void InitButtonEvent() {
        for (int i = 0; i < buttons.Length; i++) {
            buttons[i].interactable = false;
            buttons[i].interactable = true;
        }
    }

}
