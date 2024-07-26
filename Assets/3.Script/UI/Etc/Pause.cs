using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {
    private Canvas mainCanvas;
    [SerializeField] private Canvas pauseCanvas;
    private bool isPause = false;

    public Camera mainCamera;
    public Material blurMaterial;

    private RenderTexture renderTexture;
    private void Awake() {
        mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        if (pauseCanvas != null) {
            pauseCanvas.gameObject.SetActive(false); // Ensure pauseCanvas is inactive
        }

        blurCameraInit();
    }



    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            isPause = !isPause;
            if (isPause) {
                mainCanvas.gameObject.SetActive(false);
                pauseCanvas.gameObject.SetActive(true); 
                blurMaterial.SetFloat("_BlurValue", 1.0f);
            }
            else {
                mainCanvas.gameObject.SetActive(true);
                pauseCanvas.gameObject.SetActive(false);
                blurMaterial.SetFloat("_BlurValue", 0.0f);
            }
        }
    }



    private void blurCameraInit() {
        renderTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        if (mainCamera != null) {
            mainCamera.targetTexture = renderTexture;
            blurMaterial.SetTexture("_RenTex", renderTexture);
        }
        else {
            Debug.LogError("Main Camera is not assigned.");
        }
    }
}
