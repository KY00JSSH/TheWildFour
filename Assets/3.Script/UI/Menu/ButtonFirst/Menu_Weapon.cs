using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Weapon : MonoBehaviour, IMenuButton {
    
    private Menu_Controll menuControll;
    private void Awake() {
        menuControll = FindObjectOfType<Menu_Controll>();
    }

    // ���� ��ư���� �����
    public void I_ButtonOffClick() {
    }

    public void I_ButtonOnClick() {
    }

    public void ButtonOffClick() {
    }

    public void ButtonOnClick() {
        menuControll.CloseUI();
    }


}
