using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMenuButton {

    // 상위 버튼에서 사용함
    void I_ButtonOnClick();

    void I_ButtonOffClick();

    // 해당 버튼을 마우스 클릭할때 사용함
    void ButtonOnClick();

    void ButtonOffClick();
}
