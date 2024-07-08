using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_KeyInput : MonoBehaviour
{
    /*
     menu�� �پ Ű���带 �Է¹޴´ٸ� �ش� ��ư�� Ŭ���ǵ��� �� ����     
     */

    private Button Build;
    private Button Map;
    private Button Note;
    private Button Bag;
    private Button Weapon; // ���⼭�� ���� ������ �Ͼ����

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
            // �Ǽ� ������
            Debug.Log("�Ǽ� ��������");
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            // �� ������
            Debug.Log("�� ��������");
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            // ��Ʈ ������
            Debug.Log("��Ʈ ��������");
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            // ���� ������
            Debug.Log("���� ��������");
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            // ���� ����
            Debug.Log("���� ���� ��������");
        }
    }


}
