using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class Menu_Controll : MonoBehaviour
{
    /*
     menu에 붙어서 키보드를 입력받는다면 해당 버튼이 클릭되도록 할 예정     
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
                // 건설 열리기
                Debug.Log("건설 열려야함");
                buttonIndex = 0;
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                // 맵 열리기
                Debug.Log("맵 열려야함");
                buttonIndex = 1;
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                // 노트 열리기
                Debug.Log("노트 열려야함");
                buttonIndex = 2;
            }
            else if (Input.GetKeyDown(KeyCode.V))
            {
                // 가방 열리기
                Debug.Log("가방 열려야함");
                buttonIndex = 3;
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                // 무기 변경
                Debug.Log("무기 변경 열려야함");
                buttonIndex = 4;
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                // 전체 원위치 
                isButtonEscape = true;
                buttonIndex = 99;
            }


            ButtonAction(isButtonEscape, buttonIndex);
        }
        
    }



    private void ButtonAction(bool isButtonEscape, int buttonIndex)
    {
       // Debug.Log(" 여기를 어떻게 막지");

        if (isButtonEscape)
        {
            // 버튼 나가기
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
                    // 버튼이 위로 올라가야함
                    buttons[i].transform.localPosition = new Vector3(buttonsPosition[i].x, buttonsPosition[i].y + 100 * i, buttonsPosition[i].z);
                }
                else if(i == buttonIndex)
                {
                    buttons[i].transform.localPosition = new Vector3(buttonsPosition[i].x, buttonsPosition[i].y + 50 * i, buttonsPosition[i].z);
                }
                else
                {
                    // 버튼이 아래로 내려가야함
                    buttons[i].transform.localPosition = new Vector3(buttonsPosition[i].x, buttonsPosition[i].y - 100 * i, buttonsPosition[i].z);
                }
            }
        }
    }


}
