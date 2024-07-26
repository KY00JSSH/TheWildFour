using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour {
    private Canvas mainCanvas;
    [SerializeField] private Image pauseImg;
    [SerializeField] private GameObject buttons;
    private bool isPause = false;


    private void Awake() {
        mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    private void Update() {
        if (!ShelterUI.isShelterUIOpen && !WorkShopUI.isWorkshopUIOpen) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                isPause = !isPause;
                Debug.Log(isPause);
                TogglePause(isPause);
            }
        }
    }

    private void pauseChildSetActive() {
        foreach(Transform child in transform) {
            child.gameObject.SetActive(true);
        }
    }
    private void pauseChildSetActiveOff() {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(false);
        }
    }

    private void TogglePause(bool isPaused) {
        if (isPaused) {
            mainCanvas.gameObject.SetActive(false);
            pauseChildSetActive();
            StartCoroutine(pauseImgAlphaChange());
        }
        else {
            mainCanvas.gameObject.SetActive(true);
            pauseChildSetActiveOff();

            Color color = pauseImg.color;
            color.a = 0;
            pauseImg.color = color;
        }
    }

    private IEnumerator pauseImgAlphaChange() {
        Color color = pauseImg.color;
        while (color.a <= 0.55f) {
            color.a += Time.deltaTime;
            pauseImg.color = color;
            yield return null;
        }
    }

}
