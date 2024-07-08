using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class Menu_Controll : MonoBehaviour
{
    /*
     menu�� �پ Ű���带 �Է¹޴´ٸ� �ش� ��ư�� Ŭ���ǵ��� �� ����     
     */
    [SerializeField] private Button[] buttons;
    private Vector3[] buttonsPosition;

    private int buttonIndex = 99;

    private void Awake()
    {
        buttonsPosition = new Vector3[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i] = transform.GetChild(i).GetComponent<Button>();

            RectTransform buttonRectTransform = buttons[i].GetComponent<RectTransform>();
            buttonsPosition[i] = buttonRectTransform.anchoredPosition;
        }

        /*
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i] = transform.GetChild(i).GetComponent<Button>();
            RectTransform buttonRectTransform = buttons[i].transform as RectTransform;
            buttonsPosition[i] = buttonRectTransform.anchoredPosition;
        }
        */

    }

    private void Update()
    {
        ButtonPress();
    }

    private void ButtonPress()
    {
        if (Input.anyKey)
        {
            bool isButtonEscape = false;

            if (Input.GetKeyDown(KeyCode.B))
            {
                // �Ǽ� ������
                Debug.Log("�Ǽ� ��������");
                buttonIndex = 0;
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                // �� ������
                Debug.Log("�� ��������");
                buttonIndex = 1;
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                // ��Ʈ ������
                Debug.Log("��Ʈ ��������");
                buttonIndex = 2;
            }
            else if (Input.GetKeyDown(KeyCode.V))
            {
                // ���� ������
                Debug.Log("���� ��������");
                buttonIndex = 3;
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                // ���� ����
                Debug.Log("���� ���� ��������");
                buttonIndex = 4;
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                // ��ü ����ġ 
                isButtonEscape = true;
                buttonIndex = 99;
            }


            ButtonAction(isButtonEscape, buttonIndex);
        }
        
    }



    private void ButtonAction(bool isButtonEscape, int buttonIndex)
    {
       // Debug.Log(" ���⸦ ��� ����");

        if (isButtonEscape)
        {
            // ��ư ������
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].transform.localPosition = buttonsPosition[i];
            }
        }
        else
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (i <= buttonIndex)
                {
                    // ��ư�� ���� �ö󰡾���
                    buttons[i].transform.localPosition = new Vector3(buttonsPosition[i].x, buttonsPosition[i].y + 100 * i, buttonsPosition[i].z);
                }
                else if(i == buttonIndex)
                {
                    buttons[i].transform.localPosition = new Vector3(buttonsPosition[i].x, buttonsPosition[i].y + 50 * i, buttonsPosition[i].z);
                }
                else
                {
                    // ��ư�� �Ʒ��� ����������
                    buttons[i].transform.localPosition = new Vector3(buttonsPosition[i].x, buttonsPosition[i].y - 100 * i, buttonsPosition[i].z);
                }
            }
        }
    }


}
