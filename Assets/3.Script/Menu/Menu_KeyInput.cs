using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_KeyInput : MonoBehaviour
{
    /*
     menu에 붙어서 키보드를 입력받는다면 해당 버튼이 클릭되도록 할 예정     
     */

    private Button Build;
    private Button Map;
    private Button Note;
    private Button Bag;
    private Button Weapon; // 여기서는 무기 변경이 일어나야함

    private void Awake()
    {
        Build = transform.GetChild(0).GetComponent<Button>();
        Map = transform.GetChild(1).GetComponent<Button>();
        Note = transform.GetChild(2).GetComponent<Button>();
        Bag = transform.GetChild(3).GetComponent<Button>();
        Weapon = transform.GetChild(4).GetComponent<Button>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            // 건설 열리기
            Debug.Log("건설 열려야함");
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            // 맵 열리기
            Debug.Log("맵 열려야함");
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            // 노트 열리기
            Debug.Log("노트 열려야함");
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            // 가방 열리기
            Debug.Log("가방 열려야함");
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            // 무기 변경
            Debug.Log("무기 변경 열려야함");
        }
    }


}
