using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Bag : MonoBehaviour, IMenuButton {
    [SerializeField] private GameObject TextBox_Bag;
    private Menu_Controll menuControll;
    private void Awake() {
        menuControll = FindObjectOfType<Menu_Controll>();
    }

    // 상위 버튼에서 사용함
    public void I_ButtonOffClick() {
        TextBoxBagOff();
    }

    public void I_ButtonOnClick() {
        TextBoxBagActive();
    }

    public void ButtonOffClick() {
        TextBoxBagOff();
    }

    public void ButtonOnClick() {
        menuControll.CloseUI();
        TextBoxBagActive();
    }

    private void TextBoxBagActive() {
        TextBox_Bag.transform.parent.gameObject.SetActive(true);
        TextBox_Bag.SetActive(true);
    }

    private void TextBoxBagOff() {
        TextBox_Bag.transform.parent.gameObject.SetActive(false);
        TextBox_Bag.SetActive(false);
    }

}
