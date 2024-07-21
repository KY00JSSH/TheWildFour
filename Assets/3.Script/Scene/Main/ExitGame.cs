using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour {
    public void gameExit() {
        // Unity 에디터에서는 EditorApplication.isPlaying을 false로 설정하여 플레이 모드를 중지합니다.
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                // 빌드된 애플리케이션에서는 Application.Quit()을 호출하여 게임을 종료합니다.
                Application.Quit();
        #endif
    }
}
