using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {


    // start button
    public void StartGameButton() {
        //TODO: player ���� ���� �������� ���� Ȯ�� �� !!!�ε�!!! �Ѿ� �� �� => �ε����� ���� ���� ����
        SceneManager.LoadScene("Game");
    }

    // backspace button
    public void BackButton() {
        SceneManager.LoadScene("Main");
    }

}
