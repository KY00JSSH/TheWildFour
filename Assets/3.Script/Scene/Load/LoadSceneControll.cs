using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneControll : MonoBehaviour {
    // Start �޼��忡�� �񵿱� �ε� ����
    void Start() {
        StartCoroutine(LoadGameSceneAsync());
    }

    IEnumerator LoadGameSceneAsync() {
        // �񵿱������� GameScene �ε� ����
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Feature_MapObj");

        // �ε尡 �Ϸ�� ������ ���
        while (!asyncLoad.isDone) {
            yield return null;
        }
    }
}
