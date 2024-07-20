using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour {
    public void gameExit() {
        // Unity �����Ϳ����� EditorApplication.isPlaying�� false�� �����Ͽ� �÷��� ��带 �����մϴ�.
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                // ����� ���ø����̼ǿ����� Application.Quit()�� ȣ���Ͽ� ������ �����մϴ�.
                Application.Quit();
        #endif
    }
}
