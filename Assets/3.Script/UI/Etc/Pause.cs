using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Pause : MonoBehaviour {

    private Canvas mainCanvas;
    [SerializeField] private Image pauseImg;
    static public bool GameIsPause { get; private set; }

    // 뒤로가기 버튼
    private Stack<GameObject> gameObjects = new Stack<GameObject>();
    // 버튼 저장
    public Button button;

    // menu button check
    private Menu_Controll menuControll;

    private void Awake() {
        mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        menuControll = FindObjectOfType<Menu_Controll>();
    }
    private void Start() {
        TogglePause(false);
    }

    private void Update() {
        if (!ShelterUI.isShelterUIOpen && !WorkShopUI.isWorkshopUIOpen && !menuControll.isMenuButtonOpen) {
            if (Input.GetKeyDown(KeyCode.Escape)) Escape();
        }

        if(GameIsPause) AudioManager.instance.StopBGM();
    }

    // 현재 객체의 자식들을 활성화 시킴
    private void pauseChildSetActive(GameObject gameObject) {
        foreach (Transform child in gameObject.transform) {
            child.gameObject.SetActive(true);
        }
    }

    // 현재 객체의 자식들을 비활성화 시킴
    private void pauseChildSetActiveOff(GameObject gameObject) {
        foreach (Transform child in gameObject.transform) {
            if(child == gameObject.transform.GetChild(0)) {
                if (child.TryGetComponent(out Text text)) continue;
            }            
            child.gameObject.SetActive(false);
        }
    }

    // 현재 객체의 부모의 자식들을 활성화 시킴
    private void pauseSilbingSetActive(GameObject gameObject) {
        foreach (Transform Silbing in gameObject.transform.parent) {
            Silbing.gameObject.SetActive(true);
        }
    }

    // 일시정지 기능 bool로 활성화 조절
    private void TogglePause(bool isPaused) {
        if (isPaused) {
            StartCoroutine(pauseImgAlphaChange());
            GameIsPause = true;
            mainCanvas.gameObject.SetActive(false);
            pauseChildSetActive(transform.gameObject);
            Time.timeScale = 0;
        }
        else {
            Time.timeScale = 1;

            mainCanvas.gameObject.SetActive(true);
            pauseChildSetActiveOff(transform.gameObject);
            gameObjects.Clear();
            gameObjects.Push(transform.gameObject);
            GameIsPause = false;

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

    // 저장 및 종료 -> 취소하기 + 저장 및 종료
    public void SaveEndFirst(GameObject gameObject) {
        gameObjects.Push(gameObject.GetComponentInParent<Button>().gameObject);
        gameObject.SetActive(true);
    }

    // 저장 및 종료 재확인
    public void SaveEnd() {
        // 종료 : 메인 씬으로 돌아가기
        SceneManager.LoadScene("Main");

        Save.Instance.MakeSave();

        Destroy(TimeManager.Instance.gameObject);
    }

    // 계속
    public void Continue() {
        GameIsPause = false;
        TogglePause(GameIsPause);
    }

    public void Cancle(GameObject gameObject) {
        gameObjects.Pop();
        gameObject.SetActive(false);
    }


    // 뒤로가기 + 취소하기
    public void Escape() {
        GameObject obj = gameObjects.Pop();
        pauseChildSetActiveOff(obj);
        // 본인만 남은거아니면 상위객체 재활성화
        if (gameObjects.Count > 1) {
            pauseSilbingSetActive(obj);
        }
        else {
            gameObjects.Push(transform.gameObject);
            GameIsPause = !GameIsPause;
            TogglePause(GameIsPause);
        }

    }

    private void OnDestroy() {

        Color color = pauseImg.color;
        color.a = 0;
        pauseImg.color = color;
    }

}
/*
뒤로가기 + 취소하기
1. 스택에 저장해서 사용할 버튼 지정
2. 버튼들에게 리스너 추가
3. 리스너로 버튼이 눌리면 onclick 메소드로 버튼 들고옴
4. 뒤로가기 혹은 취소하기에서 역행
 */