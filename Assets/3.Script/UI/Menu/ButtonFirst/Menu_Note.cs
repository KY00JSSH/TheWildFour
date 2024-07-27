using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Note : MonoBehaviour, IMenuButton {
    [SerializeField] private GameObject TextBox_Note;
    private Menu_Controll menuControll;
    private void Awake() {
        menuControll = FindObjectOfType<Menu_Controll>();
    }

    // 상위 버튼에서 사용함
    public void I_ButtonOffClick() {
        TextBoxNoteOff();
    }

    public void I_ButtonOnClick() {
        TextBoxNoteActive();
    }

    public void ButtonOffClick() {
        TextBoxNoteOff();
    }

    public void ButtonOnClick() {
        menuControll.isMenuButtonOpen = true;
        menuControll.CloseUI();
        TextBoxNoteActive();
    }

    private void TextBoxNoteActive() {
        TextBox_Note.transform.parent.gameObject.SetActive(true);
        TextBox_Note.SetActive(true);
    }

    private void TextBoxNoteOff() {
        TextBox_Note.transform.parent.gameObject.SetActive(false);
        TextBox_Note.SetActive(false);
    }


}
