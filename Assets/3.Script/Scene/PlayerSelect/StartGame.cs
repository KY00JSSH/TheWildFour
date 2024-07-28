using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour {


    public void StartGameButton() {
        SceneManager.LoadScene("Load");
    }

    // backspace button
    public void BackButton() {
        SceneManager.LoadScene("Main");
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            foreach (var each in GetComponentsInChildren<Button>()) {
                if (each.name == "ButtonEsc") {
                    each.onClick.Invoke();
                    break;
                }
            }
        }
    }
}
