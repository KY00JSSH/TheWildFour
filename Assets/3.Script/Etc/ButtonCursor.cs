using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCursor : MonoBehaviour {
    private static ButtonCursor instance;
    public static ButtonCursor Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<ButtonCursor>();
            }
            return instance;
        }
    }
    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
            // return => 하단에 초기화가 있을경우
        }
    }
    public bool IsCusorOnButton(Button button) {
        Vector2 CursorPosition = button.GetComponent<RectTransform>().InverseTransformPoint(Input.mousePosition);
        return button.GetComponent<RectTransform>().rect.Contains(CursorPosition);
    }

}
