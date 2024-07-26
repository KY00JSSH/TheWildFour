using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour {

    private Canvas mainCanvas;
    [SerializeField] private Image pauseImg;
    private bool isPause = false;

    // �ڷΰ��� ��ư
    private Stack<GameObject> gameObjects = new Stack<GameObject>();
    // ��ư ����
    public Button button;

    // menu button check
    private Menu_Controll menuControll;

    private void Awake() {
        mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        menuControll = FindObjectOfType<Menu_Controll>();
    }
    private void Start() {
        gameObjects.Push(transform.gameObject);
        // ��ư ������ �߰�
        button.onClick.AddListener(() => OnButtonClick(button));
    }

    private void Update() {
        if (!ShelterUI.isShelterUIOpen && !WorkShopUI.isWorkshopUIOpen && !menuControll.isMenuButtonOpen) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                isPause = !isPause;
                TogglePause(isPause);
            }
        }
    }

    // ���� ��ü�� �ڽĵ��� Ȱ��ȭ ��Ŵ
    private void pauseChildSetActive(GameObject gameObject) {
        foreach (Transform child in gameObject.transform) {
            child.gameObject.SetActive(true);
        }
    }

    // ���� ��ü�� �ڽĵ��� ��Ȱ��ȭ ��Ŵ
    private void pauseChildSetActiveOff(GameObject gameObject) {
        foreach (Transform child in gameObject.transform) {
            if(child == gameObject.transform.GetChild(0)) {
                if (child.TryGetComponent(out Text text)) continue;
            }            
            child.gameObject.SetActive(false);

        }
    }

    // ���� ��ü�� �θ��� �ڽĵ��� Ȱ��ȭ ��Ŵ
    private void pauseSilbingSetActive(GameObject gameObject) {
        foreach (Transform Silbing in gameObject.transform.parent) {
            Silbing.gameObject.SetActive(true);
        }
    }

    // �Ͻ����� ��� bool�� Ȱ��ȭ ����
    private void TogglePause(bool isPaused) {
        if (isPaused) {
            StartCoroutine(pauseImgAlphaChange());
            mainCanvas.gameObject.SetActive(false);
            pauseChildSetActive(transform.gameObject);
        }
        else {

            Time.timeScale = 1;
            mainCanvas.gameObject.SetActive(true);
            pauseChildSetActiveOff(transform.gameObject);
            gameObjects.Clear();
            gameObjects.Push(transform.gameObject);
            isPause = false;

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
        Time.timeScale = 0;
    }

    // ���� �� ���� -> ����ϱ� + ���� �� ����
    public void SaveEndFirst(GameObject gameObject) {
        gameObject.SetActive(true);
    }

    // ���� �� ���� ��Ȯ��
    public void SaveEnd() {
        // ���� : ���� ������ ���ư���
        SceneManager.LoadScene("Main");
        //TODO: ����...
    }

    // ���
    public void Continue() {
        isPause = !isPause;
        TogglePause(isPause);
    }
    public void Cancle(GameObject gameObject) {
        gameObjects.Pop();
        gameObject.SetActive(false);
    }


    // �ڷΰ��� + ����ϱ�
    public void Escape() {
        Debug.Log(gameObjects.Count);
        GameObject obj = gameObjects.Pop();
        Debug.Log(obj.name);
        pauseChildSetActiveOff(obj);

        // ���θ� �����žƴϸ� ������ü ��Ȱ��ȭ
        if (gameObjects.Count != 0) {
            pauseSilbingSetActive(obj);
        }
        else {
            gameObjects.Push(transform.gameObject);
            isPause = !isPause;
            TogglePause(isPause);
        }
    }


    // ��ư Ŭ�� �� ȣ��Ǵ� �޼ҵ�
    private void OnButtonClick(Button clickedButton) {
        gameObjects.Push(clickedButton.gameObject);
        Debug.Log($"Button {clickedButton.name} added to stack. Stack size: {gameObjects.Count}");
    }


}
/*
�ڷΰ��� + ����ϱ�
1. ���ÿ� �����ؼ� ����� ��ư ����
2. ��ư�鿡�� ������ �߰�
3. �����ʷ� ��ư�� ������ onclick �޼ҵ�� ��ư ����
4. �ڷΰ��� Ȥ�� ����ϱ⿡�� ����
 */