using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    private Canvas mainCanvas;
    [SerializeField]private Canvas pauseCanvas;
    private bool isPause;

    private void Awake() {
        isPause = false;
        mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        pauseCanvas = GameObject.Find("PauseCanvas").GetComponent<Canvas>();
        if (pauseCanvas != null) {
            pauseCanvas.gameObject.SetActive(false); // Ensure pauseCanvas is inactive
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            isPause = !isPause;
            if (isPause) {
                mainCanvas.gameObject.SetActive(false);
                pauseCanvas.gameObject.SetActive(true);
            }
            else {
                mainCanvas.gameObject.SetActive(true);
                pauseCanvas.gameObject.SetActive(false);
            }
        }
    }
}
