using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {


    public void StartGameButton() {
        SceneManager.LoadScene("Load");
    }

    // backspace button
    public void BackButton() {
        SceneManager.LoadScene("Main");
    }

}
