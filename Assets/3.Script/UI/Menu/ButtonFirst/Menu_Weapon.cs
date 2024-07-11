using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Weapon : MonoBehaviour, IMenuButton {
    
    private Menu_Controll menuControll;
    private int CursorCount = 0;
    private void Awake() {
        menuControll = FindObjectOfType<Menu_Controll>();
    }

    // ���� ��ư���� �����
    public void I_ButtonOffClick() {
    }

    public void I_ButtonOnClick() {
        WeaponCursorSetting();
    }

    public void ButtonOffClick() {
    }

    public void ButtonOnClick() {
        menuControll.CloseUI(); 
        WeaponCursorSetting();
    }

    private void WeaponCursorSetting() {
        CursorCount++;
        if (CursorCount==2) {
            CursorCount = 0;
            transform.Find("Weapon_1").GetChild(1).gameObject.SetActive(false);
            transform.Find("Weapon_2").GetChild(1).gameObject.SetActive(true);
        }
        else {
            transform.Find("Weapon_2").GetChild(1).gameObject.SetActive(false);
            transform.Find("Weapon_1").GetChild(1).gameObject.SetActive(true);
        }
    }
}
