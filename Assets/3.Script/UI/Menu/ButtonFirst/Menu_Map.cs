using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Map : MonoBehaviour, IMenuButton {
    [SerializeField] private GameObject TextBox_Map;
    private Menu_Controll menuControll;
    private void Awake() {
        menuControll = FindObjectOfType<Menu_Controll>();
    }

    // 상위 버튼에서 사용함
    public void I_ButtonOffClick() {
        TextBoxMapOff();
    }

    public void I_ButtonOnClick() {
        TextBoxMapActive();
    }

    public void ButtonOffClick() {
        TextBoxMapOff();
    }

    public void ButtonOnClick() {
        menuControll.CloseUI();
        TextBoxMapActive();
    }

    private void TextBoxMapActive() {
        TextBox_Map.transform.parent.gameObject.SetActive(true);
        TextBox_Map.SetActive(true);
    }

    private void TextBoxMapOff() {
        TextBox_Map.transform.parent.gameObject.SetActive(false);
        TextBox_Map.SetActive(false);
    }

}
