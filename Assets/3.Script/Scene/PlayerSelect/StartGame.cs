using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {


    // start button
    public void StartGameButton() {
        //TODO: player 저장 구조 정해지면 저장 확인 후 !!!로딩!!! 넘어 갈 것 => 로딩에서 메인 게임 시작
        SceneManager.LoadScene("Game");
    }

    // backspace button
    public void BackButton() {
        SceneManager.LoadScene("Main");
    }

}
