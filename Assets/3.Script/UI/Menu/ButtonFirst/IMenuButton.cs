using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMenuButton {

    // ���� ��ư���� �����
    void I_ButtonOnClick();

    void I_ButtonOffClick();

    // �ش� ��ư�� ���콺 Ŭ���Ҷ� �����
    void ButtonOnClick();

    void ButtonOffClick();
}
