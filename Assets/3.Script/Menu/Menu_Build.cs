using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Build : MonoBehaviour
{
    /*
     * 1. 1��° ��ư Ŭ�� : build ��ư�� �ҵ��� �ϴ� 6�� ��ư�� ����
     */

    public Button[] buttons;

    /*
    public Button campfire;
    public Button furnace;
    public Button shelter;
    public Button workshop;
    public Button chest;
    public Button sign;
    */

    public void ButtonOnClick()
    {
        ButtonPosition();
    }

    // ��ư�� ��ġ ���
    private void ButtonPosition()
    {
        //transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 50, transform.localPosition.z);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(true);
        }
    }
}
