using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCursor : MonoBehaviour
{
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
            Destroy(gameObject);
        }
        // else{DonDestroyOnLoad(gameObject);}
    }

    public static bool IsCusorOnButton(Button button) {
        Vector2 CursorPosition = button.GetComponent<RectTransform>().InverseTransformPoint(Input.mousePosition);
        return button.GetComponent<RectTransform>().rect.Contains(CursorPosition);
    }
}
